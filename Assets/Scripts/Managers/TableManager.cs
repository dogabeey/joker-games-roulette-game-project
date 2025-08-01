using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    // This list holds all the roulette numbers in the scene.
    [SerializeField] private List<RouletteNumber> numbers;

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

    public List<RouletteNumber> GetSelectedNumbers()
    {
        return numbers.Where(n => n.IsSelected).ToList();
    }
}
