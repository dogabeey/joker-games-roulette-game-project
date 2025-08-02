using System.Collections.Generic;
using UnityEngine;

public class OddBetArea : BetArea
{
    public override int Priority => 1;
    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        // Example logic for OddBetArea
        List<string> numbers = new List<string>();
        for (int i = 1; i <= 36; i++)
        {
            if (i % 2 != 0) // Odd numbers
            {
                numbers.Add(i.ToString());
            }
        }
        return numbers;
    }
}
