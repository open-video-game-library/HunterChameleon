using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [SerializeField]
    private Score score;

    AudioSource audioSource;
    [SerializeField]
    private AudioClip damageSound;

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if(collidedObject.activeSelf)
        {
            if (collidedObject.name.Contains("Apple"))
            {
                audioSource.PlayOneShot(damageSound);
                StartCoroutine(score.makeTextDamaged());
                score.AddScore(-500);
                collidedObject.GetComponent<Destroyer>().pool.ReleaseToPool(collidedObject);
            }
        }
    }
}
