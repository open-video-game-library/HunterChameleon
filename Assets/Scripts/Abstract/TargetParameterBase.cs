using UnityEngine;

public abstract class TargetParameterBase
{
    public Vector2 speed;
    public float animationSpeed;
    public float size;

    public bool enableSpawn;
    public float spawnFrequency;
    public float spawnRange;

    public bool isReward;
    public bool canCollide;

    public float baseScore;
    public float collideScore;

    public void Validate()
    {
        animationSpeed = Mathf.Max(0, animationSpeed);
        spawnFrequency = Mathf.Max(0.01f, spawnFrequency);
        spawnRange = Mathf.Clamp01(spawnRange);
    }
}
