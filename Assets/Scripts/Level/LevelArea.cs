using UnityEngine;

namespace PunchPeng
{
    public class LevelArea : MonoBehaviour
    {
        public static LevelArea Inst;

        public Collider Area;

        private void Awake()
        {
            Inst = this;

            if (Area == null)
            {
                Area = GetComponent<Collider>();
            }
        }
    }
}