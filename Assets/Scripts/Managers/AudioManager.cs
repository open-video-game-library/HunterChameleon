using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMKey
{
    None,
    Title,
    InGame,
    Result,
    Setting,
    Alart,
    // 追加していく
}

public enum SEKey
{
    None,
    Button,
    CountDown,
    Start,
    Shoot,
    Hit,
    MissHit,
    ComboReward,
    ComboRewardMax,
    ComboEnd,
    Finish,
    // 追加していく
}

public enum JingleKey
{
    None,
    Alart,
    // 追加していく
}

[System.Serializable]
public class BGMEntry
{
    public BGMKey key;
    public AudioClip clip;
}

[System.Serializable]
public class SEEntry
{
    public SEKey key;
    public AudioClip[] clips;
}

[System.Serializable]
public class JingleEntry
{
    public JingleKey key;
    public AudioClip clip;
}

/// <summary>
/// プロジェクトに1つだけ配置して、BGM/SEを一元管理するマネージャ。
/// - BGMは1チャンネル（ループ）
/// - SEはAudioSourceプールで同時多重再生に対応
/// - 音量は ParameterManager.Instance.parameter.audio の Master/BGM/SE を参照
/// - フェード対応
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("=== Audio Entries ===")]
    [SerializeField] private List<BGMEntry> bgmEntries = new List<BGMEntry>();
    [SerializeField] private List<SEEntry> seEntries = new List<SEEntry>();
    [SerializeField] private List<JingleEntry> jingleEntries = new List<JingleEntry>();

    [Header("“置き換え再生”にする対象のSEKey")]
    [SerializeField] private SEKey[] replaceIfSameKey = new SEKey[0]; // 空なら全て従来どおり

    // BGM・SE・JingleのキーとClipの辞書を管理する変数
    private Dictionary<BGMKey, AudioClip> bgmMap;
    private Dictionary<SEKey, List<AudioClip>> seMap;
    private Dictionary<JingleKey, AudioClip> jingleMap;

    // BGM・SEのAudioSourceを管理する変数
    private AudioSource bgmSource; // BGM 専用
    private AudioSource seSource;  // SE 専用
    private AudioSource jingleSource; // Jingle 専用

    // BGM用の基準ピッチ（1.0=等速）
    private float bgmPitch = 1.0f;

    private Coroutine bgmFadeCoroutine;
    private Coroutine jingleCoroutine;

    // 重ねて鳴らさないSEの処理に用いる変数
    private HashSet<SEKey> monophonicKeys; // 置き換え対象キー集合
    private Dictionary<SEKey, AudioSource> seKeySourceMap = new(); // 置き換え対象キー専用のAudioSource

    // 今流しているBGMキーを保持（ジングル後に復帰するため）
    private BGMKey currentBGMKey = BGMKey.None;    

    // ParameterManager から取得する AudioParameter
    private AudioParameter parameter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // クリップ参照用の辞書を構築（O(1)で取り出せるようにする）
        BuildDictionaries();

        // BGM用 AudioSource
        bgmSource = gameObject.GetComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        // SE用 AudioSource
        seSource = gameObject.AddComponent<AudioSource>();
        seSource.playOnAwake = false;
        seSource.loop = false;

        // SEの置き換え対象キー集合を用意
        monophonicKeys = new HashSet<SEKey>(replaceIfSameKey ?? new SEKey[0]);

        // ジングル用 AudioSource（BGM系扱いにするので音量はBGMと同じ系を適用）
        jingleSource = gameObject.AddComponent<AudioSource>();
        jingleSource.playOnAwake = false;
        jingleSource.loop = false;
    }

    private void Start()
    {
        // ParameterManager から AudioParameter を取得（未初期化時のフォールバック付き）
        if (ParameterManager.Instance != null && ParameterManager.Instance.parameter != null)
        {
            parameter = ParameterManager.Instance.parameter.audio;
            if (parameter == null)
            {
                parameter = new AudioParameter(); // null 安全
                ParameterManager.Instance.parameter.audio = parameter;
            }
        }
        else
        {
            Debug.LogWarning("[AudioManager] ParameterManager が未初期化です。暫定の AudioParameter を用います。");
            parameter = new AudioParameter();
        }

        // Parameterの値を全AudioSourceに反映
        ApplyAllVolumes();
    }

    private void Update()
    {
        // // 今後、OnChanged関数から呼ぶ
        ApplyAllVolumes();
    }

    private void BuildDictionaries()
    {
        bgmMap = new Dictionary<BGMKey, AudioClip>();
        foreach (var entry in bgmEntries)
        {
            if (entry != null && entry.clip != null && !bgmMap.ContainsKey(entry.key))
            {
                bgmMap.Add(entry.key, entry.clip);
            }
        }

        // 複数クリップをキーごとにまとめる
        seMap = new Dictionary<SEKey, List<AudioClip>>();
        foreach (var entry in seEntries)
        {
            if (entry == null) continue;

            var list = new List<AudioClip>();
            if (entry.clips != null)
            {
                foreach (var c in entry.clips)
                {
                    if (c != null) list.Add(c);
                }
            }
            if (list.Count == 0) continue;

            if (!seMap.ContainsKey(entry.key))
            {
                seMap.Add(entry.key, list);
            }
            else
            {
                seMap[entry.key].AddRange(list);
            }
        }

        jingleMap = new Dictionary<JingleKey, AudioClip>();
        foreach (var entry in jingleEntries)
        {
            if (entry != null && entry.clip != null && !jingleMap.ContainsKey(entry.key))
            {
                jingleMap.Add(entry.key, entry.clip);
            }
        }
    }

    #region Public API（BGM）

    /// <summary>
    /// 指定キーのBGMに切り替えて再生。duration秒でフェードイン。
    /// すでに別BGMが鳴っていればフェードアウト→差し替え→フェードイン。
    /// </summary>
    public void PlayBGM(BGMKey key, float fadeDuration = 0.0f)
    {
        if (!bgmMap.TryGetValue(key, out var clip) || clip == null)
        {
            Debug.LogWarning($"[AudioManager] BGM not found: {key}");
            return;
        }

        // 現在のBGMキーを記憶（ジングル後に使う）
        currentBGMKey = key;

        // すでに同じクリップなら何もしない
        if (bgmSource.clip == clip && bgmSource.isPlaying) { return; }

        // 現在のフェードを止める
        if (bgmFadeCoroutine != null) { StopCoroutine(bgmFadeCoroutine); }

        // フェードアウト→差し替え→フェードイン
        bgmFadeCoroutine = StartCoroutine(Co_SwitchBGM(clip, fadeDuration));
    }

    /// <summary>
    /// BGM停止（フェードアウトあり）
    /// </summary>
    public void StopBGM(float fadeDuration = 0.0f)
    {
        if (bgmFadeCoroutine != null) { StopCoroutine(bgmFadeCoroutine); }
        bgmFadeCoroutine = StartCoroutine(Co_StopBGM(fadeDuration));
    }

    /// <summary>
    /// BGMピッチを設定（0.5〜2.0）。即時反映＆以後のBGM切替でも維持。
    /// </summary>
    public void SetBGMPitch(float pitch)
    {
        bgmPitch = Mathf.Clamp(pitch, 0.5f, 2f);
        if (bgmSource != null) { bgmSource.pitch = bgmPitch; }
    }

    /// <summary>
    /// 現在のBGMピッチを取得。
    /// </summary>
    public float GetBGMPitch() => bgmPitch;

    /// <summary>
    /// BGMピッチを1.0に戻す（等速）。
    /// </summary>
    public void ResetBGMPitch()
    {
        bgmPitch = 1.0f;
        if (bgmSource != null) { bgmSource.pitch = bgmPitch; }
    }

    #endregion

    #region Public API（SE）

    /// <summary>
    /// SEを1発鳴らす。
    /// - basePitch: 基準となるピッチ（1.0で等倍速）。指定可能。
    /// - pitchVariance: ±のランダム揺らし幅。0なら揺らしなし。
    ///   例）basePitch=1.0, pitchVariance=0.05 → 0.95〜1.05の範囲でランダム
    ///   クリップはエントリー内の先頭（index=0）を使用。
    /// </summary>
    public void PlaySE(SEKey key, float basePitch = 1.0f, float pitchVariance = 0f)
    {
        if (!seMap.TryGetValue(key, out var clips) || clips == null || clips.Count == 0)
        {
            Debug.LogWarning($"[AudioManager] SE not found: {key}");
            return;
        }

        var clip = clips[0];
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManager] SE clip is null: {key} index 0");
            return;
        }

        float variance = (pitchVariance > 0f) ? pitchVariance : Mathf.Max(0f, parameter.defaultPitchVariance);
        float newPitch = basePitch;
        if (variance > 0f)
        {
            float delta = Random.Range(-variance, variance);
            newPitch = Mathf.Clamp(basePitch + delta, 0.5f, 2f);
        }

        // 置き換え対象なら“そのキー専用ソース”で差し替え再生
        if (monophonicKeys != null && monophonicKeys.Contains(key))
        {
            var src = GetOrCreateSESource(key);
            float originalPitch = src.pitch;

            // 既存を止めて差し替え（※フェードしたければここをコルーチンに）
            if (src.isPlaying) { src.Stop(); }

            src.pitch = newPitch;
            src.volume = GetAppliedSEVolume();
            src.clip = clip;
            src.Play();

            StartCoroutine(Co_ResetPitchAfter(src, originalPitch, clip.length));
        }
        else
        {
            float originalPitch = seSource.pitch;
            seSource.pitch = newPitch;
            seSource.volume = GetAppliedSEVolume();
            seSource.PlayOneShot(clip);
            StartCoroutine(Co_ResetPitchAfter(seSource, originalPitch, clip.length));
        }
    }


    /// <summary>
    /// SEを1発鳴らす。
    /// - clipIndex: エントリー内のどのクリップを鳴らすか指定可能。
    /// - basePitch: 基準となるピッチ（1.0で等倍速）。指定可能。
    /// - pitchVariance: ±のランダム揺らし幅。0なら揺らしなし。
    /// </summary>
    public void PlaySE(SEKey key, int clipIndex, float basePitch = 1.0f, float pitchVariance = 0f)
    {
        if (!seMap.TryGetValue(key, out var clips) || clips == null || clips.Count == 0)
        {
            Debug.LogWarning($"[AudioManager] SE not found: {key}");
            return;
        }

        if (clipIndex < 0) { clipIndex = 0; }
        else if (clipIndex >= clips.Count) { clipIndex = clips.Count - 1; }

        var clip = clips[clipIndex];
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManager] SE clip is null: {key} index {clipIndex}");
            return;
        }

        float variance = (pitchVariance > 0f) ? pitchVariance : Mathf.Max(0f, parameter.defaultPitchVariance);
        float newPitch = basePitch;
        if (variance > 0f)
        {
            float delta = Random.Range(-variance, variance);
            newPitch = Mathf.Clamp(basePitch + delta, 0.5f, 2f);
        }

        // 置き換え対象ならキー専用ソースで差し替え
        if (monophonicKeys != null && monophonicKeys.Contains(key))
        {
            var src = GetOrCreateSESource(key);
            float originalPitch = src.pitch;

            // 既存を止めて差し替え（※フェードしたければここをコルーチンに）
            if (src.isPlaying) { src.Stop(); }

            src.pitch = newPitch;
            src.volume = GetAppliedSEVolume();
            src.clip = clip;
            src.Play();

            StartCoroutine(Co_ResetPitchAfter(src, originalPitch, clip.length));
        }
        else
        {
            float originalPitch = seSource.pitch;
            seSource.pitch = newPitch;
            seSource.volume = GetAppliedSEVolume();
            seSource.PlayOneShot(clip);
            StartCoroutine(Co_ResetPitchAfter(seSource, originalPitch, clip.length));
        }
    }

    /// <summary>特定キーを外部から明示停止</summary>
    public void StopSE(SEKey key)
    {
        if (seKeySourceMap != null && seKeySourceMap.TryGetValue(key, out var src) && src != null)
        {
            src.Stop();
        }
    }

    #endregion

    #region Public API（Jingle）

    /// <summary>
    /// BGM再生中にジングルを鳴らし、ジングル終了後に「同じBGMを頭から」再生し直す。
    /// - bgmFadeOut: ジングル直前のBGMフェードアウト時間
    /// - bgmFadeIn : ジングル後のBGMフェードイン時間
    /// - jingleVolume: ジングルの相対音量（BGM系の実効音量に対して）
    /// </summary>
    public void PlayBGMJingle(JingleKey key, float bgmFadeOut = 0.15f, float bgmFadeIn = 0.15f, float jingleVolume = 1f)
    {
        if (!jingleMap.TryGetValue(key, out var jingleClip) || jingleClip == null)
        {
            Debug.LogWarning($"[AudioManager] Jingle not found: {key}");
            return;
        }
        if (jingleCoroutine != null) { StopCoroutine(jingleCoroutine); }
        jingleCoroutine = StartCoroutine(Co_PlayBGMJingle(jingleClip, bgmFadeOut, bgmFadeIn, jingleVolume));
    }

    /// <summary>
    /// 現在のジングルを即停止（BGM復帰は行わない）
    /// </summary>
    public void StopJingle()
    {
        if (jingleCoroutine != null) { StopCoroutine(jingleCoroutine); jingleCoroutine = null; }
        if (jingleSource != null && jingleSource.isPlaying) { jingleSource.Stop(); }
    }

    #endregion

    #region Public API（音量）

    /// <summary>マスター音量（0〜1）。Parameter の値を更新して即時反映。</summary>
    public void SetVolumeMaster(float volume)
    {
        parameter.masterVolume = Mathf.Clamp01(volume);
        ApplyAllVolumes();
    }

    /// <summary>BGM音量（0〜1）。Parameter の値を更新して即時反映。</summary>
    public void SetVolumeBGM(float volume)
    {
        parameter.bgmVolume = Mathf.Clamp01(volume);
        ApplyAllVolumes();
    }

    /// <summary>SE音量（0〜1）。Parameter の値を更新して即時反映。</summary>
    public void SetVolumeSE(float volume)
    {
        parameter.seVolume = Mathf.Clamp01(volume);
        ApplyAllVolumes();
    }

    public float GetVolumeMaster()
    {
        return parameter.masterVolume;
    }
    public float GetVolumeBGM()
    {
        return parameter.bgmVolume;
    }
    public float GetVolumeSE()
    {
        return parameter.seVolume;
    }

    /// <summary>
    /// ParameterManager 側の値が外部で変更された場合に同期したいときに呼ぶ。
    /// （例：UIが直接 Parameter を書き換えたあと）
    /// </summary>
    public void RefreshFromParameters()
    {
        if (ParameterManager.Instance != null && ParameterManager.Instance.parameter != null)
        {
            parameter = ParameterManager.Instance.parameter.audio ?? new AudioParameter();
            ParameterManager.Instance.parameter.audio = parameter;
        }
        ApplyAllVolumes();
    }

    #endregion

    #region 内部実装（フェード/音量）

    private IEnumerator Co_SwitchBGM(AudioClip newClip, float duration)
    {
        // 1) 既存をフェードアウト
        float t = 0f;
        float startVolume = bgmSource.volume;
        while (t < duration && bgmSource.isPlaying && bgmSource.clip != null)
        {
            t += Time.unscaledDeltaTime; // ポーズ中でも動くよう unscaled を採用
            float ratio = 1f - Mathf.Clamp01(t / duration);
            bgmSource.volume = startVolume * ratio;
            yield return null;
        }

        // 2) 差し替え
        bgmSource.clip = newClip;
        bgmSource.volume = 0f;
        bgmSource.pitch = bgmPitch;
        bgmSource.time = 0f;
        bgmSource.Play();

        // 3) フェードイン
        t = 0f;
        float targetVolume = GetAppliedBGMVolume();
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp01(t / duration);
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, ratio);
            yield return null;
        }
        bgmSource.volume = targetVolume;

        bgmFadeCoroutine = null;
    }

    private IEnumerator Co_StopBGM(float duration)
    {
        if (!bgmSource.isPlaying || bgmSource.clip == null)
        {
            bgmSource.Stop();
            bgmSource.clip = null;
            bgmFadeCoroutine = null;
            yield break;
        }

        float t = 0f;
        float startVolume = bgmSource.volume;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float ratio = 1f - Mathf.Clamp01(t / duration);
            bgmSource.volume = startVolume * ratio;
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.clip = null;
        bgmFadeCoroutine = null;
    }

    private IEnumerator Co_ResetPitchAfter(AudioSource audioSource, float original, float afterSeconds)
    {
        // 再生時間より少し余裕を持って戻す
        float t = 0f;
        while (t < afterSeconds + 0.05f)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        if (audioSource != null) { audioSource.pitch = original; }
    }

    private IEnumerator Co_PlayBGMJingle(AudioClip jingleClip, float fadeOut, float fadeIn, float jingleVol)
    {
        // 1) BGMをフェードアウトして止める（現在のキーは保持される）
        if (bgmFadeCoroutine != null) { StopCoroutine(bgmFadeCoroutine); }
        yield return Co_StopBGM(fadeOut);

        // 2) ジングルを再生（BGM系の音量を基準に）
        jingleSource.volume = Mathf.Clamp01(GetAppliedBGMVolume() * jingleVol);
        jingleSource.pitch = 1f; // ジングルは基準ピッチで
        jingleSource.clip = jingleClip;
        jingleSource.time = 0f;
        jingleSource.Play();

        // 3) 再生終了を待つ（わずかに余裕）
        float t = 0f, wait = jingleClip.length + 0.05f;
        while (t < wait)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // 4) 同じBGMキーを“頭から”再開（フェードイン）
        if (currentBGMKey != BGMKey.None)
        {
            PlayBGM(currentBGMKey, fadeIn); // PlayBGMは常に頭から再生
        }
        jingleCoroutine = null;
    }

    // 置き換え対象のSEKeyごとに1本だけ持つAudioSource
    private AudioSource GetOrCreateSESource(SEKey key)
    {
        if (!seKeySourceMap.TryGetValue(key, out var src) || src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.volume = GetAppliedSEVolume();
            seKeySourceMap[key] = src;
        }
        return src;
    }

    private void ApplyAllVolumes()
    {
        // 実効音量 = 個別音量 × マスター音量（Parameter由来）
        if (bgmSource != null) { bgmSource.volume = GetAppliedBGMVolume(); }
        if (seSource != null) { seSource.volume = GetAppliedSEVolume(); }
        if (seKeySourceMap != null)
        {
            foreach (var kv in seKeySourceMap)
            {
                var src = kv.Value;
                if (src != null) { src.volume = GetAppliedSEVolume(); }
            }
        }
        if (jingleSource != null) { jingleSource.volume = GetAppliedBGMVolume(); }
    }

    private float GetAppliedBGMVolume()
    {
        return Mathf.Clamp01(parameter.bgmVolume * parameter.masterVolume);
    }
    private float GetAppliedSEVolume()
    {
        return Mathf.Clamp01(parameter.seVolume * parameter.masterVolume);
    }

    #endregion
}
