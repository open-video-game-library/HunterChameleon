using UnityEngine;

public interface ITongueHitProcessor
{
    /// <summary>舌先のワールド位置pと半径rで処理を試み、何か処理できたらtrue。</summary>
    public bool TryProcess(Vector2 p, float r);
}
