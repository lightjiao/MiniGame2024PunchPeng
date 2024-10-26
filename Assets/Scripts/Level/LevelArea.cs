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

        [ReadOnly] public float MinX;
        [ReadOnly] public float MinZ;
        [ReadOnly] public float MaxX;
        [ReadOnly] public float MaxZ;

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

            MinX = boxPos.x - halfSize.x;
            MaxX = boxPos.x + halfSize.x;
            MinZ = boxPos.z - halfSize.z;
            MaxZ = boxPos.z + halfSize.z;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(new Vector3(MinX, 0, MinZ), new Vector3(MinX, 10, MinZ));
            Gizmos.DrawLine(new Vector3(MaxX, 0, MaxZ), new Vector3(MaxX, 10, MaxZ));
        }
    }
}