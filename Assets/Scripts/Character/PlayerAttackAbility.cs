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
    }

    public class PlayerHeadAttackAbility : PlayerAttackAbility
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_CfgAnim = m_Player.m_AnimData.HeadAttack;
            m_CfgDuration = m_CfgAnim.Length;
        }

        protected override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);

            // 第13 帧造成伤害
        }
    }

    public class PlayerPunchAttackAbility : PlayerAttackAbility
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_CfgAnim = m_Player.m_AnimData.PunchAttack;
            m_CfgDuration = m_CfgAnim.Length;
        }

        protected override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);

            // 第13 帧造成伤害
        }
    }
}