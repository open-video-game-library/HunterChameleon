using UnityEngine;

[System.Serializable]
public class PoisonAppleParameter : TargetParameterBase
{
    public PoisonAppleParameter()
    {
        speed = new Vector2(0f, -9.80f); // 必ずYは負の値
        animationSpeed = 1.0f; // 必ず0以上
        size = 0.80f;

        enableSpawn = true;
        spawnFrequency = 7; // 必ず正の値
        spawnRange = 0.80f; // 必ず0~1の間

        isReward = false;
        canCollide = false;

        baseScore = -500;
        collideScore = -200;
    }
}
