using UnityEngine;

[RequireComponent(typeof(Animator), typeof(FlyBehaviour))]
public class PoisonFlyController : MonoBehaviour, ITarget
{
    private PoisonFlyParameter parameter;

    private Animator animator;
    private FlyBehaviour behaviour;

    public bool IsReward => parameter?.isReward ?? false;
    public bool CanCollide => parameter?.canCollide ?? false;
    public float BaseScore => parameter?.baseScore ?? 0f;
    public float CollideScore => parameter?.collideScore ?? 0f;

    // 出現してから自動で表示になるまでの時間（寿命）
    private readonly float lifeTime = 10f;

    private void Start()
    {
        parameter = ParameterManager.Instance.parameter.target.poisonFly;
    }

    private void Update()
    {
        behaviour.Move(parameter.speed.x, parameter.speed.y);

        // 今後、OnChanged関数から呼ぶ
        ApplySize(parameter.size);
        ApplyAnimationSpeed(parameter.animationSpeed);
    }

    private void OnBecameInvisible()
    {
        DisableSelf();
    }

    private void OnEnable()
    {
        Init();

        // 一定時間後に非表示にする
        Invoke(nameof(DisableSelf), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void OnHitByTongue()
    {
        // カメレオンの舌で捕らえられたときの処理

        if (IsReward)
        {
            // コンボ数を増やし、ヒット判定の統計を更新
            if (ComboCounter.Instance) { ComboCounter.Instance.AddComboCount(); }
            if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnTongueHitTarget(); }

            AudioManager.Instance.PlaySE(SEKey.Hit);
        }
        else
        {
            // コンボ数を途切らせ、ミス判定の統計を更新
            if (ComboCounter.Instance) { ComboCounter.Instance.EndCombo(); }
            if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnTongueMissHit(); }

            AudioManager.Instance.PlaySE(SEKey.MissHit);
        }

        // 自身を非表示にする（プールに戻す）
        DisableSelf();
    }

    public void OnHitByChameleon()
    {
        // カメレオンの体に当たったときの処理
        DisableSelf();
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void Init()
    {
        if (animator == null) { animator = GetComponent<Animator>(); }

        if (behaviour == null) { behaviour = GetComponent<FlyBehaviour>(); }
        behaviour.Init(transform.position);
    }

    private void ApplySize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }

    private void ApplyAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }
}
