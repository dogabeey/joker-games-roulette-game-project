using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BetArea : MonoBehaviour
{
    public Transform startRef;
    public Transform endRef;

    // Priorty decides which bet area's returned value is used when multiple bet areas overlap.
    public abstract int Priority { get; }

    private void OnEnable()
    {
        EventManager.StartListening("PlayerChipDropped", OnPlayerChipDropped);
    }
    private void OnDisable()
    {
        EventManager.StopListening("PlayerChipDropped", OnPlayerChipDropped);
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
                { "number", result }
            };
            EventParam e2 = new EventParam(paramDictionary: p);
            EventManager.TriggerEvent("BetPlaced", e2);
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
