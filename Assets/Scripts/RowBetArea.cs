using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class RowBetArea : BetArea
{
    public override int Priority => 2;

    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        

        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x / 12f;
        float cellHeight = size.y;

        Vector2 localPos = worldPos - blCoords;
        int column = Mathf.FloorToInt(localPos.x / cellWidth);
        int row = 2 - Mathf.FloorToInt(localPos.y / cellHeight);

        List<string>[] columnList =
        {
            new List<string> { "3", "2", "1" },
            new List<string> { "6", "5", "4" },
            new List<string> { "9", "8", "7" },
            new List<string> { "12", "11", "10" },
            new List<string> { "15", "14", "13" },
            new List<string> { "18", "17", "16" },
            new List<string> { "21", "20", "19" },
            new List<string> { "24", "23", "22" },
            new List<string> { "27", "26", "25" },
            new List<string> { "30", "29", "28" },
            new List<string> { "33", "32", "31" },
            new List<string> { "36", "35", "34" }
        };
        if (column < 0 || column >= columnList.Length || row < 0 || row >= columnList[0].Count)
        {
            return null;
        }
        return columnList[column];
    }
}
