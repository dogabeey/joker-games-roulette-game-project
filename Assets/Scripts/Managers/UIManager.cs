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
        betInputField.onValueChanged.AddListener(SetPayoutText);
        betButton.onClick.AddListener(OnBetButtonClicked);

        SetBetButton();
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    public void SetPayout(float betMultiplier)
    {
        currentPayoutMultiplier = betMultiplier;

        SetPayoutText(betInputField.text);
        SetBetButton();
    }

    private void OnBetButtonClicked()
    {
        TableManager.Instance.PlayBet(Convert.ToInt32(cheatInput.text));
    }

    private void SetBetButton()
    {
        betButton.interactable = currentPayoutMultiplier != 0f;
    }

    private void SetPayoutText(string betText)
    {
        if (currentPayoutMultiplier != 0 && betText != "")
        {
            payoutText.text = "$" + Mathf.RoundToInt(Convert.ToInt32(betText) * currentPayoutMultiplier);
        }
        else
        {
            payoutText.text = "No bet placed!";
        }
    }
}
