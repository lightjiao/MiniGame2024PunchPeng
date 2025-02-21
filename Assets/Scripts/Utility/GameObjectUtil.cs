using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public static class GameObjectUtil
{
    public static T GetOrAddComponent<T>(this GameObject self) where T : Component
    {
        if (self == null) return null;

        var ret = self.GetComponent<T>();
        if (ret != null) return ret;

        return self.AddComponent<T>();
    }

    public static void SetActiveEx(this GameObject self, bool active)
    {
        if (active == self.activeInHierarchy) return;

        self.SetActive(active);
    }

    public static List<T> GetComponentsInChildrenEx<T>(this GameObject self) where T : Component
    {
        var list = new List<T>();
        self.GetComponentsInChildrenEx(ref list);
        return list;
    }

    private static void GetComponentsInChildrenEx<T>(this GameObject self, ref List<T> list) where T : Component
    {
        foreach (Transform child in self.transform)
        {
            var comps = child.GetComponents<T>();
            if (comps != null)
            {
                list.AddRange(comps);
            }

            child.gameObject.GetComponentsInChildrenEx(ref list);
        }
    }

    public static bool IsDestroyed(this UnityObject target)
    {
        // Checks whether a Unity object is not actually a null reference,
        // but a rather destroyed native instance.

        return !ReferenceEquals(target, null) && target == null;
    }

    public static bool IsUnityNull(this object obj)
    {
        // Checks whether an object is null or Unity pseudo-null
        // without having to cast to UnityEngine.Object manually

        return obj == null || ((obj is UnityObject) && ((UnityObject)obj) == null);
    }

    public static T AsUnityNull<T>(this T obj) where T : UnityObject
    {
        // Converts a Unity pseudo-null to a real null, allowing for coalesce operators.
        // e.g.: destroyedObject.AsUnityNull() ?? otherObject

        if (obj == null)
        {
            return null;
        }

        return obj;
    }

    public static IEnumerable<T> NotUnityNull<T>(this IEnumerable<T> enumerable) where T : UnityObject
    {
        return enumerable.Where(i => i != null);
    }

    public static void DestroyRef<T>(ref T obj) where T : Component
    {
        if (obj == null) return;
        GameObject.Destroy(obj.gameObject);
        obj = null;
    }

    public static void DestroyRef(ref GameObject obj)
    {
        if (obj == null) return;
        GameObject.Destroy(obj);
        obj = null;
    }

    public static void DestroyGo(this MonoBehaviour self, GameObject go)
    {
        if (go == null) return;
        GameObject.Destroy(go);
    }

    public static void DestroyGo(GameObject obj)
    {
        if (obj == null) return;
        GameObject.Destroy(obj);
    }
}