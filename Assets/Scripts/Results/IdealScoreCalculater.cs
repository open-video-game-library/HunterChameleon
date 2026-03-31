using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// r(舌サイクル)・τ(コンボ猶予)・K(倍率上限)を考慮し、共通滞在時間(dwellSeconds)を用いた理論最大スコアを概算
/// - 加点ターゲットのみ使用
/// - 期限 = spawnTime + dwellSeconds
/// - 連射間隔 = chameleon.extendTime + chameleon.retractTime（距離に依らず一定）
/// - 滞在時間を無視したい場合は dwellSeconds に float.PositiveInfinity を渡す
/// </summary>
public static class IdealScoreCalculater
{
    public static float CalculateIdealScore(List<SpawnedTargetsInfo> spawnedTargets, float playTime, float dwellSeconds)
    {
        if (spawnedTargets == null || spawnedTargets.Count == 0) return 0f;
        if (dwellSeconds <= 0f) return 0f;

        // ゲーム内パラメータ取得
        int K = Mathf.Max(0, ParameterManager.Instance.parameter.combo.comboCountLimit);
        float tau = Mathf.Max(0f, ParameterManager.Instance.parameter.combo.comboTimeLimit);
        float T = playTime;
        if (K <= 0 || T <= 0f) return 0f;

        // 舌サイクル（距離に依らず一定）
        float extend = Mathf.Max(0f, ParameterManager.Instance.parameter.chameleon.extendTime);
        float retract = Mathf.Max(0f, ParameterManager.Instance.parameter.chameleon.retractTime);
        float shotInterval = Mathf.Max(1e-6f, extend + retract);

        // 入力整形：加点のみ／理論期限(latest)付与
        var items = new List<Item>(spawnedTargets.Count);
        for (int i = 0; i < spawnedTargets.Count; i++)
        {
            var s = spawnedTargets[i];
            if (!s.isReward) continue;
            if (s.baseScore <= 0f) continue;

            float spawn = s.spawnedTime;
            if (spawn > T) continue;

            float latest = spawn + dwellSeconds;
            if (latest <= spawn) continue;

            int value = Mathf.RoundToInt(s.baseScore);
            items.Add(new Item(spawn, latest, value));
        }
        if (items.Count == 0) return 0f;

        items.Sort((a, b) => a.spawn.CompareTo(b.spawn));

        var heap = new ExpiringMaxHeap(items.Count); // value最大優先＋期限
        int idx = 0;

        float time = Mathf.Max(0f, items[0].spawn);
        float nextShotAt = time;

        bool hasPrevHit = false;
        float lastHitTime = -1f;
        int streak = 0;

        long ideal = 0;

        while (time <= T)
        {
            // 期限切れ掃除
            while (heap.Count > 0 && heap.PeekLatest() < time) heap.PopMax();

            // 新規スポーンを追加
            while (idx < items.Count && items[idx].spawn <= time)
            {
                if (items[idx].latest > time) heap.Push(items[idx].value, items[idx].latest);
                idx++;
            }

            // ★ 未来イベントが何もないなら終了（無限ループ防止）
            if (heap.Count == 0 && idx >= items.Count && time >= T)
                break;

            // まだ撃てない → 次イベントへ
            if (time < nextShotAt)
            {
                float nextSpawn = (idx < items.Count) ? items[idx].spawn : float.PositiveInfinity;
                float prev = time;
                time = Mathf.Min(Mathf.Min(nextShotAt, nextSpawn), T);

                // ★ 時刻が進まなかったら終了（保険）
                if (Mathf.Approximately(time, prev)) break;

                if (hasPrevHit && time > lastHitTime + tau) { hasPrevHit = false; streak = 0; }
                continue;
            }

            // 候補なし → 次スポーンへ
            if (heap.Count == 0)
            {
                float nextSpawn = (idx < items.Count) ? items[idx].spawn : float.PositiveInfinity;
                float prev = time;
                time = Mathf.Min(nextSpawn, T);

                // ★ 時刻が進まなかったら終了（保険）
                if (Mathf.Approximately(time, prev)) break;

                if (hasPrevHit && time > lastHitTime + tau) { hasPrevHit = false; streak = 0; }
                continue;
            }

            // 締切超過でコンボ切れ
            if (hasPrevHit && time > lastHitTime + tau) { hasPrevHit = false; streak = 0; }

            // 期限内の最大価値を撃つ
            while (heap.Count > 0 && heap.PeekLatest() < time) heap.PopMax();
            if (heap.Count == 0) continue;

            var node = heap.PopMax();
            int mult = Mathf.Min(streak + 1, K);
            ideal += (long)node.value * mult;

            // 命中後更新
            hasPrevHit = true;
            lastHitTime = time;
            streak = Mathf.Min(streak + 1, K);

            // 次に撃てる最短時刻（固定サイクル）
            nextShotAt = time + shotInterval;

            // 次イベントへ
            float nextSpawn2 = (idx < items.Count) ? items[idx].spawn : float.PositiveInfinity;
            float prev2 = time;
            time = Mathf.Min(Mathf.Min(nextShotAt, nextSpawn2), T);

            // ★ 時刻が進まなかったら終了（保険）
            if (Mathf.Approximately(time, prev2)) break;
        }

        return (float)ideal;
    }

    // ---- 内部：軽量構造と期限付きMaxHeap ----
    private struct Item
    {
        public readonly float spawn, latest;
        public readonly int value;
        public Item(float spawn, float latest, int value)
        { this.spawn = spawn; this.latest = latest; this.value = value; }
    }

    private sealed class ExpiringMaxHeap
    {
        private struct Node { public int value; public float latest; public Node(int v, float l) { value = v; latest = l; } }
        private readonly List<Node> a;
        public int Count => a.Count;
        public ExpiringMaxHeap(int cap = 0) { a = (cap > 0) ? new List<Node>(cap) : new List<Node>(); }

        public void Push(int value, float latest)
        {
            var n = new Node(value, latest);
            a.Add(n);
            int i = a.Count - 1;
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (a[p].value >= a[i].value) break;
                (a[p], a[i]) = (a[i], a[p]); i = p;
            }
        }
        public (int value, float latest) PopMax()
        {
            int last = a.Count - 1;
            var top = a[0];
            a[0] = a[last];
            a.RemoveAt(last);
            int i = 0;
            while (true)
            {
                int l = i * 2 + 1, r = l + 1, m = i;
                if (l < a.Count && a[l].value > a[m].value) m = l;
                if (r < a.Count && a[r].value > a[m].value) m = r;
                if (m == i) break;
                (a[i], a[m]) = (a[m], a[i]); i = m;
            }
            return (top.value, top.latest);
        }
        public float PeekLatest() => a[0].latest;
    }
}
