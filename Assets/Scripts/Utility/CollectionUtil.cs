using System;
using System.Collections.Generic;

public static class CollectionUtil
{
    public static void KeysCopyTo<TKey, TValue>(this Dictionary<TKey, TValue> self, List<TKey> keys, bool clear = true)
    {
        if (self == null || keys == null)
        {
            throw new NullReferenceException("KeysCopyTo");
        }

        if (clear)
        {
            keys.Clear();
        }

        foreach (var kv in self)
        {
            keys.Add(kv.Key);
        }
    }

    public static int RandomIndex<T>(this IList<T> self)
    {
        if (self == null || self.Count == 0)
        {
            return default;
        }

        return UnityEngine.Random.Range(0, self.Count);
    }

    public static T RandomOne<T>(this IList<T> self)
    {
        if (self == null || self.Count == 0)
        {
            return default;
        }

        return self[UnityEngine.Random.Range(0, self.Count)];
    }

    public static string ToStringEx<T>(this ICollection<T> self)
    {
        return $"[{string.Join(", ", self)}]";
    }
}