using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public float gravityScale;

    void Start()
    {
        gravityScale = ParameterManager.appleGravityScale;
        this.Init();
    }

    void LateUpdate()
    {
        this.transform.Rotate(new Vector3(0, 0, 0.5f));

        if(!TimeKeeper.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        this.Init();
    }

    private void Init()
    {
        Vector3 startingPosition = Vector3.zero;
        startingPosition.x = Random.Range(9.0f, -9.0f);
        startingPosition.y = 6.0f;
        startingPosition.z = 1.0f;
        this.transform.position = startingPosition;
    }
}
