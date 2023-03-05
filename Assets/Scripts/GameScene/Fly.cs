using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Fly : MonoBehaviour
{
    private string[] flyTypes = {
        "toLeft", "toRight", "toLeftWave", "toRightWave"
    };
    private string flyType;
    private Vector3 direction;
    
    public int speed;

    void Start() 
    {
        speed = ParameterManager.flySpeed;
        this.Init();
    }

    void LateUpdate()
    {
        this.transform.localPosition += direction * speed * Time.deltaTime;
        if (flyType == "toLeftWave" || flyType == "toRightWave")
        {
            var temp = transform.localPosition;
            temp.y = Mathf.Sin(transform.localPosition.x);
            this.transform.localPosition = temp;
        }
        // else if (flyType == "toLeftGiza" || flyType == "toRightGiza")
        // {
        //     var temp = transform.localPosition;
        //     if (pos.y + 1 < temp.y) verticalDirection = "down";
        //     else if (pos.y -1 > temp.y) verticalDirection = "up";
        //     if (verticalDirection == "up") temp.y += 0.02f;
        //     else if (verticalDirection == "down") temp.y -= 0.02f;
        //     transform.localPosition = temp;
        // }

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
        Vector3 startingPosition = Vector3.zero;
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
