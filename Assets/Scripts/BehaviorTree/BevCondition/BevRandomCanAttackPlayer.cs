using UnityEngine;

namespace PunchPeng
{
    public class BevRandomCanAttackPlayer : BevConditional
    {
        private float m_CfgAtkCd = 5;
        private float m_AtkTime = 0;

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            if (LevelController.Inst.DisableAIBevAttack)
            {
                return TaskStatus.Failure;
            }

            if (Time.time - m_AtkTime > m_CfgAtkCd && HasPlayerInFrontOfMe())
            {
                m_AtkTime = Time.time;
                if (MathUtil.InPercent(LevelController.Inst.CurLevelCfg.AIAttackPct))
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }

        private bool HasPlayerInFrontOfMe()
        {
            var predictPos = (m_Player.Position + m_Player.Forward);

            foreach (var player in LevelController.Inst.PlayerList)
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