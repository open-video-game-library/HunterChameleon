using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class SensitivitySliderController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text valueText;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = ParameterManager.Instance.parameter.input.sensitivity;
        valueText.text = ParameterManager.Instance.parameter.input.sensitivity.ToString("F2");

        slider.onValueChanged.AddListener(ApplySensitivity);
    }

    private void ApplySensitivity(float value)
    {
        ParameterManager.Instance.parameter.input.sensitivity = value;
        valueText.text = value.ToString("F2");
    }
}
