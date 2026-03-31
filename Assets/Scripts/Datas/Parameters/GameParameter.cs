[System.Serializable]
public class GameParameter
{
    // カメレオン関連のパラメータ
    public ChameleonParameter chameleon;

    // レティクル関連のパラメータ
    public ReticleParameter reticle;

    // ゲームシーンのパラメータ
    public InGameParameter inGame;

    // ターゲット関連のパラメータ
    public TargetsParameter target;

    // スコアポップアップ関連のパラメータ
    public ScorePopupParameter scorePopup;

    // コンボ関連のパラメータ
    public ComboParameter combo;

    // スコア評価関連のパラメータ
    public ScoreRankParameters rank;

    // 入力関連のパラメータ
    public InputParameter input;

    // シード値のパラメータ
    public SeedParameter seed;

    // 音声関連のパラメータ
    public AudioParameter audio;
}