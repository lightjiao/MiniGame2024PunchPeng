using UnityEngine;

public static class Vector3Ex
{
    public static bool Approximately(this Vector3 self, Vector3 other, float e = MathEx.Epsilon)
    {
        return self.x.Approximately(other.x, e) &&
            self.y.Approximately(other.y, e) &&
            self.z.Approximately(other.z, e);
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
}
