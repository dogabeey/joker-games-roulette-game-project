using System.Collections;
using UnityEngine;

public class RouletteWheelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform wheel;      // The spinning wheel
    [SerializeField] private Transform ball;       // The roulette ball

    [Header("Wheel Settings")]
    [SerializeField] private int numberCount = 38; // 37 for European (0-36), 38 for American (0-36 + 00)
    [SerializeField] private float wheelRadius = 1.2f;
    [SerializeField] private float wheelSpinSpeed = 360f; // degrees per second
    [SerializeField] private float spinDuration = 5f;

    [Header("Ball Animation")]
    [SerializeField] private float ballHeightOffset = 0.1f;
    [SerializeField] private float ballFallDuration = 1.2f;

    private float AnglePerNumber => 360f / numberCount;

    private void Start()
    {
        SpinToNumber(17); // Lands on number 17
    }

    /// <summary>
    /// Call this method to spin the wheel and land the ball on a specific number index.
    /// </summary>
    /// <param name="targetNumberIndex">Index in the number layout, from 0 to numberCount - 1</param>
    public void SpinToNumber(int targetNumberIndex)
    {
        StopAllCoroutines(); // In case you spin again quickly
        StartCoroutine(SpinRoutine(targetNumberIndex));
    }

    private IEnumerator SpinRoutine(int targetNumber)
    {
        float elapsed = 0f;
        float wheelStartRotation = Random.Range(0f, 360f);

        // Animate the wheel spinning
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float angle = wheelStartRotation + wheelSpinSpeed * elapsed;
            wheel.eulerAngles = new Vector3(-180, 0, angle);
            yield return null;
        }

        // Final wheel rotation
        float finalWheelRotation = wheelStartRotation + wheelSpinSpeed * spinDuration;
        float wheelAngleMod = finalWheelRotation % 360f;

        // Determine the future angle of the target number
        float targetLocalAngle = GetAngleForNumber(targetNumber); // e.g. 270 = top
        float worldAngle = (targetLocalAngle + wheelAngleMod) % 360f;

        // Convert to world position
        Vector3 ballTargetPos = GetBallWorldPosition(worldAngle);

        // Animate the ball falling
        yield return AnimateBallFall(ballTargetPos);
    }

    private float GetAngleForNumber(int index)
    {
        // Assuming clockwise number layout; adjust if yours differs
        return 360f - (index * AnglePerNumber);
    }

    private Vector3 GetBallWorldPosition(float angle)
    {
        Vector3 center = wheel.position;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * wheelRadius;
        return center + offset + Vector3.up * ballHeightOffset;
    }

    private IEnumerator AnimateBallFall(Vector3 targetPos)
    {
        Vector3 start = targetPos + Vector3.up * 0.5f;
        float elapsed = 0f;

        while (elapsed < ballFallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / ballFallDuration;
            float bounce = Mathf.Sin(t * Mathf.PI) * 0.15f;
            ball.position = Vector3.Lerp(start, targetPos, t) + Vector3.up * bounce;
            yield return null;
        }

        ball.position = targetPos;
    }
}
