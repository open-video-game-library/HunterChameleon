using UnityEngine;

public class InputDataManager : MonoBehaviour
{
    public static InputDataManager Instance;

    [HideInInspector]
    public InputData inputData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        // このフレームでtrueになった各入力を、次のフレームのためにリセットする
        inputData.submit = false;
        inputData.pause = false;
    }
}
