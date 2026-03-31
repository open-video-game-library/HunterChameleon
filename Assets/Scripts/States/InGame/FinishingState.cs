using UnityEngine;

public class FinishingState : IState
{
    private readonly InGameStateController ctx;

    public FinishingState(InGameStateController ctx) { this.ctx = ctx; }

    private FinishController finishController;

    public void Enter()
    {
        if (!finishController) { finishController = Object.FindObjectOfType<FinishController>(true); }

        // FinishTextを表示させる
        finishController.gameObject.SetActive(true);

        // Finish時のSEを再生する
        finishController.PlayFinishSE();

        // このState中は、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);
    }

    public void Tick()
    {
        finishController.TickFinish();

        if (finishController.isFinishDone)
        {
            // ResultSceneをロード
            SceneLoadUtility.LoadScene("ResultScene");
        }
    }

    public void Exit()
    {
        // FinishTextを非表示にする
        finishController.gameObject.SetActive(false);

        // このStateから抜けるときは、カーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);
    }
}
