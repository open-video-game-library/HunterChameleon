using UnityEngine;

[System.Serializable]
public class GoldAppleParameter : TargetParameterBase
{
    public GoldAppleParameter()
    {
        speed = new Vector2(0f, -14.70f); // 必ずYは負の値
        animationSpeed = 1.0f; // 必ず0以上
        size = 0.60f;

        enableSpawn = true;
        spawnFrequency = 9; // 必ず正の値
        spawnRange = 0.80f; // 必ず0~1の間

        isReward = true;
        canCollide = false;

        baseScore = 1000;
        collideScore = -200;
    }
}
