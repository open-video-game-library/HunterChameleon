using UnityEngine;

public class TitleStateController : MonoBehaviour, IStateController
{
    public static TitleStateController Instance;

    private StateMachine stateMachine;

    // 各State
    private MainMenuState mainMenuState;

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
        mainMenuState = new MainMenuState(this);
    }

    private void Start()
    {
        // このSceneでは、デフォルトでカーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);

        // 初期ステート設定
        ChangeToMainMenuState();
    }

    private void Update()
    {
        stateMachine.Tick();
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void ChangeToMainMenuState()
    {
        stateMachine.ChangeState(mainMenuState);
    }

    public void ResumeFromPauseState()
    {
        // TitleSceneではPause機能はなし
    }
}
