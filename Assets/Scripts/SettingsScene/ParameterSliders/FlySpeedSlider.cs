using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlySpeedSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        valueText.text = slider.value.ToString("f3");
        slider.value = float.Parse(ParameterManager.flySpeed.ToString("f3")); 
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f3");
        ParameterManager.flySpeed = float.Parse(slider.value.ToString("f3"));
    }
}
