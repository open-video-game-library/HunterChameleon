using UnityEngine;

public class PlayingState : IState
{
    private readonly InGameStateController ctx;

    public PlayingState(InGameStateController ctx) { this.ctx = ctx; }

    // TargetSpawner系
    private AppleSpawner appleSpawner;
    private GoldAppleSpawner goldAppleSpawner;
    private PoisonAppleSpawner poisonAppleSpawner;
    private FlySpawner flySpawner;
    private GoldFlySpawner goldFlySpawner;
    private PoisonFlySpawner poisonFlySpawner;

    // Sun
    private SunController sunController;

    public void Enter()
    {
        if (!appleSpawner) { appleSpawner = Object.FindObjectOfType<AppleSpawner>(true); }
        if (!goldAppleSpawner) { goldAppleSpawner = Object.FindObjectOfType<GoldAppleSpawner>(true); }
        if (!poisonAppleSpawner) { poisonAppleSpawner = Object.FindObjectOfType<PoisonAppleSpawner>(true); }
        if (!flySpawner) { flySpawner = Object.FindObjectOfType<FlySpawner>(true); }
        if (!goldFlySpawner) { goldFlySpawner = Object.FindObjectOfType<GoldFlySpawner>(true); }
        if (!poisonFlySpawner) { poisonFlySpawner = Object.FindObjectOfType<PoisonFlySpawner>(true); }

        if (!sunController) { sunController = Object.FindObjectOfType<SunController>(true); }
        sunController.gameObject.SetActive(true);

        AudioManager.Instance.PlayBGM(BGMKey.InGame);

        // ゲームの統計情報の記録を開始
        GameStatsUpdater.Instance.SetRecording(true);

        // このState中は、カーソルを非表示にする
        MouseCursorEnabler.SetCursorActive(false);

        // ここから試行を開始し、ログの記録を開始
        ParamLogger.Instance.StartTrial();
        ParamLogger.Instance.StartStream();
    }

    public void Tick()
    {
        TimeKeeper.Instance.TickTimer();

        appleSpawner.TickSpawn();
        goldAppleSpawner.TickSpawn();
        poisonAppleSpawner.TickSpawn();
        flySpawner.TickSpawn();
        goldFlySpawner.TickSpawn();
        poisonFlySpawner.TickSpawn();
        
        sunController.ChangeColor(TimeKeeper.Instance.GetCurrentNormalizedTime());

        // 制限時間を近づいていることを知らせるアラートを鳴らす時間になったとき
        if (TimeKeeper.Instance.ConsumeAlart())
        {
            // アラートのジングルを鳴らし、BGMのピッチを上げる
            AudioManager.Instance.PlayBGMJingle(JingleKey.Alart);
            AudioManager.Instance.SetBGMPitch(1.2f);
        }

        // 制限時間になったとき
        if (TimeKeeper.Instance.ConsumeTimeUp())
        {
            // 全てのターゲットを非表示にする処理
            ITarget[] targets = InterfaceFinder.FindObjectsOfInterface<ITarget>();
            foreach (var target in targets)
            {
                // 取得したターゲットを非表示にする
                target.DisableSelf();
            }

            // 全てのスコアポップアップを非表示にする処理
            ScorePopupController[] scorePopups = Object.FindObjectsOfType<ScorePopupController>();
            foreach (var scorePopup in scorePopups)
            {
                // 取得したスコアポップアップを非表示にする
                scorePopup.DisableSelf(); 
            }

            // BGMを止める
            AudioManager.Instance.StopBGM();

            // ===== ログのスナップショットを記録 & ログのストリーム記録を終了 =====
            ParamLogger.Instance.CaptureSnapshot();
            ParamLogger.Instance.StopStream();
            ParamLogger.Instance.EndTrial();

            ctx.ChangeToFinishState();
        }
    }

    public void Exit()
    {
        // ゲームの統計情報の記録を終了
        GameStatsUpdater.Instance.SetRecording(false);

        // このStateから抜けるときは、カーソルを表示する
        MouseCursorEnabler.SetCursorActive(true);
    }
}
