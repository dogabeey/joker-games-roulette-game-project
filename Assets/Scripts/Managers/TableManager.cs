using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public List<RouletteNumber> numbers;

    public static TableManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        // Find all roulettenumbers in the scene and add them to numbers list.
        numbers = FindObjectsOfType<RouletteNumber>().ToList();
    }
    private void OnEnable()
    {
        EventManager.StartListening("BetPlaced", OnBetPlaced);
    }
    private void OnDisable()
    {
        EventManager.StopListening("BetPlaced", OnBetPlaced);
    }
    public void OnBetPlaced(EventParam e)
    {
        // Get paramDictionary's "number" value
        if (e.paramDictionary != null && e.paramDictionary.TryGetValue("number", out object value) && value is List<string> eventNumbers)
        {
            foreach (RouletteNumber number in numbers)
            {
                number.ToggleNumber(false); // Deselect all numbers
            }
            // Check if the numbers are valid
            foreach (string number in eventNumbers)
            {
                // Find the Roulette number where the number value is number
                RouletteNumber selectedNumber = numbers.FirstOrDefault(n => n.numberString == number);
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
}
