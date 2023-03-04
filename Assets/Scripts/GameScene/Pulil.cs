using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulil : MonoBehaviour
{
    [SerializeField]
    private GameObject reticle;

    [SerializeField]
    private bool isRight;

    void Start()
    {

    }

    void LateUpdate()
    {
        var direction = reticle.transform.position - this.transform.position;
        if(isRight)
        {
            direction.x += 1.5f;
        }
        else
        {
            direction.x -= 1.5f;
        }
        var angle = GetAngle(Vector3.zero, direction);
        this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle + 90);
    }

    float GetAngle(Vector2 from, Vector2 to)
    {
        float dx = to.x - from.x;
        float dy = to.y - from.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
}

