using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerChip : MonoBehaviour
{
    private Camera mainCamera;
    private Plane dragPlane;
    private Vector3 offset;
    private bool isDragging = false;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        // Create a plane in XY direction at the object's position
        dragPlane = new Plane(Vector3.forward, transform.position); // Normal = Z axis (XY plane)

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            offset = transform.position - hitPoint;
            isDragging = true;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        TableManager.Instance.DetectPlacedBet(transform.position);
    }

    void Update()
    {
        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                transform.position = hitPoint + offset;
            }
        }
    }
}