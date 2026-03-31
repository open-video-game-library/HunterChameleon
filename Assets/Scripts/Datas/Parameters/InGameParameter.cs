using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameParameter
{
    public int playTime = 30;
    public int alartRemainingTime = 10;

    public List<Color32> sunColors = new List<Color32>
    {
        new Color32(10, 255, 255, 10),
        new Color32(255, 255, 128, 20),
        new Color32(255, 128, 128, 30),
        new Color32(0, 0, 0, 200)
    };
}