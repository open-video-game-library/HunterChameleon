using UnityEngine;

public class Chameleon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ITarget target))
        {
            if (!target.CanCollide) { return; }

            target.OnHitByChameleon();

            // 減算スコアを取得し、スコアを反映
            float baseScore = target.CollideScore;
            if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnScoreAdded(baseScore); }
        }
    }
}
