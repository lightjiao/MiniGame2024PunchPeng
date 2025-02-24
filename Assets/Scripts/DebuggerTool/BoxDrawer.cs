using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrawer : MonoBehaviour
{
    List<GameObject> vertices = new();

    [Button]
    private void DrawRot(float x, float y, float z, float w)
    {
        transform.rotation = new Quaternion(x, y, z, w);
    }

    [Button]
    private void DrawVerticlesFromStr(string verticesStr)
    {
        var pointStrs = verticesStr.Split(';');

        var pointList = new List<Vector3>();
        foreach (var pointStr in pointStrs)
        {
            pointList.Add(Vector3Util.TryParse(pointStr));
        }

        this.DestroyAllChild();
        foreach (var go in vertices)
        {
            if (go == null) continue;
            GameObject.DestroyImmediate(go);
        }
        vertices.Clear();

        foreach (var point in pointList)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = point;
            go.transform.localScale = Vector3.one * 0.2f;
            vertices.Add(go);
        }
    }

    [Button]
    public void TestLineInterat()
    {
        //181.14, 3.22, 178.70), (181.10, 3.22, 178.79)
        var p1 = new Vector3(181.14f, 3.22f, 178.70f);
        var p2 = new Vector3(181.10f, 3.22f, 178.79f);
        Debug.DrawLine(p1, p2, Color.green, 20f);
        Debug.DrawRay(p1, (p2 - p1).normalized, Color.green, 10);
        var box = GetComponent<BoxCollider>();
        var result = PhysicsUtil.LineintersectBoxSAT(p1, p2, box);
        var boxInfo = box.GetBoxInfo();
        //Log.Info($"{p1}, {p2}, {boxInfo.Item1}, {boxInfo.Item1}, {boxInfo.Item3}");
    }
}
