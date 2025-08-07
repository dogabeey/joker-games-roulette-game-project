using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startingMoney;
    public TableType defaultTableType;

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
    public TableType CurrenrTableType
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum GameState
{
    betting,
    spinning,
}