namespace PunchPeng
{
    // 间隔多少时间执行一次action的buff基类
    public abstract class BuffIntervalAction : Buff
    {
        protected float m_CfgInterval;
        protected float m_IntervalElapsed;

        protected override void OnUpdate(float elapseSeconds)
        {
            base.OnUpdate(elapseSeconds);

            if (m_IntervalElapsed >= m_CfgInterval)
            {
                m_IntervalElapsed -= m_CfgInterval;
                InvervalTick();
            }
        }

        protected abstract void InvervalTick();
    }
}