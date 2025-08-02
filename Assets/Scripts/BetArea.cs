using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BetArea : MonoBehaviour
{
    public Transform startRef;
    public Transform endRef;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // Draw a cube spannig from startPosition to endPosition
        Gizmos.DrawWireCube((startRef.position + endRef.position) / 2, endRef.position - startRef.position);
        Gizmos.DrawLine(startRef.position, endRef.position);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<string> result = GetRouletteNumber(worldPos, startRef.position, endRef.position);

            if (result != null)
                Debug.Log("Roulette Numbers: " + string.Join(", ", result));
        }
    }

    public abstract List<string> GetRouletteNumber(Vector2 worldPos, Vector2 blCoords, Vector2 trCoords);

}
