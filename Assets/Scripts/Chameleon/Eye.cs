using UnityEngine;

public class Eye : MonoBehaviour
{
    private InputData input;

    private void Start()
    {
        // 参照する入力データをセット
        input = InputDataManager.Instance.inputData;
    }

    private void LateUpdate()
    {
        LookAt(input.position);
    }

    private void LookAt(Vector2 targetPosition)
    {
        Vector2 eyePosition = transform.position;        
        float angle = Calculate.GetAngle(Vector2.up, eyePosition, targetPosition);

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
