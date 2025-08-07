using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance;

    public TableType tableType;

    internal List<RouletteNumber> numbers;
    internal List<BetArea> betAreas;
    internal PlayerChip playerChip;
    internal float currentBetAmount = 0f;
    internal float currentPayoutMultiplier = 1f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Find all roulettenumbers in the scene and add them to numbers list.
        numbers = FindObjectsOfType<RouletteNumber>().ToList();
        betAreas = FindObjectsOfType<BetArea>().ToList();
        playerChip = FindObjectOfType<PlayerChip>();

        if (playerChip == null)
        {
            Debug.LogError("PlayerChip not found in the scene. Please ensure there is a PlayerChip component in the scene.");
        }
    }

    public void DetectPlacedBet(Vector2 placedWorldPos)
    {
        // Find the bet area that contains the world position
        BetArea betArea = betAreas.FirstOrDefault(b => b.IsPositionInBetArea(placedWorldPos)) ?? null;

        if (betArea == null)
        {
            ToggleSelectedNumbers(new List<string>()); // Deselect all numbers
            currentPayoutMultiplier = 0;
            return;
        }
        Transform startRef = betArea.startRef;
        Transform endRef = betArea.endRef;
        string betNameID = betArea.betNameID;

        List<string> result = betArea.GetRouletteNumbers(placedWorldPos, startRef.position, endRef.position);
        if (result != null)
        {
            if (result.Count > 0)
            {
                float payoutMultiplier = GetPayoutMultiplierByBetType(betNameID, result.Count);
                UIManager.Instance.SetPayout(payoutMultiplier); // Set the payout multiplier in the UI
                ToggleSelectedNumbers(result); // Select the numbers
                PositionPlayerChip(result, startRef, endRef, betArea); // Position the player chip
            }
            else
            {
                UIManager.Instance.SetPayout(0); // No valid bet, set payout to 0
                ToggleSelectedNumbers(new List<string>()); // Deselect all numbers
                currentPayoutMultiplier = 0;
            }
        }
    }

    private void PositionPlayerChip(List<string> eventNumbers, Transform startRef, Transform endRef, BetArea betArea)
    {
        // TODO: Placement doesn't work properly for column and row group.
        if (betArea.betNameID == "number")
        {
            // If the bet is a number bet, place the chip between all selected numbers
            Vector3 averagePosition = Vector3.zero;
            foreach (string number in eventNumbers)
            {
                RouletteNumber selectedNumber = numbers.FirstOrDefault(n => n.numberString == number);
                if (selectedNumber != null)
                {
                    averagePosition += selectedNumber.transform.position;
                }
                else
                {
                    Debug.LogWarning($"Number {number} not found in table numbers.");
                }
            }
            averagePosition /= eventNumbers.Count;
            playerChip.transform.position = new Vector3(averagePosition.x, averagePosition.y, playerChip.transform.position.z);
        }
        else
        {
            // For outer bets, place the chip in the middle of startRef and endRef
            Vector3 midPoint = (startRef.position + endRef.position) / 2f;
            playerChip.transform.position = new Vector3(midPoint.x, midPoint.y, playerChip.transform.position.z);
        }
    }

    /// <summary>
    /// Returns the payout multiplier based on the bet type and the number of inner bets.
    /// </summary>
    /// <param name="betType">Name of the bet type based on the betNameID</param>
    /// <param name="innerBetNumbers">If one of the inner bets, how many numbers it includes</param>
    /// <returns></returns>
    public static float GetPayoutMultiplierByBetType(string betType, float innerBetNumbers)
    {
        switch (betType)
        {
            case "number":
                if (innerBetNumbers == 1)
                    return Constants.VALUES.STRAIGHT_BET_PAY;
                else if (innerBetNumbers == 2)
                    return Constants.VALUES.SPLIT_BET_PAY;
                else if (innerBetNumbers == 3)
                    return Constants.VALUES.STREET_BET_PAY;
                else if (innerBetNumbers == 4)
                    return Constants.VALUES.CORNER_BET_PAY;
                else
                    return 1;
            case "redBlack":
                return Constants.VALUES.RED_BLACK_BET_PAY;
            case "oddEven":
                return Constants.VALUES.ODD_EVEN_BET_PAY;
            case "highLow":
                return Constants.VALUES.HIGH_LOW_BET_PAY;
            case "dozen":
                return Constants.VALUES.DOZEN_BET_PAY;
            case "column":
                return Constants.VALUES.COLUMN_BET_PAY;
            default:
                Debug.LogWarning($"Unknown bet type: {betType}");
                return 0f;
        }
    }


    private void ToggleSelectedNumbers(List<string> eventNumbers)
    {
        foreach (RouletteNumber number in numbers)
        {
            number.ToggleNumber(false); // Deselect all numbers
        }
        // Check if the numbers are valid
        List<RouletteNumber> selectedNumbers = new List<RouletteNumber>();
        foreach (string number in eventNumbers)
        {
            // Find the Roulette number where the number value is number
            RouletteNumber selectedNumber = numbers.FirstOrDefault(n => n.numberString == number);
            selectedNumbers.Add(selectedNumber);
            if (selectedNumber != null)
            {
                selectedNumber.ToggleNumber(true); // Select the number
            }
            else
            {
                Debug.LogWarning($"Number {number} not found in table numbers.");
            }
        }
    }
}

public enum TableType
{
    European,
    American
}