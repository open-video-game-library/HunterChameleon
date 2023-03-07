using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Fly : MonoBehaviour
{
    private string[] flyTypes = {
        "toLeft", "toRight", "toLeftWave", "toRightWave", "toLeftGiza", "toRightGiza"
    };
    private string flyType;
    private Vector3 direction;

    private bool isUp = true;

    private Vector3 startingPosition;

    void Start() 
    {
        this.Init();
    }

    void LateUpdate()
    {
        this.transform.localPosition += direction * ParameterManager.flySpeed * Time.deltaTime;
        if (flyType == "toLeftWave" || flyType == "toRightWave")
        {
            var temp = transform.localPosition;
            temp.y = Mathf.Sin(transform.localPosition.x);
            this.transform.localPosition = temp;
        }
        else if (flyType == "toLeftGiza" || flyType == "toRightGiza")
        {
            var temp = transform.localPosition;
            if (startingPosition.y + 1 < temp.y) isUp = false;
            else if (startingPosition.y -1 > temp.y) isUp = true;
            if (isUp) temp.y += (ParameterManager.flySpeed / 2) * Time.deltaTime;
            else temp.y -= (ParameterManager.flySpeed / 2) * Time.deltaTime;
            this.transform.localPosition = temp;
        }

        if(!TimeKeeper.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        this.Init();
    }


    public void Init()
    {
        startingPosition = Vector3.zero;
        Vector3 startingScale = this.transform.localScale;
        flyType = flyTypes[Random.Range(0, flyTypes.Length)];
        switch (flyType)
        {
            case "toLeft":
            case "toLeftWave":
            case "toLeftGiza":
                startingScale.x = 1.0f;
                startingPosition.x = 9.5f;
                direction = Vector2.left;
                break;
            case "toRight":
            case "toRightWave":
            case "toRightGiza":
                startingScale.x = -1.0f;
                startingPosition.x = -9.5f;
                direction = Vector2.right;
                break;
        }
        startingPosition.y = Random.Range(4.5f, -2.5f);
        startingPosition.z = 1.0f;
        direction.z = 1.0f;
        this.transform.position = startingPosition;
        this.transform.localScale = startingScale;
    }
}
