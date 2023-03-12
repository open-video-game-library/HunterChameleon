using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class GamepadSensitivity : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        slider.value = int.Parse(ParameterManager.gamepadSensitivity.ToString("f0"));
        valueText.text = slider.value.ToString("f0");
        if (Gamepad.current == null)
        {
            #if !UNITY_WEBGL
                slider.interactable = false;
                valueText.text = "No connected";
            #endif
        }
        else
        {
            slider.interactable = true;
        }
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f0");
        ParameterManager.gamepadSensitivity = int.Parse(slider.value.ToString("f0"));
    }
}

