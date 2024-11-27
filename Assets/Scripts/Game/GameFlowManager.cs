using ConfigAuto;

namespace PunchPeng
{
    public class GameFlowController : Singleton<GameFlowController>
    {
        public int CurLevel { get; private set; }

        protected override void OnInit()
        {
        }

        public int ChooseLevel()
        {
            CurLevel = Config_Global.Inst.data.LevelCfg.RandomIndex();
            return CurLevel;
        }
    }
}