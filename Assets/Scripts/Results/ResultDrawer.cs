using UnityEngine;
using UnityEngine.UI;

public class ResultDrawer : MonoBehaviour
{
    private ScoreRankParameters parameters;

    [SerializeField]
    private TextUpdater scoreText;
    [SerializeField]
    private TextUpdater rankText;
    [SerializeField]
    private Image rankBackgroundImage;

    [SerializeField]
    private TextUpdater shotCountText;
    [SerializeField]
    private TextUpdater hitCountText;
    [SerializeField]
    private TextUpdater missHitCountText;
    [SerializeField]
    private TextUpdater accuracyText;
    [SerializeField]
    private TextUpdater maxComboText;

    private void Start()
    {
        parameters = ParameterManager.Instance.parameter.rank;

        if (GameStatsUpdater.Instance)
        {
            GameStatsUpdater stats = GameStatsUpdater.Instance;

            scoreText.UpdateText("Score:" + stats.Score.ToString());

            // ランク算出（ターゲットの滞在時間は、ターゲットの挙動や速度パラメータによって左右されるため、2.0秒で概算）
            float idealScore = IdealScoreCalculater.CalculateIdealScore(stats.TargetsInfo, stats.PlayTime, 2.0f);
            rankText.UpdateText(parameters.GetRank(stats.GetScoreAchievementRatio(idealScore)).rankLabel);
            rankText.ChangeTextColor(parameters.GetRank(stats.GetScoreAchievementRatio(idealScore)).displayColor);
            rankBackgroundImage.color = parameters.GetRank(stats.GetScoreAchievementRatio(idealScore)).displayColor;
            Debug.Log("Ideal Score: " + idealScore + ", Achivement Ratio: " + stats.GetScoreAchievementRatio(idealScore));

            // 統計データ
            shotCountText.UpdateText("Shot Count: " + stats.ShotCount.ToString());
            hitCountText.UpdateText("Hit: " + stats.HitCount.ToString());
            missHitCountText.UpdateText("Miss Hit: " + stats.MissHitCount.ToString());
            accuracyText.UpdateText("Accuracy: " + stats.GetAccuracy().ToString("P1"));
            maxComboText.UpdateText("Max Combo: " + stats.MaxComboCount.ToString());

            return;
        }

        scoreText.UpdateText("Score:-");

        rankText.UpdateText(parameters.GetRank(float.NegativeInfinity).rankLabel);
        rankText.ChangeTextColor(parameters.GetRank(float.NegativeInfinity).displayColor);
        rankBackgroundImage.color = parameters.GetRank(float.NegativeInfinity).displayColor;

        shotCountText.UpdateText("Shot Count: -");
        hitCountText.UpdateText("Hit: -");
        missHitCountText.UpdateText("Miss Hit: -");
        accuracyText.UpdateText("Accuracy: -");
        maxComboText.UpdateText("Max Combo: -");

        Debug.LogWarning("結果データが未設定です。");
    }
}
