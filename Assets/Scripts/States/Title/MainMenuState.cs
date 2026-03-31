public class MainMenuState : IState
{
    private readonly TitleStateController ctx;

    public MainMenuState(TitleStateController ctx) { this.ctx = ctx; }

    public void Enter()
    {
        AudioManager.Instance.PlayBGM(BGMKey.Title);
    }

    public void Tick()
    {

    }

    public void Exit()
    {
        AudioManager.Instance.StopBGM();
    }
}
