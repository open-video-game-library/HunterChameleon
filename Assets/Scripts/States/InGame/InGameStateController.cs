using UnityEngine;

public class InGameStateController : MonoBehaviour, IStateController
{
    public static InGameStateController Instance;

    private StateMachine stateMachine;

    // 各State
    private ReadyState readyState;
    private CountDownState countDownState;
    private PlayingState playingState;
    private FinishingState finishingState;
    private PauseState pauseState;

    // Pause前の復帰先
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
        readyState = new ReadyState(this);
        countDownState = new CountDownState(this);
        playingState = new PlayingState(this);
        finishingState = new FinishingState(this);
        pauseState = new PauseState(this);
    }

    private void Start()
    {
        // このSceneでは、デフォルトでカーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);

        // 前のプレイの統計情報を初期化
        GameStatsUpdater.Instance.ResetStats();

        // 初期ステート設定
        ChangeToReadyState();
    }

    private void Update()
    {
        stateMachine.Tick();

        // 入力デバイスで定義されているポーズ入力があると、ポーズ画面に移行する
        if (InputDataManager.Instance.inputData.pause)
        {
            if (stateMachine.currentState == pauseState) { ResumeFromPauseState(); }
            else { ChangeToPauseState(); }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void ChangeToReadyState()
    {
        stateMachine.ChangeState(readyState);
    }

    public void ChangeToCountDownState()
    {
        stateMachine.ChangeState(countDownState);
    }

    public void ChangeToPlayingState()
    {
        stateMachine.ChangeState(playingState);
    }

    public void ChangeToFinishState()
    {
        stateMachine.ChangeState(finishingState);
    }

    public void ChangeToPauseState()
    {
        // Pause前のStateを覚える
        stateBeforePause = stateMachine.currentState;
        stateMachine.ChangeState(pauseState);
    }

    public void ResumeFromPauseState()
    {
        if (stateMachine.currentState != pauseState) { return; }
        var target = stateBeforePause ?? playingState; // 念のためフォールバック
        stateBeforePause = null;
        stateMachine.ChangeState(target);
    }
}
