using System.Collections.Generic;
using UnityEngine;

public class SecondHalfBetArea : BetArea
{
    public override string betNameID => "highLow"; // Assuming this is the ID for second half bets
    public override int Priority => 2;
    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        List<string> numbers = new List<string>();
        if (worldPos.x >= blCoords.x && worldPos.x <= trCoords.x &&
            worldPos.y >= blCoords.y && worldPos.y <= trCoords.y)
        {
            for (int i = 19; i <= 36; i++)
            {
                numbers.Add(i.ToString());
            }
        }
        return numbers;
    }
}