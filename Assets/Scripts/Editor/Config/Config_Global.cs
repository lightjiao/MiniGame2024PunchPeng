using System.Collections.Generic;

namespace ConfigAuto
{
    public partial class Config_Global
    {
        public object data = new
        {
            TargetFrameRate = 60,
            TotalPlayerCount = 20,

            PlayerPrefab = "Character/Kelly",
            PlayerPunchFrame = 8, // 动画资源是按照30帧的第4帧算伤害
            PlayerPunchSfx = "Music/Punch",
            PlayerBeHitSfxs = new[] { "Music/受击1", "Music/受击2" },

            LevelNames = new[] { "PunchPeng", "PunchPeng_Caodi" },

            LevelConfig = new Dictionary<string, object>()
            {
                {
                    "PunchPeng", new
                    {
                        Camera = "Level/PunchpengCamera",
                        BGMRes = "Music/MM/Town.Bar",
                    }
                },
                {
                    "PunchPeng_Caodi", new
                    {
                        Camera = "Level/PunchpengCamera",
                        BGMRes = "Music/MM/Town.Village",
                    }
                },
            },

            BeHitVfx = "Vfx/BeHitVfx",
            WinnerVfx = "Vfx/WinnderVfx",
            WinSfx = "Music/MM/Battle.Win",
        };
    }
}