namespace PunchPeng
{
    public class BevDurationAction : BevAction
    {
        protected float m_CfgDuration;
        protected float m_ElapsedTime;
        protected bool TimeEnd => m_ElapsedTime >= m_CfgDuration;
        protected TaskStatus TaskStatusByDuration => TimeEnd ? TaskStatus.Success : TaskStatus.Running;

        public BevDurationAction SetDuration(float duration)
        {
            m_CfgDuration = duration;
            return this;
        }

        public override void OnBevStart()
        {
            base.OnBevStart();
            m_ElapsedTime = 0;
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;

            return TaskStatusByDuration;
        }
    }
}