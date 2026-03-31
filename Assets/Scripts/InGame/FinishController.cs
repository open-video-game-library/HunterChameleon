using UnityEngine;

[RequireComponent(typeof(TextUpdater))]
public class FinishController : MonoBehaviour
{
    // Finish演出が終了したか
    [HideInInspector]
    public bool isFinishDone;

    private TextUpdater textUpdater;

    private readonly float finishSecond = 2.0f;
    private float time = 0f;

    private void Start()
    {
        textUpdater = GetComponent<TextUpdater>();
        Init();
    }

    private void Init()
    {
        textUpdater.UpdateText("");
        isFinishDone = false;
        time = 0f;
    }

    public void PlayFinishSE()
    {
        AudioManager.Instance.PlaySE(SEKey.Finish);
    }

    public void TickFinish()
    {
        if (isFinishDone) { return; }

        time += Time.deltaTime;
        textUpdater.UpdateText("Finish!");

        if (time > finishSecond) { isFinishDone = true; }
    }
}
