using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class MathUtil
{
    public const float Epsilon = 0.0001f;

    public static bool Approximately(this float self, float other, float e = Epsilon)
    {
        return Math.Abs(self - other) < e;
    }

    public static bool ApproximatelyZero(this float self, float e = Epsilon)
    {
        return self.Approximately(0);
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

    public static bool InPercent(float pct)
    {
        if (pct == 0) return false;
        return UnityEngine.Random.Range(0, 1f) <= pct;
    }

    public static float SafeDivision(this float self, float value, float? returnValueWhenNotSafe = null)
    {
        if (value == 0)
        {
            return returnValueWhenNotSafe != null ? returnValueWhenNotSafe.Value : 0;
        }
        return self / value;
    }

    /// <summary>
    /// 阶乘
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static int Factorial(int n)
    {
        if (n == 0)
        {
            return 1;
        }
        return n * Factorial(n - 1);
    }

    /// <summary>
    /// 组合
    /// </summary>
    /// <param name="n"></param>
    /// <param name="m"></param>
    /// <returns></returns>
    public static int Combination(int n, int m)
    {
        return Factorial(n) / (Factorial(m) * Factorial(n - m));
    }

    /// <summary>
    /// 忽略多语言的字符串解析浮点数
    /// </summary>
    /// <remarks>
    /// 我们日常的数字规范里面，千分位的计数是半角逗号，小数点是半角句号 `1,234,567.89`，但某些语言（俄语、越南）正好是反的 `1.234.567,89`
    /// 这种情况，调用C#的 `float.TryParse()` 它会默认处理多语言情况，这个时候字符串 `"0.8"` 会被解析成数字 `8` ，而不是数字 `0.8`
    /// </remarks>
    /// <param name="str"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParseFloat(string str, out float result)
    {
        return float.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    }

    public static bool TryParseFloat(ReadOnlySpan<char> s, out float result)
    {
        return float.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
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