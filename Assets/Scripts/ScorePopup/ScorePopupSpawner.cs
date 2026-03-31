using UnityEngine;

public class ScorePopupSpawner : MonoBehaviour
{
    public static ScorePopupSpawner Instance;

    private ScorePopupParameter parameter;

    [SerializeField]
    private ScorePopupPoolController pool;

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
        parameter = ParameterManager.Instance.parameter.scorePopup;
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void SpawnScorePopup(Vector3 targetPosition, float baseScore, bool isCombo, float comboBonus)
    {
        GameObject obj = pool.GetPooledObject();

        // transformの設定
        obj.transform.position = new Vector3(targetPosition.x, targetPosition.y);

        // 以下、ラベル情報の設定
        if (!obj.TryGetComponent(out ScorePopupController scorePopupController)) { return; }

        // ラベルのテキストを設定
        string baseScoreSignLabel = (baseScore > 0f ? "+" : "") + baseScore.ToString();
        string comboBonusLabel = comboBonus != 1.0f && isCombo ? "x" + comboBonus.ToString() : "";

        // ラベルの表示色を設定
        Color32 baseScoreColor = baseScore >= 0f ? parameter.rewardDisplayColor : parameter.penaltyDisplayColor;
        Color32 comboBonusColor = parameter.comboBonusDisplayColor;

        // ラベルの生成
        scorePopupController.SetJudgeText(baseScoreSignLabel, comboBonusLabel, baseScoreColor, comboBonusColor);
    }
}
