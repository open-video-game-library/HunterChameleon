public class DrawResultState : IState
{
    private readonly ResultStateController ctx;

    public DrawResultState(ResultStateController ctx) { this.ctx = ctx; }

    public void Enter()
    {
        AudioManager.Instance.PlayBGM(BGMKey.Result);
    }

    public void Tick()
    {

    }

    public void Exit()
    {
        AudioManager.Instance.StopBGM();
    }
}
