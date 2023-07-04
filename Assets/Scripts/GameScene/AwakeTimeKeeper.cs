using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AwakeTimeKeeper : MonoBehaviour
{
    [SerializeField] TimeKeeper tk;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField]
    private GameObject buttonAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart()
    {
        text.text = "";
        tk.GameStart();
        Destroy(this.gameObject);
        buttonAudio.SetActive(true);
    }
}
