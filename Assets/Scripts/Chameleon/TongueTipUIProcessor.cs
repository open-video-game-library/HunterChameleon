using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TongueTipUIProcessor : MonoBehaviour, ITongueHitProcessor
{
    [SerializeField]
    private float pressCooldownSeconds = 0.12f;

    private Button lastPressedButton;
    private float lastPressedTime = -999f;

    public bool TryProcess(Vector2 tongueWorldPos, float r)
    {
        if (!EventSystem.current) { return false; }

        // ワールド→スクリーン座標
        var cam = Camera.main;
        Vector2 screenPos = cam ? (Vector2)cam.WorldToScreenPoint(tongueWorldPos) : (Vector2)tongueWorldPos;

        // UIヒット検索
        var ped = new PointerEventData(EventSystem.current) { position = screenPos };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        if (results.Count == 0) { return false; }

        // 最前面ヒットからButtonを探す
        foreach (var rr in results)
        {
            if (!rr.gameObject) { continue; }

            if (rr.gameObject.TryGetComponent<Button>(out var btn))
            {
                // 連打防止（処理は成功扱い）
                if (btn == lastPressedButton && Time.unscaledTime - lastPressedTime < pressCooldownSeconds) { return true; }

                if (!btn.interactable) { return false; }

                btn.onClick.Invoke();
                lastPressedButton = btn;
                lastPressedTime = Time.unscaledTime;
                return true;
            }
        }

        return false;
    }
}
