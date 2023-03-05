using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlyFrequencySlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        valueText.text = slider.value.ToString("f0");
        slider.value = int.Parse(ParameterManager.flyFrequency.ToString("f0"));   
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f0");
        ParameterManager.flyFrequency = int.Parse(slider.value.ToString("f0"));
    }
}
