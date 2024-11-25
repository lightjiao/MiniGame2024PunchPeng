using UnityEngine;

namespace PunchPeng
{
    public class BevRandomCanAttackPlayer : BevConditional
    {
        private float m_CfgAtkCd = 5;
        private float m_AtkTime = 0;
        private float m_AttackPct = 0.1f;

        public override TaskStatus OnUpdate(float deltaTime)
        {
            if (Time.time - m_AtkTime > m_CfgAtkCd && HasPlayerInFrontOfMe())
            {
                m_AtkTime = Time.time;
                if (MathUtil.InPercent(m_AttackPct))
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }

        private bool HasPlayerInFrontOfMe()
        {
            var predictPos = (m_Player.Position + m_Player.Forward);

            foreach (var player in GameController.Inst.PlayerList)
            {
                if (m_Player == player || player.IsDead) continue;
                if ((player.Position - predictPos).SetY(0).magnitude < 0.5f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}