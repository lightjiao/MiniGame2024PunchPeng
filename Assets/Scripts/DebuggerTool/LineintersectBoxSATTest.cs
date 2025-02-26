using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class LineintersectBoxSATTest : MonoBehaviour
{
    public BoxCollider m_Box;
    public LineDrawer m_Line;

    [ShowInInspector, ReadOnly]
    private bool m_RayCastRsult;
    [ShowInInspector, ReadOnly]
    private bool m_FuncResult;

    public bool m_StopTest;

    [Button]
    private async void StartRandomTest()
    {
        m_StopTest = false;
        if (m_Box == null || m_Line == null)
        {
            return;
        }

        while (true)
        {
            RandomGenBoxAndLine();
            await UniTask.WaitForFixedUpdate();
            Test();
            if (m_RayCastRsult != m_FuncResult)
            {
                return;
            }

            await UniTask.Delay(0.15f.ToMilliSec());

            if (m_StopTest) return;
        }
    }

    [Button]
    private void StartTest()
    {
        if (m_Box == null || m_Line == null)
        {
            return;
        }

        Test();
    }

    private void RandomGenBoxAndLine()
    {
        m_Box.transform.localScale = Vector3Util.RandomRange(Vector3.one * 0.1f, Vector3.one * 5f);
        m_Box.transform.localPosition = Vector3Util.RandomRange(Vector3.one * 0.1f, Vector3.one * 5f);
        m_Box.transform.forward = Vector3Util.RandomRange(Vector3.zero, Vector3.one).SetY(0).normalized;

        m_Line.Point1.position = Vector3Util.RandomRange(Vector3.one * 0.1f, Vector3.one * 5f);
        m_Line.Point2.position = Vector3Util.RandomRange(Vector3.one * 0.1f, Vector3.one * 5f);
        m_Line.DrawLine(m_Line.Point1.position, m_Line.Point2.position);
    }

    private void Test()
    {
        var (boxCenter, boxSize, boxRot) = m_Box.GetBoxInfo();

        var p1 = m_Line.Point1.position;
        var p2 = m_Line.Point2.position;
        var dir = p2 - p1;
        var distance = dir.magnitude;
        var hitResult = Physics.Raycast(p1, dir.normalized, out var hitInfo, distance);
        m_RayCastRsult = hitResult;// || m_Box.BoxContainsPoint(p1) || m_Box.BoxContainsPoint(p2);

        m_FuncResult = PhysicsUtil.LineIntersectBoxSAT(p1, p2, boxCenter, boxSize / 2, boxRot);
    }
}
