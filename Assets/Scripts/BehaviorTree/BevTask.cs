namespace PunchPeng
{
    public enum TaskStatus
    {
        Inactive,
        Failure,
        Success,
        Running
    }

    public abstract class BevTask
    {
        protected Player m_Player;
        public bool IsRunning;

        public virtual void Init(Player player)
        {
            m_Player = player;
        }

        public virtual void OnStart()
        {
            IsRunning = true;
        }

        public virtual TaskStatus OnUpdate(float deltaTime)
        {
            return TaskStatus.Success;
        }

        public virtual void OnEnd()
        {
            IsRunning = false;
        }
    }
}
