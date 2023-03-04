using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; 

public class PoolManager : MonoBehaviour
{

    ObjectPool<GameObject> pool;

    [SerializeField]
    private GameObject targetPrefab;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            OnCreatePooledObject, 
            OnGetFromPool, 
            OnReleaseToPool, 
            OnDestroyPooledObject)
        ;
    }

    GameObject OnCreatePooledObject()
    {
        return Instantiate(targetPrefab);
    }

    void OnGetFromPool(GameObject target)
    {
        target.SetActive(true);
    }

    void OnReleaseToPool(GameObject target)
    {
        target.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject target)
    {
        Destroy(target);
    }

    public GameObject GetFromPool()
    {
        GameObject target = pool.Get();
        return target;
    }

    public void ReleaseToPool(GameObject target)
    {
        pool.Release(target);
    }
}