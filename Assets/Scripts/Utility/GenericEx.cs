using System;
using System.Collections.Generic;

public static class GenericEx
{
    public static void KeysCopyTo<TKey, TValue>(this Dictionary<TKey, TValue> self, List<TKey> keys)
    {
        if (self == null || keys == null)
        {
            throw new NullReferenceException("KeysCopyTo");
        }

        keys.Clear();
        foreach (var kv in self)
        {
            keys.Add(kv.Key);
        }
    }
}