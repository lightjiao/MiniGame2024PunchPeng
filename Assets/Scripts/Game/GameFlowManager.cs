namespace PunchPeng
{
    public class GameFlowController : Singleton<GameFlowController>
    {
        private int m_CurLevel;

        protected override void OnInit()
        {
        }

        public void GameFlowStart()
        {
            m_CurLevel = 0;
        }
    }
}