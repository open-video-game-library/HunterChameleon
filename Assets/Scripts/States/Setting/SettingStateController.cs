using UnityEngine;

public class SettingStateController : MonoBehaviour, IStateController
{
    public static SettingStateController Instance;

    private StateMachine stateMachine;

    // 各State
    private EditState editState;
    private PreviewState previewState;

    // Pause（Edit）前の復帰先
    private IState stateBeforePause;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // StateMachineを初期化
        stateMachine = new StateMachine();

        // 各Stateクラスを生成
        editState = new EditState(this);
        previewState = new PreviewState(this);
    }

    private void Start()
    {
        // このSceneでは、デフォルトでカーソルを表示する
        MouseCursorEnabler.SetCursorActive(false);

        // 前のプレイの統計情報を初期化
        GameStatsUpdater.Instance.ResetStats();

        // 初期ステート設定
        ChangeToPreviewState();
    }

    private void Update()
    {
        stateMachine.Tick();

        // 入力デバイスで定義されているポーズ入力があると、ポーズ画面に移行する
        if (InputDataManager.Instance.inputData.pause)
        {
            if (stateMachine.currentState == editState) { ResumeFromPauseState(); }
            else { ChangeToPauseState(); }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void ChangeToPreviewState()
    {
        stateMachine.ChangeState(previewState);
    }

    public void ChangeToPauseState()
    {
        // Pause（Edit）前のStateを覚える
        stateBeforePause = stateMachine.currentState;
        stateMachine.ChangeState(editState);
    }

    public void ResumeFromPauseState()
    {
        if (stateMachine.currentState != editState) { return; }
        var target = stateBeforePause ?? previewState; // 念のためフォールバック
        stateBeforePause = null;
        stateMachine.ChangeState(target);
    }
}
