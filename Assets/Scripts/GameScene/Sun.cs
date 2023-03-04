using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sun : MonoBehaviour
{
    [SerializeField]
    private GameObject sun;

    [SerializeField]
    private GameObject axis;

    private float playTime = ParameterManager.playTime;

    private float angle;
    private float color;

    void Start()
    {
        sun.GetComponent<Light2D>().color = new Color32(192, 255, 255, 255);
        angle = 360 / playTime;
        color = 256 / playTime;
    }

    void LateUpdate()
    {        
    }

    public void StartSunMove()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer = 0.0f;
        float colorRed = 192.0f;
        float colorGreen = 255.0f;
        float colorBlue = 255.0f;
        
        while (true)
        {
            timer += Time.deltaTime;
            this.transform.RotateAround(axis.transform.position, -Vector3.forward, angle * Time.deltaTime);

            if (timer < (playTime / 4) * 2)
            {
                colorRed += (color / 2.0f) * Time.deltaTime;
            }
            else if (timer < (playTime / 4) * 3)
            {
                colorRed = 255.0f;
                colorGreen -= color * Time.deltaTime;
                colorBlue -= color * 2.0f * Time.deltaTime;
            }
            else if (timer < (playTime / 4) * 4)
            {
                colorRed -= color * 2.0f * Time.deltaTime;
                colorGreen -= color * Time.deltaTime;
                colorBlue = 128.0f;
            }
            else
            {
                colorRed = 128.0f;
                colorGreen = 128.0f;
                colorBlue = 128.0f;
                yield break;
            }
            sun.GetComponent<Light2D>().color = new Color32((byte)colorRed, (byte)colorGreen, (byte)colorBlue, 255);

            yield return null;
        }
    }
}
