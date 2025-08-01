using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteNumber : MonoBehaviour
{
    public int number;
    public Renderer selectionIndicator;

    private bool isSelected;

    public bool IsSelected
    {
        get 
        { 
            return isSelected; 
        }
        set
        {
            isSelected = value;
            ToggleNumber(isSelected);
        }
    }

    private void Start()
    {
        gameObject.name = "Number " + number.ToString();
    }

    public void ToggleNumber(bool isSelected)
    {
        selectionIndicator.enabled = isSelected;
    }
}