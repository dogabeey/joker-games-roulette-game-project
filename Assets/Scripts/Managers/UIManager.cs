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
    public TMP_InputField betInputField;
    public TMP_Text payoutText;

    private float currentPayoutMultiplier = 0f;

    // Start is called before the first frame update
    void Start()
    {
        betInputField.onValueChanged.AddListener(SetPayoutText);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EVENTS.BET_PLACED, OnPayoutMultiplierChanged);
        EventManager.StartListening(Constants.EVENTS.BET_REMOVED, OnPayoutMultiplierChanged);
    }
    private void OnDisable()
    {
        EventManager.StopListening(Constants.EVENTS.BET_PLACED, OnPayoutMultiplierChanged);
        EventManager.StopListening(Constants.EVENTS.BET_REMOVED, OnPayoutMultiplierChanged);
    }
    private void OnPayoutMultiplierChanged(EventParam e)
    {
        if(e.paramDictionary != null)
        {
            e.paramDictionary.TryGetValue("betMutliplier", out object betMultiplierObj);
            float betMultiplier = (float)betMultiplierObj;
            currentPayoutMultiplier = betMultiplier;
        }
        else
        {
            currentPayoutMultiplier = 0f;
        }
        SetPayoutText(betInputField.text);
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
