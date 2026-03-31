using UnityEngine;

[System.Serializable]
public class FlyParameter : TargetParameterBase
{
    public FlyParameter()
    {
        speed = new Vector2(5.0f, 2.0f);
        animationSpeed = 1.0f; // 必ず0以上
        size = 1.0f;

        enableSpawn = true;
        spawnFrequency = 1; // 必ず正の値
        spawnRange = 0.50f; // 必ず0~1の間

        isReward = true;
        canCollide = false;

        baseScore = 100;
        collideScore = -100;
    }
}
