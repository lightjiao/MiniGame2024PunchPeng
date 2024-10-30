using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHelper : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterAction;
    public Action<Collider> OnTriggerExitAction;

    [SerializeField]
    private List<Collider> IgnoreObjs = new();

    public void SetActiveEx(bool active)
    {
        gameObject.SetActiveEx(active);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IgnoreObjs != null && IgnoreObjs.Contains(other)) return;
        OnTriggerEnterAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (IgnoreObjs != null && IgnoreObjs.Contains(other)) return;
        OnTriggerExitAction?.Invoke(other);
    }
}
