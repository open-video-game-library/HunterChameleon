using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> reticleRenderers;
    private Dictionary<SpriteRenderer, ReticleColor32> reticleRenderersDict;

    private ReticleParameter parameter;

    private InputData input;

    private void Start()
    {
        // パラメータをセット
        parameter = ParameterManager.Instance.parameter.reticle;

        // 参照する入力データをセット
        input = InputDataManager.Instance.inputData;

        // Dictionary を初期化
        reticleRenderersDict = new Dictionary<SpriteRenderer, ReticleColor32>();
        InitRendererDict();

        // Rendererに指定の色をセット
        ApplyColor();
    }

    private void Update()
    {
        Move(input.position);

        // 今後、OnChanged関数から呼ぶ
        ApplyColor();
    }

    private void InitRendererDict()
    {
        int parameterListCount = parameter.reticleColors.Count;

        // Parameterが一つも登録されていない場合はreturnする
        if (parameterListCount < 1) { return; }

        for (int i = 0; i < reticleRenderers.Count; i++)
        {
            if (parameterListCount < i + 1)
            {
                // 登録されているRendererの数がParameterに登録されている数より多い場合は、Parameterに登録されている最後のインデックスの要素をvalueに登録
                reticleRenderersDict.Add(reticleRenderers[i], parameter.reticleColors[parameterListCount - 1]);
            }
            else
            {
                // それ以外の場合は、普通に登録
                reticleRenderersDict.Add(reticleRenderers[i], parameter.reticleColors[i]);
            }
        }
    }

    private void ApplyColor()
    {
        foreach (var renderer in reticleRenderers)
        {
            renderer.color = reticleRenderersDict[renderer].color;
        }
    }

    private void Move(Vector2 position)
    {
        transform.position = position;
    }
}
