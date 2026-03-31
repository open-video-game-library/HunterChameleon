using System;
using UnityEngine;

public static class Calculate
{
    // 基準ベクトルを軸として、元ベクトルから指定ベクトルへの回転角（度）を計算
    public static float GetAngle(Vector2 baseVector, Vector2 from, Vector2 to)
    {
        Vector2 vector = to - from;
        float angle = Vector2.SignedAngle(baseVector, vector);
        return angle;
    }

    // 入力されたpositionを、スクリーンからはみ出ないように補正
    public static Vector2 ClampPositionInScreen(Vector2 inputPosition)
    {
        float clampedPosX = Mathf.Clamp(inputPosition.x, 0f, Camera.main.scaledPixelWidth);
        float clampedPosY = Mathf.Clamp(inputPosition.y, 0f, Camera.main.scaledPixelHeight);
        Vector2 clampedPos = new Vector2(clampedPosX, clampedPosY);

        return clampedPos;
    }

    // 入力されたpositionを、スクリーンからはみ出ないように補正+ワールド座標系に変換
    public static Vector2 ClampScreenToWorldPoint(Vector2 inputPosition)
    {
        float clampedPosX = Mathf.Clamp(inputPosition.x, 0f, Camera.main.scaledPixelWidth);
        float clampedPosY = Mathf.Clamp(inputPosition.y, 0f, Camera.main.scaledPixelHeight);
        Vector2 clampedPos = new Vector2(clampedPosX, clampedPosY);

        return Camera.main.ScreenToWorldPoint(clampedPos);
    }

    // スクリーン右端のX座標と、スクリーン上端のY座標を返す
    public static Vector2 GetScreenEdgePositive()
    {
        float right = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.50f, 0f)).x; // ゲーム画面の右端中央のワールド座標
        float up = Camera.main.ViewportToWorldPoint(new Vector3(0.50f, 1.0f, 0f)).y; // ゲーム画面の上端中央のワールド座標

        return new Vector2(right, up);
    }

    // スクリーン左端のX座標と、スクリーン下端のY座標を返す
    public static Vector2 GetScreenEdgeNegative()
    {
        float left = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.50f, 0f)).x; // ゲーム画面の左端中央のワールド座標
        float down = Camera.main.ViewportToWorldPoint(new Vector3(0.50f, 0f, 0f)).y; // ゲーム画面の下端中央のワールド座標

        return new Vector2(left, down);
    }

    // 指定した列挙型の中からランダムな要素を抽選する
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        int randomIndex = UnityEngine.Random.Range(0, values.Length);
        return values[randomIndex];
    }
}
