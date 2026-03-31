using UnityEngine;
using System.Collections.Generic;

public class TargetPoolController : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObjectPrefab;

    private List<GameObject> pool;
    private readonly int poolSize = 5;

    private void Start()
    {
        pool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(targetObjectPrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        
        GameObject newObj = Instantiate(targetObjectPrefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }
}
