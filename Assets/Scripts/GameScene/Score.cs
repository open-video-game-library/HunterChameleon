using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [System.NonSerialized]
    public int scoreNum;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreNum = 0;
    }

    void Update()
    {
        
    }

    public void AddScore(int addedScore)
    {
        scoreNum += addedScore;
        scoreText.text = "SCORE: " + scoreNum.ToString();
    }

    public IEnumerator makeTextDamaged()
    {
        scoreText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        scoreText.color = Color.white;
        yield break;
    }
}
