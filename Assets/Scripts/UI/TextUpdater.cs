using UnityEngine;
using System.Collections;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    [SerializeField]
    private TMP_Text targetText;

    private Color32 baseColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        baseColor = targetText.color;
    }

    public void UpdateText(string text)
    {
        if (targetText != null) { targetText.text = text; }
    }

    public void ChangeTextColor(Color32 color)
    {
        if (targetText != null)
        {
            targetText.color = color;
            baseColor = color;
        }
    }

    public void FlashColor(Color32 flashColor, float duration)
    {
        if (flashCoroutine != null) { StopCoroutine(flashCoroutine); }
        flashCoroutine = StartCoroutine(FlashCoroutine(flashColor, duration));
    }

    private IEnumerator FlashCoroutine(Color32 flashColor, float duration)
    {
        targetText.color = flashColor;

        yield return new WaitForSeconds(duration);

        targetText.color = baseColor;
        flashCoroutine = null;
    }
}
