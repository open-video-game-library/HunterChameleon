public interface ITarget
{
    public bool IsReward { get; }
    public bool CanCollide { get; }
    public float BaseScore { get; }
    public float CollideScore { get; }
    public void OnHitByTongue(); // 舌が当たったときの処理
    public void OnHitByChameleon(); // カメレオン本体に当たったときの処理
    public void DisableSelf();
}