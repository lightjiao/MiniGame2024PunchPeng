using UnityEngine;

namespace PunchPeng
{
    public class BevCoolDown : BevConditional
    {
        private float m_Cd = 5;
        private float m_LastTime;

        public BevCoolDown SetCD(float cd)
        {
            m_Cd = cd;
            return this;
        }

        public override void Init(Player player)
        {
            base.Init(player);
            m_LastTime = 0;
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            if (Time.time - m_LastTime > m_Cd)
            {
                m_LastTime = Time.time;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}