namespace PunchPeng
{
    public class BevSelector : BevComposites
    {
        private BevTask m_SelectedTask;

        public override void OnStart()
        {
            base.OnStart();
            m_SelectedTask ??= m_Tasks.RandomOne();
            m_SelectedTask?.OnStart();
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            return m_SelectedTask == null ? TaskStatus.Success : m_SelectedTask.OnUpdate(deltaTime);
        }

        public override void OnEnd()
        {
            m_SelectedTask?.OnEnd();
            m_SelectedTask = null;
            base.OnEnd();
        }
    }
}