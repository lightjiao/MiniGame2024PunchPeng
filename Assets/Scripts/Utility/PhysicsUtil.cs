using UnityEngine;

public static class PhysicsUtil
{
    public static (Vector3, Vector3) GetBoxInfo(this BoxCollider self)
    {
        var boxWorldCenter = self.transform.TransformPoint(self.center);
        var boxSize = Vector3.Scale(self.size, self.transform.lossyScale);

        return (boxWorldCenter, boxSize);
    }

    public static bool BoxContainsPoint(this BoxCollider self, Vector3 point)
    {
        if (self == null) return false;

        var (worldCenter, boxSize) = self.GetBoxInfo();
        var rot = self.transform.rotation;

        return IsPointInBox(point, worldCenter, boxSize / 2, rot);
    }

    public static bool IsPointInBox(Vector3 point, Vector3 boxCenter, Vector3 boxHalfSize, Quaternion boxRot)
    {
        var localPoint = Quaternion.Inverse(boxRot) * (point - boxCenter);

        return Mathf.Abs(localPoint.x) <= boxHalfSize.x &&
               Mathf.Abs(localPoint.y) <= boxHalfSize.y &&
               Mathf.Abs(localPoint.z) <= boxHalfSize.z;
    }

    // 判断线段与 Box 是否相交
    public static unsafe bool LineintersectBoxSAT(Vector3 lineStart, Vector3 lineEnd, Vector3 boxCenter, Vector3 boxHalfSize, Quaternion boxRot)
    {
        var localVertices = stackalloc Vector3[8];
        localVertices[0] = new Vector3(-boxHalfSize.x, -boxHalfSize.y, -boxHalfSize.z);
        localVertices[1] = new Vector3(boxHalfSize.x, -boxHalfSize.y, -boxHalfSize.z);
        localVertices[2] = new Vector3(-boxHalfSize.x, boxHalfSize.y, -boxHalfSize.z);
        localVertices[3] = new Vector3(boxHalfSize.x, boxHalfSize.y, -boxHalfSize.z);
        localVertices[4] = new Vector3(-boxHalfSize.x, -boxHalfSize.y, boxHalfSize.z);
        localVertices[5] = new Vector3(boxHalfSize.x, -boxHalfSize.y, boxHalfSize.z);
        localVertices[6] = new Vector3(-boxHalfSize.x, boxHalfSize.y, boxHalfSize.z);
        localVertices[7] = new Vector3(boxHalfSize.x, boxHalfSize.y, boxHalfSize.z);

        var worldVertices = stackalloc Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            worldVertices[i] = boxCenter + boxRot * localVertices[i];
        }

        // 定义分离轴
        var boxAxes = stackalloc Vector3[3]; // Box 的 3 个局部轴
        boxAxes[0] = boxRot * Vector3.right;
        boxAxes[1] = boxRot * Vector3.up;
        boxAxes[2] = boxRot * Vector3.forward;

        var lineDir = (lineEnd - lineStart).normalized; // 线段方向

        // 所有分离轴
        var axes = stackalloc Vector3[7];
        axes[0] = boxAxes[0];
        axes[1] = boxAxes[1];
        axes[2] = boxAxes[2];
        axes[3] = lineDir;
        axes[4] = Vector3.Cross(lineDir, boxAxes[0]);
        axes[5] = Vector3.Cross(lineDir, boxAxes[1]);
        axes[6] = Vector3.Cross(lineDir, boxAxes[2]);

        // 3. SAT 检测
        var linePoints = stackalloc Vector3[2] { lineStart, lineEnd };
        for (int i = 0; i < 7; i++)
        {
            var axis = axes[i];
            if (axis == Vector3.zero) continue; // 忽略零向量

            // 投影 Box 和线段到当前轴
            (float minBox, float maxBox) = ProjectOntoAxisInternal(worldVertices, 8, axis);
            (float minLine, float maxLine) = ProjectOntoAxisInternal(linePoints, 2, axis);

            // 检查投影是否有间隔
            if (maxBox < minLine || maxLine < minBox)
            {
                return false; // 如果在某个轴上没有重叠，则不相交
            }
        }

        return true; // 所有轴上都有重叠，则相交
    }

    // 将点投影到轴上，返回投影的最小值和最大值
    private static unsafe (float min, float max) ProjectOntoAxisInternal(Vector3* vertices, int verticesCnt, Vector3 axis)
    {
        float min = Vector3.Dot(vertices[0], axis);
        float max = min;

        for (int i = 1; i < verticesCnt; i++)
        {
            float projection = Vector3.Dot(vertices[i], axis);
            min = Mathf.Min(min, projection);
            max = Mathf.Max(max, projection);
        }

        return (min, max);
    }
}