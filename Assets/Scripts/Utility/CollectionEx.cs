using System;
using System.Collections.Generic;

public static class CollectionEx
{
    private static Random s_Random = new Random();

    public static T RandomOne<T>(this IList<T> self)
    {
        if (self == null || self.Count == 0)
        {
            return default;
        }

        return self[s_Random.Next(self.Count)];
    }
}