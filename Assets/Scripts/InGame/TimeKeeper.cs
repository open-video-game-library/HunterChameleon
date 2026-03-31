using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    public static TimeKeeper Instance;

    private int playTime;
    private int alartRemainingTime;

    private float time;
    private float prevTime = -1.0f;
    private float normalizedTime;

    private InGameParameter parameter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // パラメータをセット
        parameter = ParameterManager.Instance.parameter.inGame;

        playTime = parameter.playTime;
        alartRemainingTime = Mathf.Min(parameter.alartRemainingTime, playTime);
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; }
    }

    public void TickTimer()
    {
        if (time >= playTime) { return; }
        prevTime = time;
        time += Time.deltaTime;
    }

    public float GetCurrentTime()
    {
        if (time >= playTime) { return playTime; }
        return time;
    }

    public float GetCurrentNormalizedTime()
    {
        if (playTime <= 0) { return 0f; }

        normalizedTime = time / playTime;

        if (normalizedTime > 1) { return 1.0f; }
        return normalizedTime;
    }

    public bool ConsumeAlart()
    {
        float remainingTime = playTime - time;
        float prevRemainingTime = playTime - prevTime;

        return prevRemainingTime >= alartRemainingTime && alartRemainingTime >= remainingTime;
    }

    public bool ConsumeTimeUp()
    {
        return time >= playTime && playTime >= prevTime;
    }
}
