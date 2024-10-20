using UnityEngine;

public class MonoEntity : MonoBehaviour
{
    private Transform _trans;
    public Transform CachedTransform
    {
        get
        {
            if (_trans == null) _trans = transform;
            return _trans;
        }
    }

    public Vector3 Position
    {
        get => CachedTransform.position;
        set => CachedTransform.position = value;
    }

    public Quaternion Rotation
    {
        get => CachedTransform.rotation;
        set => CachedTransform.rotation = value;
    }

    public Vector3 Forward => CachedTransform.forward;
}