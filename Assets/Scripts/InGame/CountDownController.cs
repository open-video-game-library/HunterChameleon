using UnityEngine;

[RequireComponent(typeof(TextUpdater))]
public class CountDownController : MonoBehaviour
{
    // カウントダウンが終了したか
    [HideInInspector]
    public bool isCountDownDone;

    private TextUpdater textUpdater;

    private enum CountDownPhase { Before, CountDown, After }

    // カウントダウンが始まる前の予備の時間
    private readonly float secondBeforeCountDown = 1.0f;
    // カウントダウン本体の時間
    private readonly float countSecond = 3.0f;
    // カウントダウンが終わってからゲームプレイに移るまでの予備の時間
    private readonly float secondAfterCountDown = 1.0f;

    // 何秒からカウントダウンするか
    private readonly float startCount = 3.0f;

    private CountDownPhase phase = CountDownPhase.Before;

    // 現在のフェーズ内の経過時間
    private float phaseTime = 0f;
    // 1カウントあたりの表示時間(= countSecond / startCount)
    private float stepDuration = 1.0f;

    // 直前に表示したカウント（変化検知用）
    private int prevDisplayedCount = -1;

    private void Start()
    {
        textUpdater = GetComponent<TextUpdater>();
        Init();
    }

    private void Init()
    {
        textUpdater.UpdateText("");
        isCountDownDone = false;
        phase = CountDownPhase.Before;
        phaseTime = 0f;
        stepDuration = Mathf.Max(0.0001f, countSecond / Mathf.Max(1.0f, startCount));
        prevDisplayedCount = -1;
    }

    public void TickCountDown()
    {
        if (isCountDownDone) { return; }

        switch (phase)
        {
            case CountDownPhase.Before:
                phaseTime += Time.deltaTime;
                textUpdater.UpdateText(""); // 何も出さない
                if (phaseTime >= secondBeforeCountDown)
                {
                    phase = CountDownPhase.CountDown;
                    phaseTime = 0f;
                    prevDisplayedCount = -1;
                }
                break;

            case CountDownPhase.CountDown:
                phaseTime += Time.deltaTime;

                float remaining = countSecond - phaseTime;
                if (remaining > 0f)
                {
                    // 残り時間から現在表示すべき数値を求める
                    int n = Mathf.CeilToInt(remaining / stepDuration); // 3→2→1
                    n = Mathf.Clamp(n, 1, Mathf.CeilToInt(startCount));

                    // ▼ 数字が変わったフレームでだけSEを鳴らす
                    if (n != prevDisplayedCount)
                    {
                        AudioManager.Instance.PlaySE(SEKey.CountDown);
                        prevDisplayedCount = n;
                    }

                    textUpdater.UpdateText(n.ToString());
                }
                else
                {
                    // カウント終了 → Start! フェーズへ
                    phase = CountDownPhase.After;
                    phaseTime = 0f;
                    textUpdater.UpdateText("Start!");

                    AudioManager.Instance.PlaySE(SEKey.Start);
                }
                break;

            case CountDownPhase.After:
                phaseTime += Time.deltaTime;
                // Start! を表示し続ける
                if (phaseTime >= secondAfterCountDown)
                {
                    textUpdater.UpdateText("");
                    isCountDownDone = true;
                }
                break;
        }
    }
}
