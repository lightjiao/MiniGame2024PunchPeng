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
}