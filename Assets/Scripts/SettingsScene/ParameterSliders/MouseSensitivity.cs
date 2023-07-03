using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        slider.value = int.Parse(ParameterManager.mouseSensitivity.ToString("f0"));
        valueText.text = slider.value.ToString("f0");
        if (Mouse.current == null)
        {

        }
        else
        {
            slider.interactable = true;
        }
    }

    public void OnChanged()
    {
        valueText.text = slider.value.ToString("f0");
        ParameterManager.mouseSensitivity = int.Parse(slider.value.ToString("f0"));
    }
}
