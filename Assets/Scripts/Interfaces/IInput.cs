using UnityEngine;

public interface IInput
{
    public void SetInputActive(bool active);
    public Vector2 GetPosition();
    public bool GetSubmit();
    public bool GetPauseSubmit();
}
