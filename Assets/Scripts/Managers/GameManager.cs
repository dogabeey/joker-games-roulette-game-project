using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager is a singleton class that manages the game state, including the current bet, payouts, and the roulette wheel and table instantiation. 
/// It handles the game logic for betting, winning, and losing, and keeps track of the player's net money, wins, and losses. It's the main class that controls the flow of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startingMoney;
    internal TableType defaultTableType = TableType.European;
    public Transform wheelParent;
    public Transform tableParent;
    public GameObject rouletteWheelPrefabAmerican;
    public GameObject rouletteWheelPrefabEuropean;
    public GameObject tablePrefabAmerican;
    public GameObject tablePrefabEuropean;
    public ParticleSystem winParticleSystem;
    [Header("Bet Settings")]
    public float currentBet;
    public float betIncreamental = 10f;
    public float maxBet;

    internal float currentPayoutMultiplier = 0f;
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

    public void CalculateGainBasedOnPayout( float betAmount)
    {
        bool isWin = TableManager.Instance.currentBetNumbers.Contains(winningNumber.ToString());

        if (currentPayoutMultiplier <= 0)
        {
            Debug.LogError("Payout multiplier is zero or negative, no gain calculated.");
            return;
        }
        float gain = betAmount * currentPayoutMultiplier;
        if (isWin)
        {
            winParticleSystem.Play();
            SoundManager.Instance.Play(Constants.SOUNDS.WIN_BET);

            Wins++;
            NetMoney += gain;
            Debug.Log($"Win! Gain: {gain}, New Net Money: {NetMoney}, Bet: {betAmount}, Payout Multiplier: {currentPayoutMultiplier}");
        }
        else
        {
            SoundManager.Instance.Play(Constants.SOUNDS.LOSE_BET);

            Losses++;
            NetMoney -= betAmount;
            Debug.Log($"Lose! Loss: {betAmount}, New Net Money: {NetMoney}, Payout Multiplier: {currentPayoutMultiplier}");
        }

        UIManager.Instance.ShowNumber(winningNumber);
    }
}

public enum GameState
{
    betting,
    spinning,
    intermission
}