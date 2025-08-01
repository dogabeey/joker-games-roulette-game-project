using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerChip : MonoBehaviour
{
    [Tooltip("Minimum detection range for the player chip to see the bet nodes.")]
    public float minBetNodeRange;

    private void Update()
    {
        BetNode betNode = GetClosestNode();
        EvaluateNodeSelection(betNode);
    }

    private void EvaluateNodeSelection(BetNode betNode)
    {
        if (betNode != null)
        {
            // If the player is close enough to a bet node, select the numbers on that node.
            SelectNumbersOnNode(betNode);
        }
        else
        {
            // If no bet node is close enough, deselect all numbers.
            TableManager.Instance.ToggleNumbers();
        }
    }

    private BetNode GetClosestNode()
    {
        List<BetNode> betNodes = TableManager.Instance.betNodes;

        BetNode closestNode = null;
        float closestDistance = float.MaxValue;
        foreach (BetNode node in betNodes)
        {
            float distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance < closestDistance && distance < minBetNodeRange)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    private void SelectNumbersOnNode(BetNode closestNode)
    {
        TableManager.Instance.ToggleNumbers(
            closestNode.connectedNumbers.Select(n => n.number).ToArray()
        );
    }
}