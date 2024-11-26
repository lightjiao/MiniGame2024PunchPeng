namespace PunchPeng
{
    // 间隔多少时间执行一次action的buff基类
    public abstract class BuffIntervalAction : Buff
    {
        protected float m_CfgInterval;
        protected float m_IntervalElapsed;

        protected override void OnBuffAwake()
        {
            base.OnBuffAwake();
            m_CfgInterval = m_Cfg.Param1;
        }

        protected override void OnBuffUpdate(float elapseSeconds)
        {
            base.OnBuffUpdate(elapseSeconds);

            m_IntervalElapsed += elapseSeconds;

            if (m_IntervalElapsed >= m_CfgInterval)
            {
                m_IntervalElapsed -= m_CfgInterval;
                InvervalTick();
            }
        }

        protected abstract void InvervalTick();
    }
}