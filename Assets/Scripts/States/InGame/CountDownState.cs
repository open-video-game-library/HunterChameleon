using UnityEngine;

public class CountDownState : IState
{
    private readonly InGameStateController ctx;

    public CountDownState(InGameStateController ctx) { this.ctx = ctx; }

    private CountDownController countDownController;

    public void Enter()
    {
        if (!countDownController) { countDownController = Object.FindObjectOfType<CountDownController>(true); }

        // CountDownTextを表示させる
        countDownController.gameObject.SetActive(true);

        // このState中は、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);
    }

    public void Tick()
    {
        countDownController.TickCountDown();

        if (countDownController.isCountDownDone) { ctx.ChangeToPlayingState(); } 
    }

    public void Exit()
    {
        // CountDownTextを非表示にする
        countDownController.gameObject.SetActive(false);

        // このStateから抜けるときは、カーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);
    }
}
