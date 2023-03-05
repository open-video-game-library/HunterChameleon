using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadButton : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    public void OnClicked ()
    {
        dataManager.getData();
    }
}
