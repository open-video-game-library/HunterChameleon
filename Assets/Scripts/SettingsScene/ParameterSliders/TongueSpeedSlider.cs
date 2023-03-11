using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TongueSpeedSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        valueText.text = slider.value.ToString("f0");
        slider.value = int.Parse(ParameterManager.tongueSpeed.ToString("f0")); 
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f0");
        ParameterManager.tongueSpeed = int.Parse(slider.value.ToString("f0"));
    }
}
