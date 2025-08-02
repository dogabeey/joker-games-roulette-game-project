using System.Collections.Generic;
using UnityEngine;

public class DoubleZeroBetArea : BetArea
{
    public override int Priority => 1;

    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x / 3f;
        float cellHeight = size.y / 3f;
        Vector2 localPos = worldPos - blCoords;
        int column = Mathf.FloorToInt(localPos.x / cellWidth);
        int row = 2 - Mathf.FloorToInt(localPos.y / cellHeight);
        if (column == 1 && row == 1)
        {
            return new List<string> { "00" };
        }
        Debug.LogWarning("Clicked outside of the Double Zero Bet Area.");
        return null;
    }
}