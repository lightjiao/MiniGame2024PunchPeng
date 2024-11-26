namespace PunchPeng
{
    public class BevSelector : BevComposites
    {
        private BevTask m_SelectedTask;

        public override void OnBevStart()
        {
            base.OnBevStart();
            m_SelectedTask ??= m_Tasks.RandomOne();
            m_SelectedTask?.OnBevStart();
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            return m_SelectedTask == null ? TaskStatus.Success : m_SelectedTask.OnBevUpdate(deltaTime);
        }

        public override void OnBevEnd()
        {
            m_SelectedTask?.OnBevEnd();
            m_SelectedTask = null;
            base.OnBevEnd();
        }
    }
}