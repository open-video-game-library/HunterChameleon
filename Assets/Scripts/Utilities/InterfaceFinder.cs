using System.Collections.Generic;
using UnityEngine;

public static class InterfaceFinder
{
    /// <summary>
    /// 特定のインタフェースがアタッチされた全てのオブジェクトを見つける
    /// </summary>
    /// <typeparam name="T"> 探索するインタフェース </typeparam>
    /// <returns> 取得したクラス配列 </returns>
    public static T[] FindObjectsOfInterface<T>() where T : class
    {
        List<T> findList = new List<T>();

        // オブジェクトを探索し、リストに格納
        foreach (var component in Object.FindObjectsOfType<Component>())
        {
            var obj = component as T;

            if (obj == null) { continue; }

            findList.Add(obj);
        }

        T[] findObjArray = new T[findList.Count];
        int count = 0;

        // 取得したオブジェクトを指定したインタフェース型配列に格納
        foreach (T obj in findList)
        {
            findObjArray[count] = obj;
            count++;
        }
        return findObjArray;
    }

    /// <summary>
    /// 特定のインタフェースがアタッチされた最初の1つのオブジェクトを見つける
    /// </summary>
    /// <typeparam name="T"> 探索するインタフェース </typeparam>
    /// <returns> 取得したクラス配列 </returns>
    public static T FindObjectOfInterface<T>(bool includeInactive = false) where T : class
    {
        var behaviours = Object.FindObjectsOfType<MonoBehaviour>(includeInactive);

        foreach (var b in behaviours)
        {
            if (b is T t) { return t; }
        }
        return null;
    }
}
