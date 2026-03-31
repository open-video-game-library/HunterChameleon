using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunController : MonoBehaviour
{
    private Image panel;

    private InGameParameter parameter;

    private List<Color32> sunColors;

    private void Start()
    {
        panel = GetComponent<Image>();

        // パラメータをセット
        parameter = ParameterManager.Instance.parameter.inGame;
        sunColors = parameter.sunColors;

        Init();
    }

    private void Init()
    {
        // 初期の色をセットしておく
        if (sunColors.Count < 1) { return; }
        SetColor(sunColors[0]);
    }

    public void ChangeColor(float normalizedTime)
    {
        if (sunColors.Count < 2 || normalizedTime > 1.0f) { return; }

        float division = 1.0f / (sunColors.Count - 1);

        Color32 originalColor = sunColors[(int)(normalizedTime * (sunColors.Count - 1))];
        Color32 targetColor = sunColors[Mathf.Min((int)(normalizedTime * (sunColors.Count - 1)) + 1, sunColors.Count - 1)];

        float t = normalizedTime / division - (int)(normalizedTime * (sunColors.Count - 1));
        Color32 currentColor = Color.Lerp(originalColor, targetColor, t);

        SetColor(currentColor);
    }

    public void SetColor(Color32 color)
    {
        panel.color = color;
    }
}
