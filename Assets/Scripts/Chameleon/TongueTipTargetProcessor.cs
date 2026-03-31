using UnityEngine;

public class TongueTipTargetProcessor : MonoBehaviour, ITongueHitProcessor
{
    public bool TryProcess(Vector2 p, float r)
    {
        // 半径内の全コライダーを取得
        var hits = Physics2D.OverlapCircleAll(p, r);
        if (hits == null || hits.Length == 0) { return false; }

        bool processed = false;

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out ITarget target))
            {
                processed = true;
                target.OnHitByTongue();

                // ターゲットのスコア情報を取得
                float baseScore = target.BaseScore;

                // コンボ情報を取得可能であれば取得する
                bool isCombo = ComboCounter.Instance ? ComboCounter.Instance.isCombo : false;
                float comboBonus = ComboCounter.Instance ? ComboCounter.Instance.limitedComboCount : 0f;

                // コンボ中かどうかに応じて、最終的なスコアを加算
                if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnScoreAdded(isCombo ? baseScore * comboBonus : baseScore); }

                // スコアポップアップを表示させる
                if (ScorePopupSpawner.Instance) { ScorePopupSpawner.Instance.SpawnScorePopup(transform.position, baseScore, isCombo, comboBonus); }
            }
            else if (h.TryGetComponent(out StartAppleController startApple))
            {
                processed = true;
                startApple.OnHitByTongue();
            }
        }

        // 1件でも処理したら true（コンボは Controller 側で切らない）
        return processed;
    }
}
