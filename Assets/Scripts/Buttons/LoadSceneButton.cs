using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneAction : MonoBehaviour, IButton
{
    [SerializeField]
    private string sceneName;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Execute());
    }

    public void Execute()
    {
        SceneLoadUtility.LoadScene(sceneName);
    }
}
