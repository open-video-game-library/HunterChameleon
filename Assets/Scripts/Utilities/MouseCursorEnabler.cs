using UnityEngine;

public static class MouseCursorEnabler
{
    public static bool GetCursorActive()
    {
        return Cursor.visible && Cursor.lockState == CursorLockMode.None;
    }

    public static void SetCursorActive(bool enable)
    {
        Cursor.visible = enable;

        if (enable) { Cursor.lockState = CursorLockMode.None; }
        else { Cursor.lockState = CursorLockMode.Locked; }
    }
}
