using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Button betButton;
    public Button lowerBetButton, increaseBetButton;
    public TMP_InputField betInputField;
    public TMP_InputField cheatInput;
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
        GameManager.Instance.currentBet += 10;
        if (GameManager.Instance.currentBet > GameManager.Instance.maxBet)
        {
            GameManager.Instance.currentBet = GameManager.Instance.maxBet;
        }
    }
    private void OnLowerBetButtonClicked()
    {
        GameManager.Instance.currentBet -= 10;
        if (GameManager.Instance.currentBet < 0)
        {
            GameManager.Instance.currentBet = 0;
        }
    }

    private void SetBetButton()
    {
        betButton.interactable = CanEnableBetButton();
    }

    private bool CanEnableBetButton()
    {
        return currentPayoutMultiplier != 0f 
            && betInputField.text != "" 
            && GameManager.Instance.gameState == GameState.betting;
    }

    private void SetBetValue()
    {
        GameManager.Instance.currentBet = betInputField.text != "" ? Convert.ToInt32(betInputField.text) : 0;
    }
    private void SetPayoutText()
    {
        if (currentPayoutMultiplier != 0 && betInputField.text != "")
        {
            payoutText.text = "$" + GameManager.Instance.currentBet * currentPayoutMultiplier;
        }
        else
        {
            payoutText.text = "No bet placed!";
        }
    }
}
