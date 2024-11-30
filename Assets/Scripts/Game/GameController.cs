using ConfigAuto;
using System.Collections.Generic;

namespace PunchPeng
{
    /// <summary>
    /// 整个游戏的 controller，具体某一关由LevelController 管理
    /// </summary>
    public class GameController : Singleton<GameController>
    {
        public int CurLevel { get; private set; }
        private List<int> LevelListIdx = new();

        protected override void OnInit()
        {
        }

        public int ChooseRandomLevelId()
        {
            if (LevelListIdx.Count == 0)
            {
                Config_Global.Inst.data.LevelCfg.KeysCopyTo(LevelListIdx);
            }

            var loopCnt = 100;
            var newLevel = -1;
            do
            {
                newLevel = LevelListIdx.RandomOne();
                loopCnt--;

            } while (CurLevel == newLevel && loopCnt > 0);

            CurLevel = newLevel;
            LevelListIdx.Remove(newLevel);

            return CurLevel;
        }

        public void ChooseLevelTest()
        {
            var list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                var curLevel = CurLevel;
                ChooseRandomLevelId();
                list.Add(CurLevel);
                if (curLevel == CurLevel)
                {
                    Log.Error("关卡选择测试错误");
                }
            }
            Log.Info(list.ToStringEx());
        }
    }
}