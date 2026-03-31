using UnityEngine;

[System.Serializable]
public class GoldFlyParameter : TargetParameterBase
{
    public GoldFlyParameter()
    {
        speed = new Vector2(7.50f, 3.0f);
        animationSpeed = 1.0f; // 必ず0以上
        size = 0.80f;

        enableSpawn = true;
        spawnFrequency = 7; // 必ず正の値
        spawnRange = 0.50f; // 必ず0~1の間

        isReward = true;
        canCollide = false;

        baseScore = 800;
        collideScore = -100;
    }
}
