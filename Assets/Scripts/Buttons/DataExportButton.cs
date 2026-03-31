using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DataExportButton : MonoBehaviour, IButton
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Execute());
    }

    public void Execute()
    {
        if (ParamLogger.Instance) { ParamLogger.Instance.Export(); }
    }
}
