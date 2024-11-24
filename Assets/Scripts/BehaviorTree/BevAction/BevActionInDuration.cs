namespace PunchPeng
{
    public class BevDurationAction : BevAction
    {
        protected float m_CfgDuration;
        protected float m_ElapsedTime;
        protected bool TimeEnd => m_ElapsedTime >= m_CfgDuration;
        protected TaskStatus TaskStatusByDuration => TimeEnd ? TaskStatus.Success : TaskStatus.Running;

        public override void OnStart()
        {
            base.OnStart();
            m_ElapsedTime = 0;
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;

            return TaskStatusByDuration;
        }
    }
}