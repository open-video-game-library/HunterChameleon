using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [SerializeField]
    private Score score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if(collidedObject.activeSelf)
        {
            if (collidedObject.name.Contains("Apple"))
            {
                StartCoroutine(score.makeTextDamaged());
                score.AddScore(-500);
                collidedObject.GetComponent<Destroyer>().pool.ReleaseToPool(collidedObject);
            }
        }
    }
}
