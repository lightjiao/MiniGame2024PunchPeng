namespace PunchPeng
{
    public class BevSequence : BevComposites
    {
        private int m_CurIndx;
        private BevTask m_CurTask => m_Tasks.Count > m_CurIndx ? m_Tasks[m_CurIndx] : null;

        public override void OnStart()
        {
            base.OnStart();
            m_CurIndx = 0;
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            if (m_CurTask == null)
            {
                return TaskStatus.Success;
            }

            var status = BevTree.RunOneTask(m_CurTask, deltaTime);
            if (status == TaskStatus.Failure)
            {
                return TaskStatus.Failure;
            }

            m_CurIndx++;
            return TaskStatus.Running;
        }
    }
}