using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    void Start()
    {
        this.Init();
    }

    void Update()
    {
        
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
        this.Init();
    }

    private void Init()
    {
        Vector3 startingPosition = Vector3.zero;
        startingPosition.x = Random.Range(4.5f, -4.5f);
        startingPosition.y = 6.0f;
        startingPosition.z = 1.0f;
        this.transform.position = startingPosition;
    }
}
