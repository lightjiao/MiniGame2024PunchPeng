using System.Collections.Generic;
using UnityEngine;

public static class CollectionEx
{
    public static T RandomOne<T>(this IList<T> self)
    {
        if (self == null || self.Count == 0)
        {
            return default;
        }

        return self[Random.Range(0, self.Count)];
    }
}