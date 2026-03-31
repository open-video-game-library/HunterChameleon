using UnityEngine;

public interface ITargetBehaviour
{
    public void Init(Vector3 initialPosition);
    public void Move(float speedX, float speedY);
}
