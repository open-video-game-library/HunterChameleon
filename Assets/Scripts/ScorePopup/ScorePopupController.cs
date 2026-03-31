using TMPro;
using UnityEngine;

public class ScorePopupController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text baseScoreLabel;
    [SerializeField]
    private MeshRenderer baseScoreMeshRenderer;

    [SerializeField]
    private TMP_Text comboBonusLabel;
    [SerializeField]
    private MeshRenderer comboBonusMeshRenderer;

    [Header("Order in Layer")]
    [SerializeField] private int layer;

    // 出現してから自動で表示になるまでの時間（寿命）
    private readonly float lifeTime = 0.50f;

    private void OnEnable()
    {
        // 一定時間後に非表示にする
        Invoke(nameof(DisableSelf), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void SetJudgeText(string baseScoreText, string comboBonusText, Color32 baseScoreColor, Color32 comboBonusColor)
    {
        baseScoreLabel.text = baseScoreText;
        baseScoreLabel.color = baseScoreColor;
        baseScoreMeshRenderer.sortingOrder = layer;

        comboBonusLabel.text = comboBonusText;
        comboBonusLabel.color = comboBonusColor;
        comboBonusMeshRenderer.sortingOrder = layer;
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
