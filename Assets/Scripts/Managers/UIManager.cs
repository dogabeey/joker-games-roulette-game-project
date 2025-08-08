using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Button betButton;
    public Button lowerBetButton, increaseBetButton;
    public TMP_InputField betInputField;
    public TMP_InputField cheatInput;
    public TMP_Text moneyText;
    public TMP_Text payoutText;
    public TMP_Text winningNumber;

    private float currentPayoutMultiplier = 0f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        betButton.onClick.AddListener(OnBetButtonClicked);
        lowerBetButton.onClick.AddListener(OnLowerBetButtonClicked);
        increaseBetButton.onClick.AddListener(OnIncreaseBetButtonClicked);

        SetBetButton();
    }

    private void Update()
    {
        SetBetValue();
        SetPayoutText();
        SetBetButton();
        UpdateMoney();
    }

    public void SetPayout(float betMultiplier)
    {
        currentPayoutMultiplier = betMultiplier;

    }

    public void ShowNumber(int number)
    {
        winningNumber.text = "" + GameManager.Instance.winningNumber;
    }
    public void HideWinningNumber()
    {
        winningNumber.text = "";
    }

    private void OnBetButtonClicked()
    {
        int number = cheatInput.text != "" ? Convert.ToInt32(cheatInput.text) : -100;
        RouletteWheelController.Instance.PlayBet(number);
    }
    private void OnIncreaseBetButtonClicked()
    {
        float incrementalMultiplier;
        if (GameManager.Instance.currentBet < 100)
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental;
        }
        else if (GameManager.Instance.currentBet < 1000)
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental * 10;
        }
        else
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental * 100;
        }

        GameManager.Instance.currentBet += incrementalMultiplier;
        if (GameManager.Instance.currentBet > GameManager.Instance.maxBet)
        {
            GameManager.Instance.currentBet = GameManager.Instance.maxBet;
        }
    }
    private void OnLowerBetButtonClicked()
    {
        float incrementalMultiplier;
        if (GameManager.Instance.currentBet < 200)
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental;
        }
        else if (GameManager.Instance.currentBet < 2000)
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental * 10;
        }
        else
        {
            incrementalMultiplier = GameManager.Instance.betIncreamental * 100;
        }

        GameManager.Instance.currentBet -= incrementalMultiplier;
        if (GameManager.Instance.currentBet < 0)
        {
            GameManager.Instance.currentBet = 0;
        }
    }

    private void SetBetButton()
    {
        betButton.interactable = CanEnableBetButton();
    }

    private void UpdateMoney()
    {
        moneyText.text = "$" + ConvertDecimalToKMB(GameManager.Instance.NetMoney);
        moneyText.color = GameManager.Instance.NetMoney < 0 ? Color.red : Color.white;
    }

    private bool CanEnableBetButton()
    {
        return currentPayoutMultiplier != 0f 
            && GameManager.Instance.currentBet > 0
            && GameManager.Instance.gameState == GameState.betting;
    }

    private void SetBetValue()
    {
        betInputField.text = ConvertDecimalToKMB(GameManager.Instance.currentBet);
    }
    private string ConvertDecimalToKMB(float value)
    {
        float absValue = Mathf.Abs(value); // Ensure value is positive for formatting

        if (absValue >= 1000 && absValue < 1000000)
        {
            return (absValue / 1000).ToString("F1") + "K";
        }
        else if (absValue >= 1000000)
        {
            return (absValue / 1000000).ToString("F1") + "M";
        }
        else
        {
            return absValue.ToString("");
        }
    }
    private void SetPayoutText()
    {
        if (currentPayoutMultiplier == 0)
        {
            payoutText.text = "No chip placed!";
        }
        if (GameManager.Instance.currentBet <= 0)
        {
            payoutText.text = "No bet placed!";
        }

        if (currentPayoutMultiplier != 0 && GameManager.Instance.currentBet > 0)
        {
            payoutText.text = "PAYOUT: " + "$" + GameManager.Instance.currentBet * currentPayoutMultiplier;
        }

    }
}
