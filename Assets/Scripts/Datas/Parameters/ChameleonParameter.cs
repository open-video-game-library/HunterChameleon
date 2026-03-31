using UnityEngine;

[System.Serializable]
public class ChameleonParameter
{
    public Color32 tongueColor = new Color32(
        (byte)243,
        (byte)132,
        (byte)229,
        (byte)255
    );

    [Header("Tongue Hit Radius")]
    public float hitRadiusMultiplier = 1.50f; // 見た目を基準とした当たり判定の大きさの倍率

    [Header("Tongue Sizes")]
    public float rootWidth = 0.60f;  // 根元の幅（ワールド単位）
    public float tipWidth = 0.12f;  // 先端側ボディ幅

    [Header("Tongue Tip Scale")]
    public float tipNearScale = 0.15f; // 手前の大きさ
    public float tipFarScale = 0.08f; // 奥の小ささ

    [Header("Tongue Animation")]
    public float extendTime = 0f; // 舌を伸ばしきるのにかかる時間
    public float retractTime = 0.30f; // 舌を戻しきるのにかかる時間（クールタイム）
}