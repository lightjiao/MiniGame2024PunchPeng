namespace PunchPeng
{
    public class BevTree
    {
        private Player m_Player;
        private BevEntryTask m_EntryTask = new();

        public void Init(Player player)
        {
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

        public static BevTree CreateBevTree()
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

            var tree = new BevTree();
            tree.m_EntryTask.AddTask(parallel);

            return tree;
        }
    }
}