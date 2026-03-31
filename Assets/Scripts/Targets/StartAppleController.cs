using UnityEngine;

public class StartAppleController : MonoBehaviour
{
    // このボタンがクリックされたか
    [HideInInspector]
    public bool isHitByTongue;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isHitByTongue = false;
    }

    public void OnHitByTongue()
    {
        // カメレオンの舌で捉えられたときの処理
        AudioManager.Instance.PlaySE(SEKey.Button);
        isHitByTongue = true;
    }
}
