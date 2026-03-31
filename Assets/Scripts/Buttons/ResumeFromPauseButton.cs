using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResumeFromPauseButton : MonoBehaviour, IButton
{
    private IStateController stateController;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Execute());
    }

    private void Start()
    {
        stateController = InterfaceFinder.FindObjectOfInterface<IStateController>();
    }

    public void Execute()
    {
        stateController.ResumeFromPauseState();
    }
}
