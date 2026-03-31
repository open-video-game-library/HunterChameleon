using UnityEngine;

public struct SpawnedTargetsInfo
{
    public string targetName;

    public bool isReward;
    public bool canCollide;

    public float baseScore;
    public float collideScore;

    public Vector2 spawnPosition;
    public float spawnedTime;

    public SpawnedTargetsInfo(string targetName, bool isReward, bool canCollide, float baseScore, float collideScore, Vector2 spawnPosition, float spawnedTime)
    {
        this.targetName = targetName;
        this.baseScore = baseScore;
        this.isReward = isReward;
        this.canCollide = canCollide;
        this.collideScore = collideScore;
        this.spawnPosition = spawnPosition;
        this.spawnedTime = spawnedTime;
    }
}
