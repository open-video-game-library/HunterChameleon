using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer tongueSpriteRenderer;
    private int tongueSpeed;

    private bool isShooting;

    [SerializeField]
    private Score score;

    AudioSource audioSource;
    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip achieveSound;

    [SerializeField]
    private Reticle reticle;
    private Vector3 shotPosition;

    [SerializeField]
    private PoolManager judgePool;
    [SerializeField]
    private Sprite[] judges = new Sprite[4];

    [System.NonSerialized]
    public int hitNum;

    void Start()
    {
        Init();
        
        isShooting = false;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    void LateUpdate()
    {
    }

    public IEnumerator Shoot(Vector3 triggeredPosition)
    {
        if (!isShooting)
        {
            shotPosition = reticle.transform.position;
            audioSource.PlayOneShot(shootSound);
            isShooting = true;
            float scaleY = 0.5f;
            bool isReached = false;
            float angle = GetAngle(transform.position, triggeredPosition);
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            float distance = Vector3.Distance(transform.position, triggeredPosition);

            while (isShooting)
            {
                if (!isReached)
                {
                    scaleY += tongueSpeed * Time.deltaTime;
                    if (scaleY >= distance)
                    {
                        isReached = true;
                        scaleY = distance;
                    }
                }
                else
                {
                    scaleY -= tongueSpeed * Time.deltaTime;
                    if (scaleY <= 0.5f)
                    {
                        isShooting = false;
                        scaleY = 0.5f;
                        yield break;
                    }
                }
                transform.localScale = new Vector3(0.5f, scaleY, 0.5f);
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if(collidedObject.activeSelf && collidedObject.CompareTag("Target"))
        {
            hitNum++;
            
            Vector3 collidedPosition = collidedObject.transform.position;
            collidedPosition.z = 1.0f;

            GameObject judge = judgePool.GetFromPool();
            judge.transform.position = collidedPosition;

            Destroyer destroyer = judge.GetComponent<Destroyer>();
            destroyer.pool = judgePool;
            destroyer.StartDestroyTimer1sec();

            if (collidedObject.name.Contains("Fly"))
            {
                float distance = Vector3.Distance(shotPosition, collidedPosition);

                if(distance < 1.0f)
                {
                    judge.GetComponent<SpriteRenderer>().sprite = judges[1];
                    audioSource.PlayOneShot(achieveSound);
                }
                else if (distance < 5.0f) 
                {
                    judge.GetComponent<SpriteRenderer>().sprite = judges[2];
                }
                else
                {
                    judge.GetComponent<SpriteRenderer>().sprite = judges[3];
                }
                
                audioSource.PlayOneShot(hitSound);
                score.AddScore(300);
                collidedObject.GetComponent<Destroyer>().pool.ReleaseToPool(collidedObject);
            }

            if (collidedObject.name.Contains("Apple"))
            {            
                judge.GetComponent<SpriteRenderer>().sprite = judges[0];

                audioSource.PlayOneShot(hitSound);
                audioSource.PlayOneShot(achieveSound);
                score.AddScore(500);
                collidedObject.GetComponent<Destroyer>().pool.ReleaseToPool(collidedObject);
            }
        }
    }

    private float GetAngle(Vector2 from, Vector2 to)
    {
        float dx = to.x - from.x;
        float dy = to.y - from.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

    public void Init()
    {
        tongueSpriteRenderer.color = new Color32(
            (byte)ParameterManager.tongueColorRed, 
            (byte)ParameterManager.tongueColorGreen, 
            (byte)ParameterManager.tongueColorBlue,
            (byte)ParameterManager.tongueColorAlpha
        );
        tongueSpeed = ParameterManager.tongueSpeed;
    }
}
