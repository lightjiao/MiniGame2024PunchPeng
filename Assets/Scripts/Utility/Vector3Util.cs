using UnityEngine;

public static class Vector3Util
{
    public static bool Approximately(this Vector3 self, Vector3 other, float e = MathUtil.Epsilon)
    {
        return self.x.Approximately(other.x, e) &&
            self.y.Approximately(other.y, e) &&
            self.z.Approximately(other.z, e);
    }

    public static bool ApproximatelyZero(this Vector3 self, float e = MathUtil.Epsilon)
    {
        return self.Approximately(Vector3.zero, e);
    }

    public static float HorizontalMagnitude(this Vector3 self)
    {
        var copy = self;
        copy.y = 0;
        return copy.magnitude;
    }

    public static float HorizontalSqrMagnitude(this Vector3 self)
    {
        var copy = self;
        copy.y = 0;
        return copy.sqrMagnitude;
    }

    public static Vector3 SetY(this Vector3 self, float y = 0)
    {
        var copy = self;
        copy.y = y;
        return copy;
    }

    public static Vector3 ToHorizontalVector3(this Vector2 self)
    {
        return new Vector3(self.x, 0, self.y);
    }

    /// <summary>
    /// 椭圆网格映射
    /// </summary>
    /// <remarks>
    /// 将二维矢量作为移动的输入时，输入的矢量范围是一个正方形，显然正方形的斜角比边要长
    /// 如果直接将这个二维矢量作为移动的输入的话，会导致斜角的移动速度更快
    /// 采用这个函数将矢量的值域从正方形限制为圆形
    /// 而圆的半径是固定的，斜方向的移动速度不会更快
    /// 论文地址:https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf 第5页 elliptical grid mapping
    /// </remarks>
    public static Vector2 SquareToCircle(Vector2 input)
    {
        var output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    public static Vector3 SquareToCircle(Vector3 input)
    {
        var output = SquareToCircle(new Vector2(input.x, input.z));
        return new Vector3(output.x, input.y, output.y);
    }

    public static string ToStringEx(this Vector3 self)
    {
        return string.Format("({0:N2}, {1:N2}, {2:N2})", self.x, self.y, self.z);
    }

    public static Vector3 Rand2DDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    public static Vector3 RandomRange(Vector3 min, Vector3 max)
    {
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

    public static bool InRange(this Vector3 self, Vector3 min, Vector3 max)
    {
        return self.LessThan(max) && self.GreaterThan(min);
    }

    public static bool GreaterThan(this Vector3 self, Vector3 value, bool checkEqual = true)
    {
        if (checkEqual)
        {
            return self.x >= value.x && self.y >= value.y && self.z >= value.z;
        }
        return self.x > value.x && self.y > value.y && self.z > value.z;
    }

    public static bool LessThan(this Vector3 self, Vector3 value, bool checkEqual = true)
    {
        if (checkEqual)
        {
            return self.x <= value.x && self.y <= value.y && self.z <= value.z;
        }
        return self.x < value.x && self.y < value.y && self.z < value.z;
    }

    public static bool InRange2D(this Vector3 self, Vector3 min, Vector3 max)
    {
        return self.LessThan2D(max) && self.GreaterThan2D(min);
    }

    public static bool GreaterThan2D(this Vector3 self, Vector3 value, bool checkEqual = true)
    {
        if (checkEqual)
        {
            return self.x >= value.x && self.z >= value.z;
        }
        return self.x > value.x && self.z > value.z;
    }

    public static bool LessThan2D(this Vector3 self, Vector3 value, bool checkEqual = true)
    {
        if (checkEqual)
        {
            return self.x <= value.x && self.z <= value.z;
        }
        return self.x < value.x && self.z < value.z;
    }

    public static Vector3 ClampMagnitude(this Vector3 self, float min, float max)
    {
        var magnitude = self.magnitude;
        var clampedMagnitude = Mathf.Clamp(magnitude, min, max);

        if (magnitude == clampedMagnitude)
        {
            return self;
        }
        return self.normalized * clampedMagnitude;
    }
}
