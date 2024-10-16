using System;
using System.Runtime.CompilerServices;

public class LazyVar<T>
{
    private bool _hasInit;
    private T _value;

    public T Value
    {
        get
        {
            TryInit();
            return _value;
        }

        set => _value = value;
    }

    private Func<T> _func;

    public LazyVar(Func<T> func)
    {
        _func = func;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void TryInit()
    {
        if (_hasInit) return;

        _value = _func.Invoke();
        _hasInit = true;
    }
}
