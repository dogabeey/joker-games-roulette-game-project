using System;
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
        //EventManager.StartListening(Constants.EVENTS.PLAYER_CHIP_PLACED, OnPlayerChipDropped);
    }
    private void OnDisable()
    {
        //EventManager.StopListening(Constants.EVENTS.PLAYER_CHIP_PLACED, OnPlayerChipDropped);
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

    public abstract List<string> GetRouletteNumbers(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords);

    internal bool IsPositionInBetArea(Vector2 worldPos)
    {
        Vector2 blCoords = startRef.position;
        Vector2 trCoords = endRef.position;
        // Check if the world position is within the bounds of the bet area
        return worldPos.x >= blCoords.x && worldPos.x <= trCoords.x &&
               worldPos.y >= blCoords.y && worldPos.y <= trCoords.y;
    }
}
