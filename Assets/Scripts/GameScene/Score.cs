using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [System.NonSerialized]
    public int score;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = "SCORE: " + score.ToString();
    }

    public IEnumerator makeTextDamaged()
    {
        scoreText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        scoreText.color = Color.white;
        yield break;
    }
}
