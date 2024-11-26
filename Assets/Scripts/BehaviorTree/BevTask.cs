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

        public virtual void OnBevAwake(Player player)
        {
            m_Player = player;
        }

        public virtual void OnBevStart()
        {
            IsRunning = true;
        }

        public virtual TaskStatus OnBevUpdate(float deltaTime)
        {
            return TaskStatus.Success;
        }

        public virtual void OnBevEnd()
        {
            IsRunning = false;
        }
    }
}
