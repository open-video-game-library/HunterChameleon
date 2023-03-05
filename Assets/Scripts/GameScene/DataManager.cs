using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DataManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void addData(string jsonData);

    [DllImport("__Internal")]
    private static extern void downloadData();
#endif

    [System.Serializable]
    public class Data
    {
        public int score;
        public int hitNum;
        public float hitRate;
    }

    public void postData(int score, int hitCount, int triggerCount)
    {
        Data data = new Data();
        data.score = score;
        data.hitNum = hitCount;
        data.hitRate = (float)hitCount / (float)triggerCount;
        Debug.Log("score: " + data.score + ", hitNum: " + data.hitNum + ", hitRate: " + data.hitRate);
        string json = JsonUtility.ToJson(data);
#if UNITY_WEBGL && !UNITY_EDITOR
        addData(json);
#endif
    }

    public void getData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        downloadData();
#endif
    }
}
