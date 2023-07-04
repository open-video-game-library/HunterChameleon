using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultScoreText;
    [SerializeField]
    private TextMeshProUGUI resultHitNumText;
    [SerializeField]
    private TextMeshProUGUI resultHitRateText;

    void OnEnable ()
    {
        //Cursor.visible = true;
    }

    public void SetResult (float scoreNum, float hitNum, float triggerNum)
    {
        float hitRatio = (float)hitNum / (float)triggerNum;

        resultScoreText.text = "SCORE: " + scoreNum;
        resultHitNumText.text = "NUMBER OF HITS: " + hitNum;
        resultHitRateText.text = "HIT RATIO: " + (hitRatio  * 100) + "%";
    }
}
