using UnityEngine;

public class PreviewState : IState
{
    private readonly SettingStateController ctx;

    public PreviewState(SettingStateController ctx) { this.ctx = ctx; }

    // TargetSpawner系
    private AppleSpawner appleSpawner;
    private GoldAppleSpawner goldAppleSpawner;
    private PoisonAppleSpawner poisonAppleSpawner;
    private FlySpawner flySpawner;
    private GoldFlySpawner goldFlySpawner;
    private PoisonFlySpawner poisonFlySpawner;

    public void Enter()
    {
        if (!appleSpawner) { appleSpawner = Object.FindObjectOfType<AppleSpawner>(true); }
        if (!goldAppleSpawner) { goldAppleSpawner = Object.FindObjectOfType<GoldAppleSpawner>(true); }
        if (!poisonAppleSpawner) { poisonAppleSpawner = Object.FindObjectOfType<PoisonAppleSpawner>(true); }
        if (!flySpawner) { flySpawner = Object.FindObjectOfType<FlySpawner>(true); }
        if (!goldFlySpawner) { goldFlySpawner = Object.FindObjectOfType<GoldFlySpawner>(true); }
        if (!poisonFlySpawner) { poisonFlySpawner = Object.FindObjectOfType<PoisonFlySpawner>(true); }

        AudioManager.Instance.PlayBGM(BGMKey.Setting);

        // ゲームの統計情報の記録を開始
        GameStatsUpdater.Instance.SetRecording(true);

        // このState中は、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);
    }

    public void Tick()
    {
        appleSpawner.TickSpawn();
        goldAppleSpawner.TickSpawn();
        poisonAppleSpawner.TickSpawn();
        flySpawner.TickSpawn();
        goldFlySpawner.TickSpawn();
        poisonFlySpawner.TickSpawn();
    }

    public void Exit()
    {
        // ゲームの統計情報の記録を終了
        GameStatsUpdater.Instance.SetRecording(false);

        // このStateから抜けるときは、カーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);
    }
}
