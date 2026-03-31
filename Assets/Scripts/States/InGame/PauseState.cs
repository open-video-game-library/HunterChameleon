using UnityEngine;

public class PauseState : IState
{
    private readonly InGameStateController ctx;

    public PauseState(InGameStateController ctx) { this.ctx = ctx; }

    private PauseController pauseController;

    public void Enter()
    {
        if (!pauseController) { pauseController = Object.FindObjectOfType<PauseController>(true); }

        PauseUtility.Pause();

        // Pause画面を表示させる
        pauseController.gameObject.SetActive(true);

        // このState中は、カーソルを表示にする
        MouseCursorEnabler.SetCursorActive(true);
    }

    public void Tick()
    {

    }

    public void Exit()
    {
        PauseUtility.Unpause();

        // Pause画面を非表示にする
        pauseController.gameObject.SetActive(false);

        // このStateから抜けるときは、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);
    }
}
