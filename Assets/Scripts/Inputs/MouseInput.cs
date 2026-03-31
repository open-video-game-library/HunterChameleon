using UnityEngine;

public class MouseInput : MonoBehaviour, IInput
{
    // 入力が有効/無効かどうか
    private bool isInputActive = true;

    // 仮想カーソルのスクリーン座標（カーソルが非表示のときに用いる）
    private Vector2 virtualCursorPosition;

    // --- 直前フレームのカーソル状態 ---
    private bool wasCursorActive = true; // マウスカーソルが表示されていたか？
    private Vector2 preMousePosition; // マウスカーソルの位置

    private InputParameter parameter;

    void Start()
    {
        // 直前フレームのカーソル状態をセット
        wasCursorActive = MouseCursorEnabler.GetCursorActive();
        preMousePosition = Input.mousePosition;

        // 仮想マウスカーソルの初期値をセット
        virtualCursorPosition = new Vector2(Screen.width * 0.50f, Screen.height * 0.50f);

        // 初期状態は、入力を有効化
        SetInputActive(true);

        parameter = ParameterManager.Instance.parameter.input;
    }

    void Update()
    {
        // マウスが接続されていなかったら、処理を行わない
        if (!Input.mousePresent) { return; }

        // マウスが接続されていれば、Pause入力のみ常時受け付ける
        if (GetPauseSubmit()) { InputDataManager.Instance.inputData.pause = true; }

        // 入力が無効化されている場合は、処理を行わない
        if (!isInputActive) { return; }

        if (DetectCursorActiveChanged())
        {
            // マウスカーソルの有効化を検知し、仮想マウスカーソルの初期値をセット
            virtualCursorPosition = new Vector2(Screen.width * 0.50f, Screen.height * 0.50f);
        }

        // --- 入力データをInputDataに反映 ---
        InputDataManager.Instance.inputData.position = GetPosition();
        if (GetSubmit()) { InputDataManager.Instance.inputData.submit = true; }

        // 次のフレームで使うために、現フレームのカーソル状態をセット
        wasCursorActive = MouseCursorEnabler.GetCursorActive();
        preMousePosition = Input.mousePosition;
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
        if (MouseCursorEnabler.GetCursorActive())
        {
            // マウスカーソルが動いていない場合は、変化なし
            if (preMousePosition == (Vector2)Input.mousePosition) { return InputDataManager.Instance.inputData.position; }

            // 実際のマウスカーソルの位置をワールド座標に変換したものをセット
            return Calculate.ClampScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            float dx = Input.GetAxisRaw("Mouse X");
            float dy = Input.GetAxisRaw("Mouse Y");
            Vector2 delta = new Vector2(dx, dy);

            // virtualCursorPositionはスクリーン座標なので、inputDataの情報をスクリーン座標に変換して代入
            virtualCursorPosition = Camera.main.WorldToScreenPoint(InputDataManager.Instance.inputData.position);

            virtualCursorPosition += delta * parameter.sensitivity;
            virtualCursorPosition = Calculate.ClampPositionInScreen(virtualCursorPosition);

            // virtualCursorPositionはスクリーン座標なので、ワールド座標に変換したものを返す 
            return Camera.main.ScreenToWorldPoint(virtualCursorPosition);
        }
    }

    public bool GetSubmit()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetPauseSubmit()
    {
        // マウス入力だが、キーボードのEsc入力のほうが一般的
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private bool DetectCursorActiveChanged()
    {
        // 前フレームと状態が切り替わったかを検出
        return !MouseCursorEnabler.GetCursorActive() && wasCursorActive; ;
    }
}
