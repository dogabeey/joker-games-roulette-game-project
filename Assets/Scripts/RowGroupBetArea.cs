using System.Collections.Generic;
using UnityEngine;

public class  RowGroupBetArea : BetArea
{
    public override int Priority => 3;
    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x / 3f;
        float cellHeight = size.y;

        Vector2 localPos = worldPos - blCoords;
        int column = Mathf.FloorToInt(localPos.x / cellWidth);
        int row = 2 - Mathf.FloorToInt(localPos.y / cellHeight);

        List<string>[] columnList =
        {
            new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" },
            new List<string> { "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" },
            new List<string> { "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36" }
        };
        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            return null;
        }
        return columnList[column];
    }
}
