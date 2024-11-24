namespace PunchPeng
{
    public class BevParallel : BevComposites
    {
        private bool m_WaitOne;

        public void SetWaitOne()
        {
            m_WaitOne = true;
        }

        public override void OnStart()
        {
            base.OnStart();

            foreach (var item in m_Tasks)
            {
                item.OnStart();
            }
        }


        public override TaskStatus OnUpdate(float deltaTime)
        {
            var allEnd = true;
            var oneEnd = false;
            var oneEndStatus = TaskStatus.Success;

            foreach (var item in m_Tasks)
            {
                var status = item.OnUpdate(deltaTime);
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

        public override void OnEnd()
        {
            foreach (var item in m_Tasks)
            {
                item.OnEnd();
            }

            base.OnEnd();
        }
    }
}