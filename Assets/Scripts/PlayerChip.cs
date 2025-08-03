using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerChip : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Tooltip("Minimum detection range for the player chip to see the bet nodes.")]
    public float minBetNodeRange;

    private Vector3 lastPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag: " + eventData.position);
        // Move the object on XY plane
        lastPosition = transform.position;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        newPosition.z = lastPosition.z; // Keep the original Z position
        transform.position = newPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        lastPosition = transform.position;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        newPosition.z = lastPosition.z; // Keep the original Z position
        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventParam param = new EventParam();
        param.paramVector3 = transform.position;
        EventManager.TriggerEvent("PLAYER_CHIP_PLACED", param);
    }
}