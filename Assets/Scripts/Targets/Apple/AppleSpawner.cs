using UnityEngine;

public class AppleSpawner : MonoBehaviour, ITargetSpawner
{
    [SerializeField]
    private TargetPoolController pool;

    private Camera mainCamera;

    private AppleParameter parameter;

    private float spawnTimer;

    private void Start()
    {
        mainCamera = Camera.main;
        parameter = ParameterManager.Instance.parameter.target.apple;
        spawnTimer = 0f;
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
        float left = mainCamera.ViewportToWorldPoint(new Vector3(0.50f - spawnRange / 2.0f, 0.50f, z)).x;
        float right = mainCamera.ViewportToWorldPoint(new Vector3(0.50f + spawnRange / 2.0f, 0.50f, z)).x;
        float startPosX = Random.Range(left, right);

        // Y座標を設定
        float up = Calculate.GetScreenEdgePositive().y; // ゲーム画面の上端中央のワールド座標
        float startPosY = up;

        return new Vector3(startPosX, startPosY, currentPosition.z);
    }

    public Quaternion GetInitialRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z = Random.Range(0f, 360f);
        return Quaternion.Euler(currentRotation);
    }
}
