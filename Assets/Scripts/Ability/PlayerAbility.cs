using Animancer;

namespace PunchPeng
{
    public class PlayerAbility
    {
        protected Player m_Player;

        protected float m_CfgDuration;
        protected ClipTransition m_CfgAnim;

        public bool IsEffecting { get; protected set; }
        protected float m_EffectingElapsed;

        public void Init(Player player)
        {
            m_Player = player;
            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        public void Update(float elapseSeconds)
        {
            if (!IsEffecting)
            {
                if (AbilityCanStart())
                {
                    AbilityStart();
                }
            }
            else
            {
                m_EffectingElapsed += elapseSeconds;
                if (AbilityCanStop())
                {
                    AbilityStop();
                }
            }
            OnUpdate(elapseSeconds);
        }

        protected virtual void OnUpdate(float elapseSeconds)
        {

        }

        protected virtual bool AbilityCanStart()
        {
            return !IsEffecting && !m_Player.IsDead;
        }

        protected void AbilityStart()
        {
            IsEffecting = true;
            m_EffectingElapsed = 0;

            m_Player.LocomotionState.Value = PlayerLocomotionState.Ability;
            m_Player.PlayAnim(m_CfgAnim);
            AbilityOnStart();
        }

        protected virtual void AbilityOnStart()
        {

        }

        protected virtual bool AbilityCanStop()
        {
            if (IsEffecting && m_EffectingElapsed > m_CfgDuration)
            {
                return true;
            }

            return false;
        }

        protected void AbilityStop()
        {
            IsEffecting = false;
            m_Player.LocomotionState.Value = PlayerLocomotionState.Locomotion;
            AbilityOnStop();
        }

        protected virtual void AbilityOnStop()
        {
        }
    }
}