using UnityEngine;

public class ResultStateController : MonoBehaviour
{
    public static ResultStateController Instance;

    private StateMachine stateMachine;

    // 各State名
    private DrawResultState drawResultState;

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
        drawResultState = new DrawResultState(this);
    }

    private void Start()
    {
        // このSceneでは、デフォルトでカーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);

        // 初期ステート設定
        ChangeDrawResultState();
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void ChangeDrawResultState()
    {
        stateMachine.ChangeState(drawResultState);
    }

    public void ResumeFromPauseState()
    {
        // TitleSceneではPauseなし
    }
}
