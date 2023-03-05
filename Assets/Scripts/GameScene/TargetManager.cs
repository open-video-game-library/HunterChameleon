using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private float flyFrequency;
    private float appleFrequency;

    [SerializeField]
    private PoolManager flyPool;
    [SerializeField]
    private PoolManager applePool;

    Coroutine flySpawnTimer;
    Coroutine appleSpawnTimer;

    void Start () 
    {
        Init();
    }

    public void StartSpawn ()
    {
        flySpawnTimer = StartCoroutine(FlySpawnTimer());
        appleSpawnTimer = StartCoroutine(AppleSpawnTimer());
    }

    public void StopSpawn ()
    {
        StopCoroutine(flySpawnTimer);
        StopCoroutine(appleSpawnTimer);
    }

    private IEnumerator FlySpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(flyFrequency);
            Destroyer destroyer = flyPool.GetFromPool().GetComponent<Destroyer>();
            destroyer.pool = flyPool;
            destroyer.StartDestroyTimer();
        }
    }

    private IEnumerator AppleSpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(appleFrequency);
            Destroyer destroyer = applePool.GetFromPool().GetComponent<Destroyer>();
            destroyer.pool = applePool;
            destroyer.StartDestroyTimer();
        }
    }

    public void Init ()
    {
        flyFrequency = 10.0f / ParameterManager.flyFrequency;
        appleFrequency = 10.0f / ParameterManager.appleFrequency;
    }
}