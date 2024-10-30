using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil
{
    public const float Epsilon = 0.0001f;

    public static bool Approximately(this float self, float other, float e = Epsilon)
    {
        return Math.Abs(self - other) < e;
    }

    public static int ToMilliSec(this float self)
    {
        return (int)(self * 1000);
    }

    public static float RadToDeg(this float self)
    {
        return self * Mathf.Rad2Deg;
    }

    public static float DegToRad(this float self)
    {
        return self * Mathf.Deg2Rad;
    }
}

public class FloatComparer : IEqualityComparer<float>
{
    public static FloatComparer Default = new();

    public bool Equals(float x, float y)
    {
        return x.Approximately(y);
    }

    public int GetHashCode(float obj)
    {
        return obj.GetHashCode();
    }
}

public class Vector3Comparer : IEqualityComparer<Vector3>
{
    public static Vector3Comparer Default = new();

    public bool Equals(Vector3 x, Vector3 y)
    {
        return x.Approximately(y);
    }

    public int GetHashCode(Vector3 obj)
    {
        return obj.GetHashCode();
    }
}