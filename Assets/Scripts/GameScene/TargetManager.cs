using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private float flyFrequency = 10 / ParameterManager.flyFrequency;
    private float appleFrequency = 10 / ParameterManager.appleFrequency;

    [SerializeField]
    private PoolManager flyPool;
    [SerializeField]
    private PoolManager applePool;

    [SerializeField]
    private GameObject flyPrefab;

    [SerializeField]
    Vector3 minSpawnPosition = Vector3.zero;
    [SerializeField]
    Vector3 maxSpawnPosition = Vector3.zero;

    Coroutine flySpawnTimer;
    Coroutine appleSpawnTimer;

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
            Destroyer destroyer = flyPool.GetFromPool().GetComponent<Destroyer>();
            destroyer.pool = flyPool;
            destroyer.StartDestroyTimer();
            yield return new WaitForSeconds(flyFrequency);
        }
    }

    private IEnumerator AppleSpawnTimer()
    {
        while (true)
        {
            Destroyer destroyer = applePool.GetFromPool().GetComponent<Destroyer>();
            destroyer.pool = applePool;
            destroyer.StartDestroyTimer();
            yield return new WaitForSeconds(appleFrequency);
        }
    }
}