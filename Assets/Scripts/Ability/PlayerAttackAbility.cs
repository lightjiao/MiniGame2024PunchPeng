using ConfigAuto;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerAttackAbility : PlayerAbility
    {
        protected override bool AbilityCanStart()
        {
            var can = m_Player.InputAttack && m_Player.CanAttack && base.AbilityCanStart();

            return can; ;
        }

        protected override void AbilityOnStart()
        {
            base.AbilityOnStart();
            m_Player.CanMove.RefCnt--;
            m_Player.PlaySfx(Config_Global.Inst.data.PlayerPunchSfx).Forget();
        }

        protected override void AbilityOnStop()
        {
            m_Player.CanMove.RefCnt++;
            base.AbilityOnStop();
        }

        protected void AttackOther(Collider other)
        {
            var player = other.GetComponent<Player>();
            if (player != null && !m_Player.IsDead)
            {
                player.RecieveDamage(m_Player, 999);
            }
        }
    }

    public class PlayerHeadAttackAbility : PlayerAttackAbility
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_CfgAnim = m_Player.m_AnimData.HeadAttack;
            m_CfgDuration = m_CfgAnim.Length;
        }

        protected override void AbilityOnStart()
        {
            base.AbilityOnStart();
            m_Player.m_HeadAttackTrigger.SetActiveEx(true);
            m_Player.m_HeadAttackTrigger.OnTriggerEnterAction += AttackOther;
        }

        protected override void AbilityOnStop()
        {
            base.AbilityOnStop();
            m_Player.m_HeadAttackTrigger.OnTriggerEnterAction -= AttackOther;
            m_Player.m_HeadAttackTrigger.SetActiveEx(false);
        }
    }

    public class PlayerPunchAttackAbility : PlayerAttackAbility
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_CfgAnim = m_Player.m_AnimData.PunchAttack;
            m_CfgDuration = m_CfgAnim.Length;
            m_Player.m_PunchAttackTrigger.OnTriggerEnterAction += AttackOther;
        }

        protected override void AbilityOnStart()
        {
            base.AbilityOnStart();
            PunchFrame().Forget();
        }

        private async UniTask PunchFrame()
        {
            var punchFrame = (int)(Config_Global.Inst.data.PlayerPunchFrame / Time.timeScale);
            var damageFrame = (int)(5 / Time.timeScale);

            await UniTask.DelayFrame(punchFrame);
            if (m_Player == null || m_Player.IsDead) return;

            m_Player.m_PunchAttackTrigger.SetActiveEx(true);
            await UniTask.DelayFrame(damageFrame);

            if (m_Player == null || m_Player.IsDead) return;
            m_Player.m_PunchAttackTrigger.SetActiveEx(false);
        }
    }
}