using System;

public class LazyVar<T> where T : new()
{
    private T _value;

    public T Value
    {
        get
        {
            if (_value == null)
            {
                if (m_CustomConstructerFunc != null)
                {
                    _value = m_CustomConstructerFunc.Invoke();
                }
                else
                {
                    _value = Activator.CreateInstance<T>();
                }
            }

            return _value;
        }

        set => _value = value;
    }

    private Func<T> m_CustomConstructerFunc;

    public LazyVar(Func<T> func = null)
    {
        m_CustomConstructerFunc = func;
    }
}
