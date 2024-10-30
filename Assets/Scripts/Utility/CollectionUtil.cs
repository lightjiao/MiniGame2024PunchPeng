using System;
using System.Collections.Generic;

public static class CollectionUtil
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

    public static T RandomOne<T>(this IList<T> self)
    {
        if (self == null || self.Count == 0)
        {
            return default;
        }

        return self[UnityEngine.Random.Range(0, self.Count)];
    }
}