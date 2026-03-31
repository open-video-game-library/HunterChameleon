using UnityEngine.SceneManagement;

public static class SceneLoadUtility
{
    public static void LoadScene(string nextSceneName)
    {
        // 遷移後のSceneで操作不能にならないための保険
        PauseUtility.Unpause();

        // 今鳴っているBGMを止める
        AudioManager.Instance.StopBGM();
        // BGMのピッチを元に戻す
        AudioManager.Instance.ResetBGMPitch();

        // 試行中のデータを破棄する
        ParamLogger.Instance.AbortTrial();

        SceneManager.LoadScene(nextSceneName);
    }
}
