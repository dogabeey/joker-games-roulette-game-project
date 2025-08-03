using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteNumber : MonoBehaviour
{
    public string numberString;
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

    
    private void OnValidate()
    {
        gameObject.name = "Number " + numberString;
    }

    public void ToggleNumber(bool isSelected)
    {
        selectionIndicator.enabled = isSelected;
    }
}