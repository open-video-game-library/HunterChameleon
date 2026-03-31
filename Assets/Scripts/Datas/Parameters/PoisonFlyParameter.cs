using UnityEngine;

[System.Serializable]
public class PoisonFlyParameter : TargetParameterBase
{
    public PoisonFlyParameter()
    {
        speed = new Vector2(5.0f, 2.0f);
        animationSpeed = 1.0f; // 必ず0以上
        size = 1.50f;

        enableSpawn = true;
        spawnFrequency = 5; // 必ず正の値
        spawnRange = 0.50f; // 必ず0~1の間

        isReward = false;
        canCollide = false;

        baseScore = -300;
        collideScore = -100;
    }
}
