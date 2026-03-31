using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadInput : MonoBehaviour, IInput
{
    // 入力が有効/無効かどうか
    private bool isInputActive = true;

    // 仮想カーソルの位置
    private Vector2 virtualCursorPosition;

    private InputParameter parameter;

    [Tooltip("ゲームパッドの基本カーソル速度")]
    [SerializeField] private float gamepadCursorSpeed = 20f;

    private void Start()
    {
        // 仮想マウスカーソルの初期値をセット
        virtualCursorPosition = new Vector2(Screen.width * 0.50f, Screen.height * 0.50f);

        // 初期状態は、入力を有効化
        SetInputActive(true);

        parameter = ParameterManager.Instance.parameter.input;
    }

    private void Update()
    {
        // ゲームパッドが接続されていなかったら、処理を行わない
        if (Gamepad.current == null) { return; }

        // ゲームパッドが接続されていれば、Pause入力のみ常時受け付ける
        if (GetPauseSubmit()) { InputDataManager.Instance.inputData.pause = true; }

        // 入力が無効化されている場合は、処理を行わない
        if (!isInputActive) { return; }

        // --- 入力データをInputDataに反映 ---
        InputDataManager.Instance.inputData.position = GetPosition();
        if (GetSubmit()) { InputDataManager.Instance.inputData.submit = true; }
    }

    private void OnEnable()
    {
        SetInputActive(true);
    }

    private void OnDisable()
    {
        SetInputActive(false);
    }

    public void SetInputActive(bool active)
    {
        isInputActive = active;
    }

    public Vector2 GetPosition()
    {
        Vector2 currentPosition = InputDataManager.Instance.inputData.position;

        // virtualCursorPositionはスクリーン座標なので、inputDataの情報をスクリーン座標に変換して代入
        virtualCursorPosition = Camera.main.WorldToScreenPoint(currentPosition);

        virtualCursorPosition += Gamepad.current.leftStick.ReadValue() * gamepadCursorSpeed * parameter.sensitivity * Time.deltaTime;
        virtualCursorPosition = Calculate.ClampPositionInScreen(virtualCursorPosition);

        // virtualCursorPositionはスクリーン座標なので、ワールド座標に変換したものを返す 
        return Camera.main.ScreenToWorldPoint(virtualCursorPosition);
    }

    public bool GetSubmit()
    {
        return Gamepad.current.buttonSouth.wasPressedThisFrame;
    }

    public bool GetPauseSubmit()
    {
        // Start/Menuボタン、Optionsボタン、+ボタンなどでポーズ入力
        return Gamepad.current.startButton.wasPressedThisFrame;
    }
}
