namespace PunchPeng
{
    public class Buff
    {
        protected Player m_Player;
        protected float m_CfgDuration;

        protected float m_EffectingElapsed;

        public void Init(Player player)
        {
            m_Player = player;
            OnInit();
        }

        protected virtual void OnInit()
        {
            BuffStart();
        }

        public void Update(float elapseSeconds)
        {
            m_EffectingElapsed += elapseSeconds;
            if (m_EffectingElapsed > m_CfgDuration)
            {
                BuffEnd();
            }
            else
            {
                OnUpdate(elapseSeconds);
            }
        }

        protected virtual void OnUpdate(float elapseSeconds)
        {

        }

        private void BuffStart()
        {
            OnBuffStart();
        }

        protected virtual void OnBuffStart()
        {

        }

        private void BuffEnd()
        {
            OnBuffEnd();
        }

        protected virtual void OnBuffEnd()
        {

        }
    }
}