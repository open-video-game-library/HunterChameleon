using UnityEngine;

public class ReadyState : IState
{
    private readonly InGameStateController ctx;

    public ReadyState(InGameStateController ctx) { this.ctx = ctx; }

    private StartAppleController startAppleController;

    public void Enter()
    {
        if (!startAppleController) { startAppleController = Object.FindObjectOfType<StartAppleController>(true); }

        // StartAppleを表示させる
        startAppleController.gameObject.SetActive(true);

        // このState中は、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);
    }

    public void Tick()
    {
        if (startAppleController.isHitByTongue) { ctx.ChangeToCountDownState(); }
    }

    public void Exit()
    {
        // StartAppleを非表示にする
        startAppleController.gameObject.SetActive(false);

        // このStateから抜けるときは、カーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);
    }
}
