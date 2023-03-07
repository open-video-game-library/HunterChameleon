using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DownloadButton : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    void Awake ()
    {
#if !UNITY_WEBGL
        this.gameObject.SetActive(false);
#endif
    }

    public void OnClicked ()
    {
        dataManager.getData();
    }
}
