namespace PunchPeng
{
    public class BevParallel : BevComposites
    {
        private bool m_WaitOne;

        public void SetWaitOne()
        {
            m_WaitOne = true;
        }

        public override void OnBevStart()
        {
            base.OnBevStart();

            foreach (var item in m_Tasks)
            {
                item.OnBevStart();
            }
        }


        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            var allEnd = true;
            var oneEnd = false;
            var oneEndStatus = TaskStatus.Success;

            foreach (var item in m_Tasks)
            {
                var status = item.OnBevUpdate(deltaTime);
                if (m_WaitOne)
                {
                    oneEnd = true;
                    oneEndStatus = status;
                    break;
                }
                else
                {
                    if (status == TaskStatus.Running)
                    {
                        allEnd = false;
                    }
                }
            }

            if (m_WaitOne && oneEnd)
            {
                return oneEndStatus;
            }
            if (!m_WaitOne && allEnd)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        public override void OnBevEnd()
        {
            foreach (var item in m_Tasks)
            {
                item.OnBevEnd();
            }

            base.OnBevEnd();
        }
    }
}