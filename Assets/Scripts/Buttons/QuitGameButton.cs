using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuitGameAction : MonoBehaviour, IButton
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Execute());
    }

    public void Execute()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
