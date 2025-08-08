using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public class RouletteWheelController : MonoBehaviour
{
    public static RouletteWheelController Instance;

    public TableType tableType;

    [Header("References")]
    public Transform ballTransform;
    public Transform wheelTransform;
    public Transform ballParent; // Used for rotating the ball. Ball itself isn't rotated.
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
    public float ballFirstSpinSpeed = 720f; // Speed of the ball spin in degrees per second.
    public float ballSecondSpinSpeed = 180f; // Speed of the ball spin in degrees per second.
    public float ballTravelDistance = 1;
    public int ballPreDropTurn = 1; // How many turns the ball spins before it starts dropping the the wheel.
    public int ballInWheelTurn = 1; // How many turns the ball spins while it is in the wheel.
    public float ballLocalEndZLevel; // LOCAL Z position of the ball when It stops spinning and placed one of the numbers.

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
    public void SendTheBallToDeterminedNumber(int determinedNumber)
    {
        GameManager.Instance.gameState = GameState.spinning; // Set the game state to spinning.
        GameManager.Instance.winningNumber = determinedNumber; // Set the winning number in the GameManager.

        ballTransform.gameObject.SetActive(true);

        List<int> wheelNumbers = tableType == TableType.European ? wheelNumbersEuropean : wheelNumbersAmerican;
        float anglePerNumber = tableType == TableType.European ? anglePerNumberEuropean : anglePerNumberAmerican;

        // Calculate how many numbers to shift based on the wheel speed.
        float duration1 = (ballPreDropTurn * 360f) / ballFirstSpinSpeed;
        float duration2 = (ballInWheelTurn * 360f) / ballSecondSpinSpeed;
        float totalDuration = duration1 + duration2;
        float wheelTotalSpinAmount = totalDuration * wheelSpinSpeed;
        int numberShift = Mathf.RoundToInt(wheelTotalSpinAmount / anglePerNumber);

        // Start rotating the wheel based on spin speed.

        float angle = 0f; // Starting angle
        int numberIndex = wheelNumbers.IndexOf(determinedNumber) - numberShift;
        angle = numberIndex * anglePerNumber;
        Vector3 startPosition = wheelTransform.position + Quaternion.Euler(0, 0, -angle) * Vector3.up * wheelSpinStartRadius;
        ballTransform.position = startPosition;

        StartCoroutine(SpinWheel());
        StartCoroutine(SpinTheBallParent());
    }
    private IEnumerator SpinWheel()
    {
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            wheelTransform.Rotate(0, wheelSpinSpeed * Time.deltaTime, 0);
            yield return null;
        }

    }
    public IEnumerator SpinTheBallParent()
    {
        // Lerp the ballParent rotation to simulate the spinning of the ball.
        float targetAngle = ballPreDropTurn * 360f; // Total angle to spin the ball
        // Calculate the duration based on the spin speed
        float duration = targetAngle / ballFirstSpinSpeed; // Duration in seconds
        yield return StartCoroutine(SpinBallCoroutine(targetAngle, duration));
        MoveBallTowardsWheel();
    }
    public void MoveBallTowardsWheel()
    {
        // Lerp the ballParent rotation to simulate the spinning of the ball.
        float targetAngle = ballInWheelTurn * 360f; // Total angle to spin the ball
        // Calculate the duration based on the spin speed
        // Move the ball towards the wheel center while spinning it.
        StartCoroutine(MoveBallTowardsWheelCoroutine());
        float duration = targetAngle / ballSecondSpinSpeed; // Duration in seconds
        StartCoroutine(SpinBallCoroutine(targetAngle, duration, true));
    }

    private IEnumerator MoveBallTowardsWheelCoroutine()
    {
        // Lerp the ball position towards the wheel center while spinning it.
        float targetAngle = ballInWheelTurn * 360f; // Total angle to spin the ball while in the wheel
        float duration = targetAngle / ballSecondSpinSpeed; // Duration in seconds
        float elapsedTime = 0f;
        Vector3 startPosition = ballTransform.localPosition;
        // Move the ball towards the ballParent's position.
        Vector3 targetPosition = ballTransform.localPosition + ((ballParent.position - ballTransform.position).normalized * (wheelSpinStartRadius - wheelNumbersRadius));
        targetPosition.z = ballLocalEndZLevel;
        while (elapsedTime < duration)
        {

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            // Move the ball towards the wheel center
            ballTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }


        ballTransform.localPosition = targetPosition;
        ballTransform.parent = wheelTransform;

        // Return the game state to betting after the ball stops spinning.
        GameManager.Instance.gameState = GameState.betting;
        StopCoroutine(SpinWheel()); // Stop spinning the wheel.
        float currentBet = TableManager.Instance.currentBetAmount;
        float payoutMultiplier = TableManager.Instance.currentPayoutMultiplier;
        GameManager.Instance.CalculateGainBasedOnPayout(currentBet, payoutMultiplier);
    }
    private IEnumerator SpinBallCoroutine(float targetAngle, float duration, bool jumpTheBall = false)
    {
        float elapsedTime = 0f;
        float initialAngle = ballParent.eulerAngles.z;
        float finalAngle = initialAngle + targetAngle;

        while (elapsedTime < duration)
        {
            if(jumpTheBall)
            {
                // Add a slight jump effect to the ballParent to simulate the ball bouncing.
                float jumpHeight = 1f;
                Mathf.Lerp(jumpHeight, 0, elapsedTime / duration);
                float jumpTime = Mathf.PingPong(elapsedTime * 2f, 1f); // PingPong to create a bounce effect
                float jumpOffset = Mathf.Sin(jumpTime * Mathf.PI) * jumpHeight; // Calculate the jump offset
                ballParent.localPosition = new Vector3(ballParent.localPosition.x, jumpOffset, ballParent.localPosition.z);
            }
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
        SendTheBallToDeterminedNumber(determinedNumber);
    }

    internal void PlayBet(int number)
    {
        int minimumNumber = tableType == TableType.European ? 0 : -1; // European roulette starts from 0, American roulette starts from -1 (00).

        if (number < minimumNumber || number > 36) // If a number outside the range is provided, generate a random number within the valid range.
        {
            int randomNumber = UnityEngine.Random.Range(minimumNumber, 37);
            SendTheBallToDeterminedNumber(randomNumber);
        }
        else
        {
            SendTheBallToDeterminedNumber(number);
        }

    }
}
