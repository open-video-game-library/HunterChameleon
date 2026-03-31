using UnityEngine;

public interface ITargetSpawner
{
    public void TickSpawn();
    public void SpawnTarget();
    public Vector3 GetInitialPosition(float spawnRange);
    public Quaternion GetInitialRotation();
}