using System.Collections.Generic;
using UnityEngine;

public class DoubleZeroBetArea : BetArea
{
    public override string betNameID => "number";
    public override int Priority => 1;

    public override List<string> GetRouletteNumbers(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        Vector2 size = trCoords - blCoords;
        float cellWidth = size.x;
        float cellHeight = size.y;

        // Check if the world position is within the bounds of the bet area
        if (worldPos.x < blCoords.x || worldPos.x > trCoords.x || worldPos.y < blCoords.y || worldPos.y > trCoords.y)
        {
            return null;
        }
        
        return new List<string> { "00" };
    }
}