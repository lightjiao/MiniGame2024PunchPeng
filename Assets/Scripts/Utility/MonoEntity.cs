using System.Threading;
using UnityEngine;

public class MonoEntity : MonoBehaviour
{
    private CancellationTokenSource _destroyCts;
    protected CancellationTokenSource m_DestroyCts => _destroyCts ??= new CancellationTokenSource();

    private Transform _trans;
    public Transform CachedTransform
    {
        get
        {
            if (_trans == null) _trans = transform;
            return _trans;
        }
    }

    public virtual Vector3 Position
    {
        get => CachedTransform.position;
        set => CachedTransform.position = value;
    }

    public virtual Quaternion Rotation
    {
        get => CachedTransform.rotation;
        set => CachedTransform.rotation = value;
    }

    public virtual Vector3 Forward
    {
        get => CachedTransform.forward;
        set => CachedTransform.forward = value;
    }

    protected virtual void OnDestroy()
    {
        _destroyCts?.Cancel();
    }
}