using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    public string m_PointsByStr;

    public bool m_UpdateDrawPoint = false;
    public Transform Point1;
    public Transform Point2;

    private HashSet<Vector3> m_Points = new();
    private List<GameObject> m_PointGo = new();

    private LineRenderer m_LineRenderer;
    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        Assert.IsNotNull(m_LineRenderer);
    }

    private void Update()
    {
        if (m_UpdateDrawPoint)
        {
            m_Points.Clear();
            if (Point1 != null && Point2 != null)
            {
                m_Points.Add(Point1.position);
                m_Points.Add(Point2.position);
            }
            DrawLineInternal();
        }
    }

    [Button]
    private void ParseStrAndDrawLine()
    {
        m_Points.Clear();
        if (string.IsNullOrEmpty(m_PointsByStr))
        {
            return;
        }

        var points = m_PointsByStr.Split(';');
        foreach (var strPoint in points)
        {
            var point = strPoint.Split(',');
            if (point.Length != 3) continue;
            m_Points.Add(new Vector3(float.Parse(point[0]), float.Parse(point[1]), float.Parse(point[2])));
        }
        DrawLineInternal();
    }

    public void DrawLine(List<Vector3> points)
    {
        m_Points.Clear();
        m_Points = points.ToHashSet();
        DrawLineInternal();
    }

    public void DrawLine(params Vector3[] points)
    {
        m_Points.Clear();
        m_Points = points.ToHashSet();
        DrawLineInternal();
    }

    private void DrawLineInternal()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        if (m_LineRenderer == null) return;

        m_LineRenderer.startWidth = 0.1f;
        m_LineRenderer.endWidth = 0.1f;
        m_LineRenderer.positionCount = m_Points.Count;
        m_LineRenderer.SetPositions(m_Points.ToArray());

        foreach (var item in m_PointGo)
        {
            DestroyImmediate(item);
        }
        m_PointGo.Clear();

        foreach (var point in m_Points)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var collider = go.GetComponent<Collider>();
            DestroyImmediate(collider);

            go.transform.parent = transform;
            go.transform.localScale = Vector3.one * 0.2f;
            go.transform.localPosition = point;
            m_PointGo.Add(go);
        }
    }
}
