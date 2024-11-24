namespace PunchPeng
{
    public class BevEntryTask : BevTask
    {
        private BevTask m_Task;

        public void AddTask(BevTask task)
        {
            m_Task = task;
        }

        public override void Init(Player player)
        {
            base.Init(player);
            m_Task.Init(player);
        }

        public override void OnStart()
        {
            base.OnStart();
            m_Task.OnStart();
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            return m_Task.OnUpdate(deltaTime);
        }

        public override void OnEnd()
        {
            m_Task.OnEnd();
            base.OnEnd();
        }
    }
}
