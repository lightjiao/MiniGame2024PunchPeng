namespace PunchPeng
{
    public class BevTree
    {
        private Player m_Player;
        private BevEntryTask m_EntryTask;

        public void Init(Player player)
        {
            CreateAsNormal();

            m_Player = player;
            m_EntryTask.Init(m_Player);
        }

        public void OnUpdate(float deltaTime)
        {
            RunOneTask(m_EntryTask, deltaTime);
        }

        public static TaskStatus RunOneTask(BevTask task, float deltaTime)
        {
            if (!task.IsRunning)
            {
                task.OnStart();
            }

            var taskStatus = task.OnUpdate(deltaTime);

            if (taskStatus == TaskStatus.Success || taskStatus == TaskStatus.Failure)
            {
                task.OnEnd();
            }

            return taskStatus;
        }

        // ============ quite hack
        public void CreateAsNormal()
        {
            var randomMove = new BevSelector();
            randomMove.AddTask(new BevIdle());
            randomMove.AddTask(new BevIdle());
            randomMove.AddTask(new BevIdle());
            randomMove.AddTask(new BevMove());
            randomMove.AddTask(new BevMove());
            randomMove.AddTask(new BevRun());

            var randomAttack = new BevSequence();
            randomAttack.AddTask(new BevRandomCanAttackPlayer());
            randomAttack.AddTask(new BevAttack());

            var parallel = new BevParallel();
            parallel.AddTask(randomMove);
            parallel.AddTask(randomAttack);

            m_EntryTask = new BevEntryTask();
            m_EntryTask.AddTask(parallel);
        }

        public void CreateWithoutAttack()
        {

        }
    }
}