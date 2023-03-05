using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppleSpeedSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        valueText.text = slider.value.ToString("f1");
        slider.value = float.Parse(ParameterManager.appleGravityScale.ToString("f1")); 
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f1");
        ParameterManager.appleGravityScale = float.Parse(slider.value.ToString("f1"));
    }
}
