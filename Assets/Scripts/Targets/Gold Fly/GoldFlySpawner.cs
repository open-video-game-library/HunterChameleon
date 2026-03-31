using UnityEngine;

public class GoldFlySpawner : MonoBehaviour, ITargetSpawner
{
    [SerializeField]
    private TargetPoolController pool;

    private Camera mainCamera;

    private GoldFlyParameter parameter;
    private FlyDirection direction;

    private float spawnTimer;

    private void Start()
    {
        mainCamera = Camera.main;
        parameter = ParameterManager.Instance.parameter.target.goldFly;
    }

    public void TickSpawn()
    {
        // Spawnしない設定になっている場合は、以降の処理を呼ばない
        if (!parameter.enableSpawn) { return; }

        // 経過時間を加算
        spawnTimer += Time.deltaTime;

        // 現在の頻度（秒）を取得
        float currentFrequency = parameter.spawnFrequency;

        // 頻度に達したらスポーンしてタイマーをリセット
        if (spawnTimer >= currentFrequency)
        {
            SpawnTarget();
            spawnTimer = 0f;
        }
    }

    public void SpawnTarget()
    {
        GameObject obj = pool.GetPooledObject();

        direction = Calculate.GetRandomEnumValue<FlyDirection>();
        obj.transform.position = GetInitialPosition(parameter.spawnRange);
        obj.transform.rotation = GetInitialRotation();

        if (GameStatsUpdater.Instance && TimeKeeper.Instance)
        {
            GameStatsUpdater.Instance.OnTargetSpawned(
                obj.name.Replace("(Clone)", "").Trim(),
                parameter.isReward,
                parameter.canCollide,
                parameter.baseScore,
                parameter.collideScore,
                obj.transform.position,
                TimeKeeper.Instance.GetCurrentTime()
            );
        }
    }

    public Vector3 GetInitialPosition(float spawnRange)
    {
        Vector3 currentPosition = transform.position;

        float z = Mathf.Abs(mainCamera.transform.position.z);

        // X座標を設定
        float left = Calculate.GetScreenEdgeNegative().x; // ゲーム画面の左端中央のワールド座標
        float right = Calculate.GetScreenEdgePositive().x; // ゲーム画面の右端中央のワールド座標
        float baseX = (direction == FlyDirection.Left) ? right : left;
        float startPosX = baseX - transform.localScale.x * (int)direction;

        // Y座標を設定
        float spawnRangeUp = mainCamera.ViewportToWorldPoint(new Vector3(0.50f, 0.50f + spawnRange / 2.0f, z)).y;
        float spawnRangeBottom = mainCamera.ViewportToWorldPoint(new Vector3(0.50f, 0.50f - spawnRange / 2.0f, z)).y;
        float startPosY = Random.Range(spawnRangeBottom, spawnRangeUp);

        return new Vector3(startPosX, startPosY, currentPosition.z);
    }

    public Quaternion GetInitialRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y = Mathf.Atan2(0f, (int)direction) * Mathf.Rad2Deg;
        return Quaternion.Euler(currentRotation);
    }
}
