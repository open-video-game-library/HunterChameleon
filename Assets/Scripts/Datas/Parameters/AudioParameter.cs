using UnityEngine;

[System.Serializable]
public class AudioParameter
{
    [Header("Volumes (0..1)")]
    [Range(0f, 1f)] public float masterVolume = 1.0f;
    [Range(0f, 1f)] public float bgmVolume = 0.8f;
    [Range(0f, 1f)] public float seVolume = 1.0f;

    [Header("SE Defaults")]
    [Tooltip("SE連打の耳疲れ対策: 0.05 で ±5% のピッチ揺らし")]
    [Range(0f, 0.5f)] public float defaultPitchVariance = 0f;
}
