using System.Collections.Generic;
using UnityEngine;

public class BlackNumbersBetArea : BetArea
{
    public override int Priority => 1;

    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        // Define the black numbers in roulette
        List<string> blackNumbers = new List<string>
        {
            "2", "4", "6", "8", "10", "11", "13", "15", "17", "19",
            "20", "22", "24", "26", "29", "31", "33", "35"
        };
        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            return null;
        }
        return blackNumbers;
    }
}
