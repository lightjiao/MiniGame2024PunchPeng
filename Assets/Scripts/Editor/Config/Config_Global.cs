using System.Collections.Generic;

namespace ConfigAuto
{
    public partial class Config_Global
    {
        public object data = new
        {
            TargetFrameRate = 60,
            TotalPlayerCount = 30,

            PlayerPrefab = "Character/Kelly",

            LevelTestScene = "TestLevel",
            LevelPunchPengScene = "PunchPeng",

            LevelConfig = new Dictionary<string, object>() {
            {
                "PunchPeng" , new { Camera = "Level/PunchpengCamera"}},
            }
        };
    }
}