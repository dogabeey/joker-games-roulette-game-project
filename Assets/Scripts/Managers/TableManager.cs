using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    internal List<RouletteNumber> numbers;
    internal PlayerChip playerChip;
    internal float currentBetAmount = 0f;
    internal float currentPayoutMultiplier = 1f;

    private void Start()
    {
        // Find all roulettenumbers in the scene and add them to numbers list.
        numbers = FindObjectsOfType<RouletteNumber>().ToList();
        playerChip = FindObjectOfType<PlayerChip>();
        if (playerChip == null)
        {
            Debug.LogError("PlayerChip not found in the scene. Please ensure there is a PlayerChip component in the scene.");
        }
    }
    private void OnEnable()
    {
        EventManager.StartListening(Constants.EVENTS.BET_PLACED, OnBetPlaced);
        EventManager.StartListening(Constants.EVENTS.BET_REMOVED, OnBetRemoved);
    }
    private void OnDisable()
    {
        EventManager.StopListening(Constants.EVENTS.BET_PLACED, OnBetPlaced);
        EventManager.StopListening(Constants.EVENTS.BET_REMOVED, OnBetRemoved);
    }
    public void OnBetPlaced(EventParam e)
    {
        e.paramDictionary.TryGetValue("numberList", out object rouletteNumbersObj);
        e.paramDictionary.TryGetValue("startRef", out object startRefObj);
        e.paramDictionary.TryGetValue("endRef", out object endRefObj);
        e.paramDictionary.TryGetValue("betArea", out object betAreaObj);
        e.paramDictionary.TryGetValue("betMutliplier", out object betMultiplierObj);
        List<string> eventNumbers = rouletteNumbersObj as List<string>;
        Transform startRef = startRefObj as Transform;
        Transform endRef = endRefObj as Transform;
        BetArea betArea = betAreaObj as BetArea;
        float betMultiplier = (float) betMultiplierObj;

        if (e.paramDictionary != null)
        {
            // Toggle number indicators
            ToggleSelectedNumbers(eventNumbers);
            // Change the current payout multiplier
            currentPayoutMultiplier = betMultiplier;
            // Move player chip middle of the start and end reference points if outer bet, otherwise place it between all selected numbers.
            PositionPlayerChip(eventNumbers, startRef, endRef, betArea);
        }
    }
    public void OnBetRemoved(EventParam e)
    {
        ToggleSelectedNumbers(new List<string>()); // Deselect all numbers
        currentPayoutMultiplier = 0;
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
