using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private Button pauseButton;

    private bool isPausing;

    [SerializeField]
    private Sprite[] buttonSprites = new Sprite[2];

    [SerializeField]
    private GameObject parameterPanel;

    [SerializeField]
    private Score score;

    [SerializeField]
    private TargetManager targetManager;
    [SerializeField]
    private Tongue tongue;
    [SerializeField]
    private Reticle reticle;

    void Start ()
    {
        isPausing = true;
        pauseButton.onClick.AddListener(OnClicked);
        Cursor.visible = true;
    }

    void OnClicked () 
    {
        if(isPausing)
        {
            Cursor.visible = false;
            Init();
            TimeKeeper.isPlaying = true;
            isPausing = false;
            this.gameObject.GetComponent<Image>().sprite = buttonSprites[1];
            parameterPanel.SetActive(false);
            targetManager.StartSpawn();
        }
        else
        {
            Cursor.visible = true;
            TimeKeeper.isPlaying = false;
            isPausing = true;
            this.gameObject.GetComponent<Image>().sprite = buttonSprites[0];
            parameterPanel.SetActive(true);
            targetManager.StopSpawn();
        }
    }

    private void Init ()
    {
        score.Init();
        targetManager.Init();
        tongue.Init();
        reticle.Init();
    }
}
