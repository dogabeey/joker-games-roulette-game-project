using System.Collections.Generic;
using UnityEngine;

public class RedNumbersBetArea : BetArea
{
    public override int Priority => 1;

    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        // Define the red numbers in roulette
        List<string> redNumbers = new List<string>
        {
            "1", "3", "5", "7", "9", "12", "14", "17", "19", "21",
            "23", "25", "27", "30", "32", "34", "36"
        };
        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            Debug.LogWarning("Clicked outside of the Red Numbers Bet Area.");
            return null;
        }
        return redNumbers;
    }
}