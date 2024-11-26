namespace PunchPeng
{
    public class BevEntryTask : BevTask
    {
        private BevTask m_Task;

        public void AddTask(BevTask task)
        {
            m_Task = task;
        }

        public override void OnBevAwake(Player player)
        {
            base.OnBevAwake(player);
            m_Task.OnBevAwake(player);
        }

        public override void OnBevStart()
        {
            base.OnBevStart();
            m_Task.OnBevStart();
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            return m_Task.OnBevUpdate(deltaTime);
        }

        public override void OnBevEnd()
        {
            m_Task.OnBevEnd();
            base.OnBevEnd();
        }
    }
}
