using UnityEngine;

[System.Serializable]
public class AppleParameter : TargetParameterBase
{
    public AppleParameter()
    {
        speed = new Vector2(0f, -9.80f); // 必ずYは負の値
        animationSpeed = 1.0f; // 必ず0以上
        size = 0.80f;

        enableSpawn = true;
        spawnFrequency = 2; // 必ず正の値
        spawnRange = 0.80f; // 必ず0~1の間

        isReward = true;
        canCollide = false;

        baseScore = 200;
        collideScore = -200;
    }
}
