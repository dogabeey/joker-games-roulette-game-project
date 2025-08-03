using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BetArea : MonoBehaviour
{
    public Transform startRef;
    public Transform endRef;

    public abstract string betNameID { get; }
    // Priorty decides which bet area's returned value is used when multiple bet areas overlap.
    public abstract int Priority { get; }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EVENTS.PLAYER_CHIP_PLACED, OnPlayerChipDropped);
    }
    private void OnDisable()
    {
        EventManager.StopListening("PlayerChipDropped", OnPlayerChipDropped);
    }

    /// <summary>
    /// Returns the payout multiplier based on the bet type and the number of inner bets.
    /// </summary>
    /// <param name="betType">Name of the bet type based on the betNameID</param>
    /// <param name="innerBetNumbers">If one of the inner bets, how many numbers it includes</param>
    /// <returns></returns>
    public static float GetPayoutMultiplierByBetType(string betType, float innerBetNumbers)
    {
        switch (betType)
        {
            case "number":
                if (innerBetNumbers == 1)
                    return Constants.VALUES.STRAIGHT_BET_PAY;
                else if (innerBetNumbers == 2)
                    return Constants.VALUES.SPLIT_BET_PAY;
                else if (innerBetNumbers == 3)
                    return Constants.VALUES.STREET_BET_PAY;
                else if (innerBetNumbers == 4)
                    return Constants.VALUES.CORNER_BET_PAY;
                else
                    return 1;
            case "redBlack":
                return Constants.VALUES.RED_BLACK_BET_PAY;
            case "oddEven":
                return Constants.VALUES.ODD_EVEN_BET_PAY;
            case "highLow":
                return Constants.VALUES.HIGH_LOW_BET_PAY;
            case "dozen":
                return Constants.VALUES.DOZEN_BET_PAY;
            case "column":
                return Constants.VALUES.COLUMN_BET_PAY;
            default:
                Debug.LogWarning($"Unknown bet type: {betType}");
                return 0f;
        }
    }

    public void OnPlayerChipDropped(EventParam e)
    {
        Vector2 worldPos = e.paramVector2;
        List<string> result = GetRouletteNumber(worldPos, startRef.position, endRef.position);
        if (result != null && result.Count > 0)
        {
            Debug.Log($"Bet Area: {this.name} - Result: {string.Join(", ", result)}");
            Dictionary<string, object> p = new Dictionary<string, object>
            {
                { "numberList", result },
                { "startRef", startRef },
                { "endRef", endRef },
                { "betArea", this },
                { "betMutliplier", GetPayoutMultiplierByBetType(betNameID, result.Count) }
            };
            EventParam e2 = new EventParam(paramDictionary: p);
            EventManager.TriggerEvent(Constants.EVENTS.BET_PLACED, e2);
        }
    }

    private void OnDrawGizmos()
    {
        if(startRef == null || endRef == null)
            return;

        Gizmos.color = Color.cyan;
        // Draw a cube spannig from startPosition to endPosition
        Gizmos.DrawWireCube((startRef.position + endRef.position) / 2, endRef.position - startRef.position);

        Gizmos.DrawLine(startRef.position, endRef.position);
        Gizmos.DrawLine(new Vector3(startRef.position.x, endRef.position.y, 0), new Vector3(endRef.position.x, startRef.position.y, 0));
    }

    public abstract List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords);

}
