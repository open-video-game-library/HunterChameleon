using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TongueTipController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ChameleonParameter parameter;

    // 処理順に並べる（UI → Target）
    private ITongueHitProcessor[] processors;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parameter = ParameterManager.Instance.parameter.chameleon;

        if (processors == null || processors.Length == 0) { processors = GetComponents<ITongueHitProcessor>(); }
    }

    private float GetRadius()
    {
        return spriteRenderer.bounds.extents.x * parameter.hitRadiusMultiplier;
    }

    public void CheckHits()
    {
        Vector2 p = transform.position;
        float r = GetRadius();

        foreach (var proc in processors)
        {
            // 何か処理できたら終了（コンボも切らない）
            if (proc != null && proc.TryProcess(p, r)) { return; }
        }

        // どれも処理しなかった＝ヒット無し → コンボ終了
        if (ComboCounter.Instance) { ComboCounter.Instance.EndCombo(); }
    }
}
