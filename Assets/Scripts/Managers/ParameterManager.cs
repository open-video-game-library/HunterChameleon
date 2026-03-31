using System.IO;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{
    public static ParameterManager Instance;

    public GameParameter parameter;

    [SerializeField, Range(30, 60)]
    private int frameRate = 60;

    private string defaultPath;
    private string writePath;

    private void Awake()
    {
        if (Instance == null)
        {
            defaultPath = Path.Combine(Application.streamingAssetsPath, "GameParameter.json");
            writePath = Path.Combine(Application.persistentDataPath, "GameParameter.json");

            SaveParameter();
            LoadParameter();

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // フレームレートを設定
        Application.targetFrameRate = frameRate;
    }

    private void LoadParameter()
    {
        // 既存ファイルがあればwritePathを使い、なければdefaultPathを使う
        string path = File.Exists(writePath) ? writePath : defaultPath;

        // JSONを読み込み、デシリアライズする
        string json = File.ReadAllText(path);
        parameter = JsonUtility.FromJson<GameParameter>(json);
    }

    public void SaveParameter()
    {
        string json = JsonUtility.ToJson(parameter, true);
        File.WriteAllText(writePath, json);
    }
}

