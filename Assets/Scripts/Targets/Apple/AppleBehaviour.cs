using UnityEngine;

public class AppleBehaviour : MonoBehaviour, ITargetBehaviour
{
    private float velocityY;

    public void Init(Vector3 initialPosition)
    {
        velocityY = 0f;
    }

    public void Move(float speedX, float speedY)
    {
        velocityY += speedY * Time.deltaTime;
        transform.Translate(0f, velocityY * Time.deltaTime, 0f, Space.World);
    }
}
