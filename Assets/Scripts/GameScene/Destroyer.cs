using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [System.NonSerialized]
    public PoolManager pool;
    
    private bool isElapsed;

    private int maxTime = 10;

    public void StartDestroyTimer()
    {
        isElapsed = false;
        StartCoroutine(DestroyTimer());
    }

    public void StartDestroyTimer1sec()
    {
        StartCoroutine(DestroyTimer1sec());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2);
        isElapsed = true;

        yield return new WaitForSeconds(maxTime - 2);
        pool.ReleaseToPool(this.gameObject);
        yield break;
    }

    private IEnumerator DestroyTimer1sec ()
    {
        yield return new WaitForSeconds(1);
        pool.ReleaseToPool(this.gameObject);
        yield break;
    }

    private void OnBecameInvisible()
	{
        if (this.gameObject.activeSelf && isElapsed)
        {
            pool.ReleaseToPool(this.gameObject);
        }
	}
}
