using UnityEngine;

public class LifecycleLogger : MonoBehaviour
{
    private void Awake()
    {
        Debug.LogError($"{name}.Awake");
    }

    private void Start()
    {
        Debug.LogError($"{name}.Start");
    }

    private void OnEnable()
    {
        Debug.LogError($"{name}.OnEnable");
    }

    private void OnDisable()
    {
        Debug.LogError($"{name}.OnDisable");
    }

    private void OnDestroy()
    {
        Debug.LogError($"{name}.OnDestroy");
    }
}
