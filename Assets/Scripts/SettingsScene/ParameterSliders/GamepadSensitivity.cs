using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GamepadSensitivity : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI valueText;

    void Start()
    {
        slider.value = float.Parse(ParameterManager.gamepadSensitivity.ToString("f2")); 
        if (Gamepad.current == null)
        {
            slider.interactable = false;
            valueText.text = "No connected";
        }
        else
        {
            slider.interactable = true;
            valueText.text = slider.value.ToString("f2");
        }
    }

    public void OnChanged ()
    {
        valueText.text = slider.value.ToString("f2");
        ParameterManager.gamepadSensitivity = float.Parse(slider.value.ToString("f2"));
    }
}

