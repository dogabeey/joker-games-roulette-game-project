using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // This list holds all the roulette numbers in the scene.
    public List<RouletteNumber> numbers;

    public List<RouletteNumber> GetSelectedNumbers()
    {
        return numbers.Where(n => n.IsSelected).ToList();
    }
    public void ToggleNumbers(params int[] selectedNumbers)
    {
        // First, disable all numbers.
        foreach (RouletteNumber number in numbers)
        {
            number.IsSelected = false;
        }
        // Then, enable only the selected numbers.
        foreach (RouletteNumber number in numbers)
        {
            if (selectedNumbers.Contains(number.number))
            {
                number.IsSelected = true;
            }
        }
    }
}
