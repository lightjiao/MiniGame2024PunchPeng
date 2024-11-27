using System;
using System.Collections.Generic;

public interface IPooledGameObject
{
    public void Acquire()
    {

    }

    public void Release()
    {

    }
}


public class GameObjectPoolManager : SingletonMono<GameObjectPoolManager>
{
    private Dictionary<Type, Stack<IPooledGameObject>> m_AllPool = new();

    protected override void OnAwake()
    {

    }
}
