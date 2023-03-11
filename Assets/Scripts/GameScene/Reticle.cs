using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private Tongue tongue;

    [SerializeField]
    private SpriteRenderer reticleSpriteRenderer;
    [SerializeField]
    private SpriteRenderer reticleOutlineSpriteRenderer;

    [System.NonSerialized]
    public int triggerNum;

    private Vector2 preMousePosition;

    private float gamepadSensitivity;

    void Start()
    {
        Cursor.visible = false;
        this.Init();
        preMousePosition = (Mouse.current).position.ReadValue();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            var mouseInput = Mouse.current;
            var mouseLeftButton = mouseInput.leftButton;
            var mousePosition = mouseInput.position.ReadValue();

            if (preMousePosition != mousePosition)
            {
                var cursorPosition = Camera.main.ScreenToWorldPoint(mouseInput.position.ReadValue());
                this.transform.position = new Vector3(Mathf.Clamp(cursorPosition.x, -9.0f, 9.0f), Mathf.Clamp(cursorPosition.y, -3.5f, 5.0f), 1.0f);
            }

            if (mouseLeftButton.wasPressedThisFrame)
            {
                this.trigger();
            }

            preMousePosition = mouseInput.position.ReadValue();
        }
        
        if (Gamepad.current != null)
        {
            var gamepadInput = Gamepad.current;
            var gamepadSouthButton = gamepadInput.buttonSouth;
            var gamepadDpad = gamepadInput.dpad.ReadValue();
            var gamepadLeftStick = gamepadInput.leftStick.ReadValue();

            if (gamepadDpad != Vector2.zero)
            {
                var currentPosition = this.transform.position;
                this.transform.position = new Vector3(Mathf.Clamp(currentPosition.x + gamepadDpad.x * gamepadSensitivity, -9.0f, 9.0f), Mathf.Clamp(currentPosition.y + gamepadDpad.y * gamepadSensitivity, -3.5f, 5.0f), 1.0f);
            }

            if (gamepadLeftStick != Vector2.zero)
            {
                var currentPosition = this.transform.position;
                this.transform.position = new Vector3(Mathf.Clamp(currentPosition.x + gamepadLeftStick.x * gamepadSensitivity, -9.0f, 9.0f), Mathf.Clamp(currentPosition.y + gamepadLeftStick.y * gamepadSensitivity, -3.5f, 5.0f), 1.0f);
            }

            if (gamepadSouthButton.wasPressedThisFrame)
            {
                this.trigger();
            }
        }
    }

    private void trigger ()
    {
        if (TimeKeeper.isPlaying)
        {
            triggerNum++;
        }
        Vector3 temp = this.transform.position;
        temp.z = 0;
        StartCoroutine(tongue.Shoot(temp));
    }

    public void Init ()
    {
        gamepadSensitivity = ParameterManager.gamepadSensitivity;
        reticleSpriteRenderer.color = new Color32(
            (byte)ParameterManager.tongueColorRed, 
            (byte)ParameterManager.tongueColorGreen, 
            (byte)ParameterManager.tongueColorBlue,
            (byte)ParameterManager.tongueColorAlpha
        );
        reticleOutlineSpriteRenderer.color = new Color32(
            (byte)ParameterManager.tongueColorRed, 
            (byte)ParameterManager.tongueColorGreen, 
            (byte)ParameterManager.tongueColorBlue,
            (byte)ParameterManager.tongueColorAlpha
        );
    }
}
