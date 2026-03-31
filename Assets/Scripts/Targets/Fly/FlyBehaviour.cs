using UnityEngine;

public enum FlyPattern
{
    Straight,
    Wave,
    Jagged
}

public class FlyBehaviour : MonoBehaviour, ITargetBehaviour
{
    private FlyPattern pattern;

    private float initialPosY;
    private float previousPosY;
    private float phase;

    public void Init(Vector3 initialPosition)
    {
        pattern = Calculate.GetRandomEnumValue<FlyPattern>();
        initialPosY = initialPosition.y;
        previousPosY = initialPosY;
        phase = 0f;
    }

    public void Move(float speedX, float speedY)
    {
        float posOffsetX = speedX * Time.deltaTime;
        float posOffsetY = GetOffsetY(pattern, speedY);

        transform.Translate(posOffsetX, posOffsetY, 0f);
    }

    private float GetOffsetY(FlyPattern flyPattern, float speed)
    {
        phase += speed * Time.deltaTime;

        float currentPosY = initialPosY;

        // 1周期にかかる時間
        float duration;

        switch (flyPattern)
        {
            case FlyPattern.Straight:
                // 何もしない
                break;
            case FlyPattern.Wave:
                duration = 1.50f;
                currentPosY += Mathf.Sin(phase * duration);
                break;
            case FlyPattern.Jagged:
                duration = 1.20f;
                currentPosY += Mathf.PingPong(phase * duration, 2.0f) - 1.0f;
                break;
        }

        float diff = currentPosY - previousPosY;
        previousPosY = currentPosY;
        return diff;
    }
}
