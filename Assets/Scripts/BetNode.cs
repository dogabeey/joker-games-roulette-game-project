using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetNode : MonoBehaviour
{
    [Tooltip("How much your money is multiplied when you win the bet.")]
    [SerializeField] private float gainMultiplier;
    
    public RouletteNumber[] connectedNumbers;
}
