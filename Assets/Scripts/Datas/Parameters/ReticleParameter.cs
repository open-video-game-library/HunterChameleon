using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReticleParameter
{
    public List<ReticleColor32> reticleColors = new List<ReticleColor32>
    {
        new ReticleColor32(new Color32(243, 132, 229, 255)),
        new ReticleColor32(new Color32(243, 132, 229, 255))
    };
}