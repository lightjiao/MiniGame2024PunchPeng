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
            PlayerPunchFrame = 8, // 动画资源是按照30帧的第4帧算伤害
            PlayerPunchSfx = "Music/Punch",
            PlayerBeHitSfx1 = "Music/受击1",
            PlayerBeHitSfx2 = "Music/受击2",

            LevelTestScene = "TestLevel",
            LevelPunchPengScene = "PunchPeng",

            LevelBGMTownMMBar = "Music/MM/Town.Bar",

            LevelConfig = new Dictionary<string, object>()
            {
                {
                    "PunchPeng" , new
                    {
                        Camera = "Level/PunchpengCamera",
                        BGMRes = "Music/MM/Town.Bar",
                    }
                },
                {
                    "PunchPeng_Caodi" , new
                    {
                        Camera = "Level/PunchpengCamera",
                        BGMRes = "Music/MM/Town.Village",
                    }
                },
            },
        };
    }
}