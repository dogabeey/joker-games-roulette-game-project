using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used for Column bets in Roulette.
/// </summary>
public class ColumnBetArea : BetArea
{
    public override int Priority => 2;
    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {

        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x;
        float cellHeight = size.y / 3f;

        Vector2 localPos = worldPos - blCoords;
        int column = Mathf.FloorToInt(localPos.x / cellWidth);
        int row = 2 - Mathf.FloorToInt(localPos.y / cellHeight);

        List<string>[] rowList =
        {
            new List<string> { "3", "6", "9", "12", "15", "18", "21", "24", "27", "30", "33", "36" },
            new List<string> { "2", "5", "8", "11", "14", "17", "20", "23", "26", "29", "32", "35" },
            new List<string> { "1", "4", "7", "10", "13", "16", "19", "22", "25", "28", "31", "34" },
        };

        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            Debug.LogWarning("Clicked outside of the Black Numbers Bet Area.");
            return null;
        }
        return rowList[row];
    }
}
