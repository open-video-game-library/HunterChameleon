using UnityEngine;

public static class PauseUtility
{
    private static bool isPause = false;

    public static void Pause()
    {
        if (isPause) { return; }

        // 時間・オーディオ・ログ記録
        Time.timeScale = 0f;
        AudioListener.pause = true;
        ParamLogger.Instance.PauseTrial();

        // ゲーム操作を無効化する処理
        IInput[] inputs = InterfaceFinder.FindObjectsOfInterface<IInput>();
        foreach (var input in inputs)
        {
            // 取得した入力を無効化する
            input.SetInputActive(false);
        }

        isPause = true;
        Debug.Log("Paused");
    }

    public static void Unpause()
    {
        if (!isPause) { return; }

        // 時間・オーディオ・ログ記録
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        ParamLogger.Instance.ResumeTrial();

        // ゲーム操作を有効化する処理
        IInput[] inputs = InterfaceFinder.FindObjectsOfInterface<IInput>();
        foreach (var input in inputs)
        {
            // 取得した入力を有効化する
            input.SetInputActive(true);
        }

        isPause = false;
        Debug.Log("Unpaused");
    }
}
