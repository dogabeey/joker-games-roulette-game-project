using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouletteWheelController : MonoBehaviour
{
    [Header("References")]
    public Transform ballParent; // Used for rotating the ball. Ball itself isn't rotated. 
    public Transform ballTransform;
    public Transform wheelTransform;
    [Header("Wheel Settings")]
    public List<int> wheelNumbersEuropean = new List<int> 
    { 
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36,
        11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9,
        22, 18, 29, 7, 28, 12, 35, 3, 26 
    };
    public List<int> wheelNumbersAmerican = new List<int>
    {
        0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36,
        11, 30, 8, 23, 10, -1, 5, 24, 16, 33, 1, 20, 14, 31, 9,
        22, 18, 29, 7, 28, 12, 35, 3, 26 // -1 represents the '00' in American roulette
    };
    public float anglePerNumberEuropean = 9.729f; // 360 degrees / 37 numbers (0-36) in European roulette
    public float anglePerNumberAmerican = 9.473f; // 360 degrees / 38 numbers (0-36 + 00) in American roulette
    public float wheelSpinStartRadius = 5; // Distance from wheel center to start the ball spin.
    public float wheelNumbersRadius = 3; // Distance from wheel center to the numbers.
    public float wheelSpinSpeed = 90f;
    [Header("Ball Movement Settings")]
    public float ballSpinSpeed = 720f; // Speed of the ball spin in degrees per second.
    public int ballPreDropTurn = 1; // How many turns the ball spins before it starts dropping the the wheel.
    public int ballInWheelTurn = 1; // How many turns the ball spins while it is in the wheel.

    private void OnDrawGizmos()
    {
        if (wheelTransform == null)
            return;

        // Draw the wheel spin start radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wheelTransform.position, wheelTransform.position + Vector3.up * wheelSpinStartRadius);
        // Draw the wheel numbers radius
        Gizmos.color = Color.green;
        Gizmos.DrawLine(wheelTransform.position, wheelTransform.position + Vector3.down * wheelNumbersRadius);
    }

    // Step 1: Place the ball to the wheelSpinStartRadius distance from the wheel center based on determinedNumber's corresponding angle based on its index on wheelNumbers array.
    // Step 2: Start spinning the ball for the specified number of ballPreDropTurn turns by rotating the ballParent transform.
    // Step 3: Spin the ballParent for the specified number of ballInWheelTurn turns while it is in the wheel, and also start moving ballTransform towards the ballParent for wheelSpinRadius-wheelNumbersRadius distance.
    public void InstantiateBall(int determinedNumber, List<int> wheelNumbers, float anglePerNumber)
    {
        float angle = 0f; // Starting angle
        int numberIndex = wheelNumbers.IndexOf(determinedNumber);
        angle = numberIndex * anglePerNumber;
        Vector3 startPosition = wheelTransform.position + Quaternion.Euler(0, 0, -angle) * Vector3.up * wheelSpinStartRadius;
        ballTransform.position = startPosition;
    }
    public void SpinTheBallParent()
    {
        // Lerp the ballParent rotation to simulate the spinning of the ball.
        float targetAngle = ballPreDropTurn * 360f; // Total angle to spin the ball
        // Calculate the duration based on the spin speed
        float duration = targetAngle / ballSpinSpeed; // Duration in seconds
        StartCoroutine(SpinBallCoroutine(targetAngle, duration));
    }
    private IEnumerator SpinBallCoroutine(float targetAngle, float duration)
    {
        float elapsedTime = 0f;
        float initialAngle = ballParent.eulerAngles.z;
        float finalAngle = initialAngle + targetAngle;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float currentAngle = Mathf.Lerp(initialAngle, finalAngle, t);
            ballParent.rotation = Quaternion.Euler(0, 0, currentAngle);
            yield return null;
        }

        ballParent.rotation = Quaternion.Euler(0, 0, finalAngle);
    }

    public void DebugInstantiateBallOnDeterminedNumber(int determinedNumber)
    {
        InstantiateBall(determinedNumber, wheelNumbersEuropean, anglePerNumberEuropean);
        SpinTheBallParent();
    }
}
