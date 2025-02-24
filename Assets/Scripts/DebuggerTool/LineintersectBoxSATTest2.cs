using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineintersectBoxSATTest2 : MonoBehaviour
{
    public BoxCollider[] m_Box;
    public string m_PointsByStr;
    private List<Vector3> m_Points = new();

    [Button]
    public void Test()
    {
        m_Points.Clear();
        if (string.IsNullOrEmpty(m_PointsByStr))
        {
            return;
        }

        var points = m_PointsByStr.Split(';');
        foreach (var strPoint in points)
        {
            var point = Vector3Util.TryParse(strPoint);
            if (m_Points.Contains(point))
            {
                continue;
            }
            m_Points.Add(point);
        }

        var result = new List<(Vector3, Vector3)>();
        foreach (var box in m_Box)
        {
            for (var i = 1; i < m_Points.Count; i++)
            {
                var p1 = m_Points[i - 1];
                var p2 = m_Points[i];
                if (PhysicsUtil.LineintersectBoxSAT(p1, p2, box))
                {
                    result.Add((p1, p2));
                    Debug.Log($"{p1}, {p2}, {box.name}");
                }
            }
        }
    }
}
