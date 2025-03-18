using UnityEngine;

public static class BezierUtil
{
    /// <summary>
    /// Cal bezier result
    /// </summary>
    /// <param name="result"> length of result is bezier interpolation </param>
    /// <param name="points"> bezier point arr </param>
    public static void CalBezier(ref Vector3[] result, params Vector3[] points)
    {
        if (points.Length == 3)
        {
            CalBezier(ref result, points[0], points[1], points[2]);
            return;
        }

        if (points.Length == 4)
        {
            CalBezier(ref result, points[0], points[1], points[2], points[3]);
            return;
        }

        if (points == null || points.Length <= 1 || result == null || result.Length < points.Length)
        {
            return;
        }

        var resolution = 1f / (result.Length - 1);
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = CalBezierPoint(resolution * i, points);
        }
    }

    public static void CalBezier(ref Vector3[] result, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        if (result == null || result.Length < 3)
        {
            return;
        }

        var resolution = 1f / (result.Length - 1);
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = CalBezierPoint(resolution * i, p0, p1, p2);
        }
    }

    public static void CalBezier(ref Vector3[] result, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        if (result == null || result.Length < 4)
        {
            return;
        }

        var resolution = 1f / (result.Length - 1);
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = CalBezierPoint(resolution * i, p0, p1, p2, p3);
        }
    }

    public static Vector3 CalBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        var u = 1 - t;
        return u * u * p0 + 2 * t * u * p1 + t * t * p2;
    }

    public static Vector3 CalBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float tt = t * t;
        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        var p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public static Vector3 CalBezierPoint(float t, params Vector3[] p)
    {
        int n = p.Length - 1;
        float u = 1 - t;

        var result = Vector3.zero;

        for (int i = 0; i < p.Length; i++)
        {
            result += Mathf.Pow(t, i) * Mathf.Pow(u, n - i) * MathUtil.Combination(n, i) * p[i];
        }

        return result;
    }
}