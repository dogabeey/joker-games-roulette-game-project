using System.Collections.Generic;
using UnityEngine;

public class RedNumbersBetArea : BetArea
{
    public override string betNameID => "redBlack"; // Assuming this is the ID for red bets
    public override int Priority => 1;

    public override List<string> GetRouletteNumbers(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        // Define the red numbers in roulette
        List<string> redNumbers = new List<string>
        {
            "1", "3", "5", "7", "9", "12", "14", "16", "18", "21",
            "23", "25", "27", "28", "30", "32", "34", "36"
        };
        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            return null;
        }
        return redNumbers;
    }
}