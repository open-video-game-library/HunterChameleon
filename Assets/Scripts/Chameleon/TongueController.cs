using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TongueController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform rootTransform; // カメレオンの口（始点）
    [SerializeField] private SpriteRenderer tipSpriteRenderer; // 円スプライト（舌先）
    [SerializeField] private TongueTipController tipController; // 舌先を制御するスクリプト

    [Header("Order in Layer")]
    [SerializeField] private int layer;

    private Mesh mesh;
    private Vector3[] verts = new Vector3[4];
    private readonly int[] tris = { 0, 1, 2, 2, 1, 3 };
    private readonly Vector2[] uvs = {
        new Vector2(0,0), 
        new Vector2(1,0),
        new Vector2(0,1), 
        new Vector2(1,1)
    };

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // 0=根元, 1=目標まで
    private float progress = 0f;

    // 伸ばす先（ターゲット）
    private Vector2 target;

    // 舌が伸びている状態かどうか
    private bool isShooting;

    private ChameleonParameter parameter;
    private InputData input;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh == null) { meshFilter.sharedMesh = new Mesh(); }
        mesh = meshFilter.sharedMesh;

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial == null) { meshRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default")); }

        // Order in Layer の値を設定
        meshRenderer.sortingOrder = layer;
    }

    private void Start()
    {
        // パラメータをセット
        parameter = ParameterManager.Instance.parameter.chameleon;

        // 参照する入力データをセット
        input = InputDataManager.Instance.inputData;
    }

    private void Update()
    {
        if (input.submit) { StartCoroutine(Shoot(input.position)); }

        // 今後、OnChanged関数から呼ぶ
        ApplyColor();
    }

    public void SetProgress(float t)
    {
        progress = Mathf.Clamp01(t);
        if (!rootTransform || mesh == null) return;

        Vector2 r = rootTransform.position;
        Vector2 to = target;

        Vector2 d = to - r;
        float dist = d.magnitude;
        Vector2 dir = (dist > 1e-6f) ? d / dist : Vector3.right;

        float curLen = dist * progress;
        Vector2 tipPos = r + dir * curLen;

        // 舌の太さ方向（法線）
        Vector2 perp = new Vector2(-dir.y, dir.x).normalized;

        float halfRoot = parameter.rootWidth * 0.50f;
        float halfTip = Mathf.Lerp(parameter.rootWidth, parameter.tipWidth, progress) * 0.50f;

        // 台形
        verts[0] = r - perp * halfRoot; // root L
        verts[1] = r + perp * halfRoot; // root R
        verts[2] = tipPos - perp * halfTip; // tip L
        verts[3] = tipPos + perp * halfTip; // tip R

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateBounds();

        if (tipSpriteRenderer)
        {
            tipSpriteRenderer.transform.position = tipPos;
            float s = Mathf.Lerp(parameter.tipNearScale, parameter.tipFarScale, progress);
            tipSpriteRenderer.transform.localScale = new Vector2(s, s);

            // ボディより手前に
            var mr = GetComponent<MeshRenderer>();
            tipSpriteRenderer.sortingLayerID = mr.sortingLayerID;
            tipSpriteRenderer.sortingOrder = mr.sortingOrder + 1;
        }
    }

    private IEnumerator Shoot(Vector2 tgt)
    {
        if (isShooting) { yield break; }

        // 下を伸ばすSEを再生する
        AudioManager.Instance.PlaySE(SEKey.Shoot);

        // 舌を伸ばした回数を記録する
        if (GameStatsUpdater.Instance) { GameStatsUpdater.Instance.OnTongueShot(); }

        // 舌が伸びている状態にする
        isShooting = true;

        // 目標をセット
        target = tgt;

        // 伸び縮みの時間管理用の変数
        float t;

        // --- 伸ばす（EaseOut） ---
        t = 0f; 
        while (t < 1.0f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, parameter.extendTime);
            float eased = EaseOutQuad(t); // 速→遅
            SetProgress(Mathf.Lerp(0f, 1.0f, eased)); // 0→1
            yield return null;
        }

        // 念のため到達位置にピタッ
        SetProgress(1.0f);

        // 舌先にターゲットとの接触判定を依頼する
        if (tipController != null) { tipController.CheckHits(); }

        // --- すぐ戻す（EaseIn） ---
        t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, parameter.retractTime);
            float eased = EaseInQuad(t); // 遅→速
            SetProgress(Mathf.Lerp(1.0f, 0f, eased)); // 1→0
            yield return null;
        }

        // 完全収納
        SetProgress(0f);

        // 舌が伸びていない状態にする
        isShooting = false;
    }

    private void ApplyColor()
    {
        // 舌本体
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.color = parameter.tongueColor;
        }

        // 舌先
        if (tipSpriteRenderer != null)
        {
            tipSpriteRenderer.color = parameter.tongueColor;
        }
    }

    private float EaseOutQuad(float x)
    {
        // 速→遅
        return 1.0f - (1.0f - x) * (1.0f - x); 
    }

    private float EaseInQuad(float x)
    {
        // 遅→速
        return x * x;
    }
}
