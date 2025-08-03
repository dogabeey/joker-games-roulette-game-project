using System.Collections.Generic;
using UnityEngine;

public class FirstHalfBetArea : BetArea
{
    public override string betNameID => "highLow"; // Assuming this is the ID for first half bets
    public override int Priority => 1;
    public override List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords)
    {
        List<string> numbers = new List<string>();
        if (worldPos.x >= blCoords.x && worldPos.x <= trCoords.x &&
            worldPos.y >= blCoords.y && worldPos.y <= trCoords.y)
        {
            for (int i = 1; i <= 18; i++)
            {
                numbers.Add(i.ToString());
            }
        }
        return numbers;
    }
}
