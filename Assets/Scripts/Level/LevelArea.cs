using Drawing;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace PunchPeng
{
    public class LevelArea : MonoBehaviour
    {
        public static LevelArea Inst;

        public Collider Area;

        [ReadOnly] public Vector3 Min;
        [ReadOnly] public Vector3 Max;

        private void Awake()
        {
            Inst = this;

            CalSize();
        }

        [Button("CalSize")]
        private void CalSize()
        {
            if (Area == null)
            {
                Area = GetComponent<Collider>();
            }

            var boxCollider = Area as BoxCollider;
            if (boxCollider == null)
            {
                throw new System.Exception("CreatePlayerAsync Has no box collider");
            }
            var boxPos = boxCollider.transform.position + boxCollider.center;
            var halfSize = boxCollider.size * 0.5f;

            var MinX = boxPos.x - halfSize.x;
            var MaxX = boxPos.x + halfSize.x;
            var MinZ = boxPos.z - halfSize.z;
            var MaxZ = boxPos.z + halfSize.z;
            Min = new Vector3(MinX, 0, MinZ);
            Max = new Vector3(MaxX, 0, MaxZ);
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawLine(new Vector3(MinX, 0, MinZ), new Vector3(MinX, 10, MinZ));
            //Gizmos.DrawLine(new Vector3(MaxX, 0, MaxZ), new Vector3(MaxX, 10, MaxZ));
            Gizmos.DrawLine(Min, Min.SetY(10));
            Gizmos.DrawLine(Max, Max.SetY(10));
        }
    }
}