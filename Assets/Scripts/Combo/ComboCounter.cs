using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public static ComboCounter Instance;

    [HideInInspector]
    public int comboCount; // 現在のコンボ数
    [HideInInspector]
    public int maxComboCount; // 現在までの最大コンボ数
    [HideInInspector]
    public int limitedComboCount; // コンボ上限が反映された現在のコンボ数
    [HideInInspector]
    public bool isCombo; // 現在コンボが継続中かどうか

    private ComboParameter parameter;

    private float comboTimeCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        parameter = ParameterManager.Instance.parameter.combo;

        ResetComboCount();
        maxComboCount = 0;
    }

    private void Update()
    {
        // 次へのコンボへの受付猶予時間以内にコンボがないと、自然にコンボが途切れる
        EndComboByTimeout();
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void AddComboCount()
    {
        comboCount++;
        isCombo = true;

        // 次へのコンボへの受付猶予時間を更新
        comboTimeCount = parameter.comboTimeLimit;

        // 現在のコンボ数が記録された最大コンボ数を上回っていた場合、最大コンボ数を更新
        if (comboCount > maxComboCount)
        {
            maxComboCount = comboCount;
            if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnMaxComboCountUpdated(maxComboCount); }
        }

        // コンボ数の上限以下になるように丸める
        limitedComboCount = Mathf.Min(comboCount, parameter.comboCountLimit);

        // 最大コンボ時の音を再生
        if (comboCount >= parameter.comboCountLimit) { AudioManager.Instance.PlaySE(SEKey.ComboRewardMax); }
        // 通常コンボ時の音を再生（コンボ段階に応じてピッチが変化）
        else { AudioManager.Instance.PlaySE(SEKey.ComboReward, limitedComboCount - 1); }
    }

    public void EndCombo()
    {
        // コンボしていないときは呼ばない
        if (!isCombo) { return; }

        ResetComboCount();

        // 音などの演出
        AudioManager.Instance.PlaySE(SEKey.ComboEnd);
    }

    private void EndComboByTimeout()
    {
        // 次へのコンボへの受付猶予時間が既になくなっているときは呼ばない
        if (comboTimeCount <= 0f) { return; }

        // 猶予時間をカウントダウンし、猶予時間がなくなったらコンボを途切れさせる
        comboTimeCount -= Time.deltaTime;
        if (comboTimeCount <= 0f)
        {
            ResetComboCount();

            // 音などの演出
            AudioManager.Instance.PlaySE(SEKey.ComboEnd);
        }
    }

    public void ResetComboCount()
    {
        comboCount = 0;
        limitedComboCount = 0;
        comboTimeCount = 0f;
        isCombo = false;
    }
}
