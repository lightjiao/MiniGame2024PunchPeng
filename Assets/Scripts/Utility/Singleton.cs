using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
    private static T _Instance;

    public static T Inst
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new T();
                (_Instance as Singleton<T>)?.OnInit();
            }

            return _Instance;
        }
    }

    protected abstract void OnInit();
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst;

    protected virtual void Awake()
    {
        Inst = GetComponent<T>();
    }
}
