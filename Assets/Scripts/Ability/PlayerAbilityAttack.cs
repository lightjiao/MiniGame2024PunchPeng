using ConfigAuto;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerAbilityAttack : PlayerAbility
    {
        protected override bool AbilityCanStart()
        {
            var can = m_Player.InputAttack && m_Player.CanAttack && base.AbilityCanStart();

            return can; ;
        }

        protected override void AbilityOnStart()
        {
            base.AbilityOnStart();
            m_Player.CanInputMove--;
            AudioManager.Inst.Play2DSfx(Config_Global.Inst.data.Sfx.PlayerPunchSfx).Forget();
        }

        protected override void AbilityOnStop()
        {
            m_Player.CanInputMove++;
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

    public class PlayerAbilityAttackByHead : PlayerAbilityAttack
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

    public class PlayerAbilityAttackByPunch : PlayerAbilityAttack
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
            PunchDamage().Forget();
        }

        private async UniTask PunchDamage()
        {
            await UniTask.Delay(Config_Global.Inst.data.PlayerPunchAtkTime.ToMilliSec());
            if (m_Player == null || m_Player.IsDead) return;
            m_Player.m_PunchAttackTrigger.SetActiveEx(true);

            await UniTask.Delay(Config_Global.Inst.data.PlayerPunchAtkDuration.ToMilliSec());
            if (m_Player == null || m_Player.IsDead) return;
            m_Player.m_PunchAttackTrigger.SetActiveEx(false);

            // TODO: 实现一个clip类，设置一个时间开始，设置一个时间结束，分别执行什么逻辑
        }
    }
}