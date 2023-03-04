using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private Tongue tongue;

    [SerializeField]
    private SpriteRenderer reticleSpriteRenderer;

    void Start()
    {
        Cursor.visible = false;
        reticleSpriteRenderer.color = new Color32(
            (byte)ParameterManager.tongueColorRed, 
            (byte)ParameterManager.tongueColorGreen, 
            (byte)ParameterManager.tongueColorBlue,
            (byte)ParameterManager.tongueColorAlpha
        );
    }

    void Update()
    {
        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(Mathf.Clamp(cursorPosition.x, -9.0f, 9.0f), Mathf.Clamp(cursorPosition.y, -3.5f, 5.0f), 1.0f);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 temp = transform.position;
            temp.z = 0;
            StartCoroutine(tongue.Shoot(temp));
        }
    }
}
