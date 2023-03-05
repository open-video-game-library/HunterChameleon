using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeKeeper : MonoBehaviour
{
    public static bool isPlaying;

    private float playTime;

    [SerializeField]
    private Sun sun;

    [SerializeField]
    private TargetManager targetManager;

    [SerializeField]
    private GameObject countDown;
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private GameObject finish;

    [SerializeField]
    private GameObject bgm;

    // データ管理
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private Score score;
    [SerializeField]
    private Reticle reticle;
    [SerializeField]
    private Tongue tongue;

    void Start()
    {
        isPlaying = false;
        playTime = ParameterManager.playTime;
        countDownText = countDown.GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartCountDown());
    }

    private IEnumerator StartCountDown ()
    {
        yield return new WaitForSeconds(1);
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        isPlaying = true;
        countDownText.text = "START!";
        targetManager.StartSpawn();
        sun.StartSunMove();
        yield return new WaitForSeconds(1);
        StartCoroutine(KeepTime());
        bgm.SetActive(true);
        countDown.SetActive(false);
        yield break;
    }

    private IEnumerator KeepTime()
    {
        yield return new WaitForSeconds(playTime);
        finish.SetActive(true);
        bgm.SetActive(false);
        Finish();
        yield return new WaitForSeconds(2);
        bgm.SetActive(true);
        yield break;
    }

    private void Finish()
    {
        isPlaying = false;
        targetManager.StopSpawn();
        dataManager.postData(score.scoreNum, tongue.hitNum, reticle.triggerNum);
    }
}
