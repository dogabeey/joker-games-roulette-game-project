using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startingMoney;
    public TableType defaultTableType;
    public Transform wheelParent;
    public Transform tableParent;
    public GameObject rouletteWheelPrefabAmerican;
    public GameObject rouletteWheelPrefabEuropean;
    public GameObject tablePrefabAmerican;
    public GameObject tablePrefabEuropean;

    internal int winningNumber = -2;
    internal GameState gameState = GameState.betting;

    public float NetMoney
    {
        get
        {
            return PlayerPrefs.GetFloat("NetMoney", startingMoney);
        }
        set
        {
            PlayerPrefs.SetFloat("NetMoney", value);
        }
    }
    public int Wins
    {
        get
        {
            return PlayerPrefs.GetInt("Wins", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Wins", value);
        }
    }
    public int Losses
    {
        get
        {
            return PlayerPrefs.GetInt("Losses", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Losses", value);
        }
    }
    public TableType CurrentTableType
    {
        get
        {
            return (TableType)PlayerPrefs.GetInt("CurrentTableType", (int)defaultTableType);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentTableType", (int)value);
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(CurrentTableType == TableType.European)
        {
            Instantiate(rouletteWheelPrefabEuropean, wheelParent);
            Instantiate(tablePrefabEuropean, tableParent);
        }
        else
        {
            Instantiate(rouletteWheelPrefabAmerican, wheelParent);
            Instantiate(tablePrefabAmerican, tableParent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateGainBasedOnPayout(float betAmount, float payoutMultiplier)
    {
        if (payoutMultiplier <= 0)
        {
            Debug.LogError("Payout multiplier is zero or negative, no gain calculated.");
            return;
        }
        float gain = betAmount * payoutMultiplier;
        if (gain > 0)
        {
            Wins++;
            NetMoney += gain;
            Debug.Log($"Win! Gain: {gain}, New Net Money: {NetMoney}");
        }
        else
        {
            Losses++;
            NetMoney -= betAmount;
            Debug.Log($"Loss! Gain: {gain}, New Net Money: {NetMoney}");
        }

        UIManager.Instance.ShowNumber(winningNumber);
    }
}

public enum GameState
{
    betting,
    spinning,
}