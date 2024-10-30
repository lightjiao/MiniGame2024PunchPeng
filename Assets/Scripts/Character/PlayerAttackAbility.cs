using ConfigAuto;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerAttackAbility : PlayerAbility
    {
        protected override bool AbilityCanStart()
        {
            var can = m_Player.PlayerInputAttack.Value && base.AbilityCanStart();
            m_Player.PlayerInputAttack.Value = false;

            return can; ;
        }

        protected override void AbilityOnStart()
        {
            base.AbilityOnStart();
            m_Player.CanMove = false;
        }

        protected override void AbilityOnStop()
        {
            m_Player.CanMove = true;
            base.AbilityOnStop();
        }

        protected void AttackOther(Collider other)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
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
            await UniTask.DelayFrame(Config_Global.Inst.data.PlayerPunchFrame);
            m_Player.m_PunchAttackTrigger.SetActiveEx(true);
            await UniTask.DelayFrame(2);
            m_Player.m_PunchAttackTrigger.SetActiveEx(false);
        }
    }
}