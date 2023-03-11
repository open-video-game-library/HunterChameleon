using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ExitButton : MonoBehaviour
{
    private void Start ()
    {
        #if UNITY_WEBGL
            this.gameObject.SetActive(false);
        #endif
    }

    public void OnClicked ()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif !UNITY_WEBGL
            Application.Quit();
        #endif
    }
}
