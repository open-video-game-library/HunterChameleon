using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreRankParameters
{
    public List<ScoreRankParameter> scoreRanks = new List<ScoreRankParameter>()
    {
        new ScoreRankParameter { rankLabel = "S", scoreThreshold = 0.65f, displayColor = new Color32(255, 180, 90, 255) },
        new ScoreRankParameter { rankLabel = "A", scoreThreshold = 0.55f, displayColor = new Color32(255, 25, 0, 255) },
        new ScoreRankParameter { rankLabel = "B", scoreThreshold = 0.35f, displayColor = new Color32(0, 110, 255, 255) },
        new ScoreRankParameter { rankLabel = "C", scoreThreshold = 0.20f, displayColor = new Color32(0, 190, 50, 255) },
        new ScoreRankParameter { rankLabel = "D", scoreThreshold = float.NegativeInfinity, displayColor = new Color32(0, 190, 180, 255) },
    };

    public ScoreRankParameter GetRank(float score)
    {
        SortByThreshold();

        foreach (var scoreRank in scoreRanks)
        {
            if (score >= scoreRank.scoreThreshold)
            {
                return scoreRank;
            }
        }

        // どのランクにも当てはまらなかった場合、最後の評価を返す
        return scoreRanks.Count > 0 ? scoreRanks[scoreRanks.Count - 1] : null;
    }

    private void SortByThreshold()
    {
        // 登録されているScoreRankParameterを、各しきい値が「大きい→小さい」になるようにソート
        scoreRanks.Sort((a, b) => b.scoreThreshold.CompareTo(a.scoreThreshold));
    }
}
