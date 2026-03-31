using System.Collections.Generic;
using UnityEngine;

public class GameStatsUpdater : MonoBehaviour
{
    public static GameStatsUpdater Instance;

    // ===== ログ出力用 =====
    [SnapshotParam("play_time")] public float PlayTime { get; private set; }
    [SnapshotParam("score")] public float Score { get; private set; }
    [SnapshotParam("shot_count")] public int ShotCount { get; private set; }
    [SnapshotParam("hit_count")] public int HitCount { get; private set; }
    [SnapshotParam("miss_hit_count")] public int MissHitCount { get; private set; }
    [SnapshotParam("max_combo_count")] public int MaxComboCount { get; private set; }

    // ゲーム中にスポーンされた全ターゲットの情報（リザルのランク算出に使用）
    public List<SpawnedTargetsInfo> TargetsInfo { get; private set; }

    // ゲームの統計情報を記録しているか
    private bool isRecordingStats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 初期化
        ResetStats();
    }

    private void Update()
    {
        CountPlayTime();
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    // === 統計情報の記録状況を管理する関数 ===

    public void ResetStats()
    {
        PlayTime = 0.0f;
        Score = 0.0f;
        ShotCount = 0;
        HitCount = 0;
        MissHitCount = 0;
        MaxComboCount = 0;
        TargetsInfo = new List<SpawnedTargetsInfo>();
    }

    public void SetRecording(bool isRecording)
    {
        isRecordingStats = isRecording;
    }

    // === 統計情報を更新する関数 ===

    public void OnScoreAdded(float addedScore)
    {
        if (!isRecordingStats) { return; }
        Score += addedScore;
    }

    public void OnTongueShot()
    {
        if (!isRecordingStats) { return; }
        ShotCount++;
    }

    public void OnTongueHitTarget()
    {
        if (!isRecordingStats) { return; }
        HitCount++;
    }

    public void OnTongueMissHit()
    {
        if (!isRecordingStats) { return; }
        MissHitCount++;
    }

    public void OnMaxComboCountUpdated(int updatedMaxComboCount)
    {
        if (!isRecordingStats) { return; }
        MaxComboCount = updatedMaxComboCount;
    }

    public void OnTargetSpawned(string name, bool isReward, bool canCollide, float baseScore, float collideScore, Vector2 spawnPosition, float spawnedTime)
    {
        if (!isRecordingStats) { return; }

        SpawnedTargetsInfo targetInfo = new SpawnedTargetsInfo(name, isReward, canCollide, baseScore, collideScore, spawnPosition, spawnedTime);
        TargetsInfo.Add(targetInfo);
    }

    // === リザルト描画用のヘルパ関数 ===

    public float GetScoreAchievementRatio(float idealScore)
    {
        // 理論値スコアから見た獲得スコアの割合を算出
        return idealScore != 0.0f ? Score / idealScore : 0.0f;
    }

    public float GetAccuracy()
    {
        // ヒット数と舌を伸ばした回数からヒット率を算出
        return ShotCount > 0 ? (float)HitCount / ShotCount : 0f;
    }

    private void CountPlayTime()
    {
        if (!isRecordingStats) { return; }
        PlayTime += Time.deltaTime;
    }
}
