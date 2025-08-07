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
    public TMP_InputField betInputField;
    public TMP_InputField cheatInput;
    public TMP_Text payoutText;

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

        SetBetButton();
    }

    private void Update()
    {
        SetPayoutText();
        SetBetButton();
    }

    public void SetPayout(float betMultiplier)
    {
        currentPayoutMultiplier = betMultiplier;

    }

    private void OnBetButtonClicked()
    {
        int number = cheatInput.text != "" ? Convert.ToInt32(cheatInput.text) : -100;
        RouletteWheelController.Instance.PlayBet(number);
    }

    private void SetBetButton()
    {
        betButton.interactable = currentPayoutMultiplier != 0f && betInputField.text != "";
    }

    private void SetPayoutText()
    {
        if (currentPayoutMultiplier != 0 && betInputField.text != "")
        {
            payoutText.text = "$" + Mathf.RoundToInt(Convert.ToInt32(betInputField.text) * currentPayoutMultiplier);
        }
        else
        {
            payoutText.text = "No bet placed!";
        }
    }
}
