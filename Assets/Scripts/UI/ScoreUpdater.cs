using UnityEngine;

[RequireComponent (typeof(TextUpdater))]
public class ScoreUpdater : MonoBehaviour
{
    private float currentScoreCount;
    private float previousScoreCount;

    private TextUpdater textUpdater;

    private Color32 flashColor = new Color32(255, 0, 0, 255);
    private readonly float flashDuration = 0.50f;

    private void Start()
    {
        textUpdater = GetComponent<TextUpdater>();
        InitScoreText();
    }

    private void LateUpdate()
    {
        if (!GameStatsUpdater.Instance) { return; }

        currentScoreCount = GameStatsUpdater.Instance.Score;

        if (currentScoreCount > previousScoreCount) { AddScoreText(currentScoreCount); }
        else if (currentScoreCount < previousScoreCount) { SubstractScoreText(currentScoreCount); }

        previousScoreCount = currentScoreCount;
    }

    private void AddScoreText(float newScore)
    {
        textUpdater.UpdateText("SCORE: " + newScore);
    }

    private void SubstractScoreText(float newScore)
    {
        textUpdater.UpdateText("SCORE: " + newScore);
        textUpdater.FlashColor(flashColor, flashDuration);
    }

    private void InitScoreText()
    {
        currentScoreCount = 0f;
        previousScoreCount = 0f;

        if (textUpdater != null && GameStatsUpdater.Instance)
        {
            // 現時点のスコアを参照し、反映する
            textUpdater.UpdateText("SCORE: " + GameStatsUpdater.Instance.Score);
        }
    }
}
