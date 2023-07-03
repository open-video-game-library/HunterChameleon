using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    private int gamepadSensitivity;
    private int mouseSensitivity;

    void Start()
    {
        this.Init();
        preMousePosition = (Mouse.current).position.ReadValue();
    }

    Vector2 vec;
    private bool useMouse = true;

    [SerializeField] TextMeshProUGUI debugText;

    public void UseCursor(bool enable)
    {
        if (enable) //use cursor
        {
            useMouse = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            useMouse = false;
            Cursor.visible = false;
            var mouseInput = Mouse.current;
            var mousePosition = mouseInput.position.ReadValue();
            var cursorPosition = Camera.main.ScreenToWorldPoint(mouseInput.position.ReadValue());
            this.transform.position = new Vector3(Mathf.Clamp(cursorPosition.x, -9.0f, 9.0f), Mathf.Clamp(cursorPosition.y, -3.5f, 5.0f), 1.0f);
        }
    }

    void Update()
    {

        Cursor.visible = true;

        if (Mouse.current != null)
        {
            var mouseInput = Mouse.current;
            var mouseLeftButton = mouseInput.leftButton;
            var mousePosition = mouseInput.position.ReadValue();

            

            if (useMouse)
            {
                if (preMousePosition != mousePosition)
                {
                    var cursorPosition = Camera.main.ScreenToWorldPoint(mouseInput.position.ReadValue());
                    this.transform.position = new Vector3(Mathf.Clamp(cursorPosition.x, -9.0f, 9.0f), Mathf.Clamp(cursorPosition.y, -3.5f, 5.0f), 1.0f);
                }
            }
            else
            {
                Screen.lockCursor = true;
                var cursorPosition = this.transform.position
                        + new Vector3(mouseInput.delta.ReadValue().x * 0.002f * mouseSensitivity, mouseInput.delta.ReadValue().y * 0.002f * mouseSensitivity, 0f);
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
                this.transform.position = new Vector3(Mathf.Clamp(currentPosition.x + gamepadDpad.x * gamepadSensitivity * 2 * Time.deltaTime, -9.0f, 9.0f), Mathf.Clamp(currentPosition.y + gamepadDpad.y * gamepadSensitivity * 2 * Time.deltaTime, -3.5f, 5.0f), 1.0f);
            }

            if (gamepadLeftStick != Vector2.zero)
            {
                var currentPosition = this.transform.localPosition;
                this.transform.localPosition = new Vector3(Mathf.Clamp(currentPosition.x + gamepadLeftStick.x * gamepadSensitivity * 2 * Time.deltaTime, -9.0f, 9.0f), Mathf.Clamp(currentPosition.y + gamepadLeftStick.y * gamepadSensitivity * 2 * Time.deltaTime, -3.5f, 5.0f), 1.0f);
            }

            if (gamepadSouthButton.wasPressedThisFrame)
            {
                this.trigger();
            }
        }

        if (debugText != null)
        {

            debugText.text = Cursor.lockState.ToString();
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
        mouseSensitivity = ParameterManager.mouseSensitivity;
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
