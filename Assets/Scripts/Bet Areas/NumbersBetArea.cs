using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NumbersBetArea : BetArea
{
    public float inBetweenDistance = 0.25f;

    public override string betNameID => "number";
    public override int Priority => 1;

    public override List<string> GetRouletteNumbers(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {

        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x / 12f;
        float cellHeight = size.y / 3f;

        Vector2 localPostNW = worldPos - blCoords + new Vector2(-inBetweenDistance, inBetweenDistance);
        Vector2 localPostNE = worldPos - blCoords + new Vector2(inBetweenDistance, inBetweenDistance);
        Vector2 localPostSW = worldPos - blCoords + new Vector2(-inBetweenDistance, -inBetweenDistance);
        Vector2 localPostSE = worldPos - blCoords + new Vector2(inBetweenDistance, -inBetweenDistance);

        List<Vector2> posList = new List<Vector2>
        {
            localPostNW,
            localPostNE,
            localPostSW,
            localPostSE
        };
        List<int> columns = new List<int>();
        List<int> rows = new List<int>();

        foreach (Vector2 localPos in posList)
        {
            int column = Mathf.FloorToInt(localPos.x / cellWidth);
            int row = 2 - Mathf.FloorToInt(localPos.y / cellHeight);
            // If out of bounds, return null or empty
            if (column < 0 || column > 11 || row < 0 || row > 2)
            {
                // If any part of the position is out of bounds, clear the lists and return null
                columns.Clear();
                rows.Clear();
                break;
            }
            else
            {
                columns.Add(column);
                rows.Add(row);
            }
        }

        string[,] numberGrid = new string[3, 12] {
                { "3", "6", "9", "12", "15", "18", "21", "24", "27", "30", "33", "36" },
                { "2", "5", "8", "11", "14", "17", "20", "23", "26", "29", "32", "35" },
                { "1", "4", "7", "10", "13", "16", "19", "22", "25", "28", "31", "34" }
            };

        List<string> results = new List<string>();

        for (int i = 0; i < columns.Count; i++)
        {
            int column = columns[i];
            int row = rows[i];
            results.Add(numberGrid[row, column]);
        }

        results = results.Distinct().ToList();
        return results;
    }
}
