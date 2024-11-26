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

            LevelCfg = new object[]
            {
                new {
                    Scene = "PunchPeng_Caodi",
                    BGMRes = "Music/MM/Town.Village",
                },
                new {
                    Scene = "PunchPeng",
                    BGMRes = "Music/MM/Town.Bar",
                },
                new {
                    Scene = "PunchPeng_Gym",
                    BGMRes = "Music/MM/Town.GoodbyeMinchi",
                    BuffIds = new []{ 1,}
                },
                //new {
                //    Scene = "PunchPeng_Gym",
                //    BGMRes = "Music/MM/Town.GoodbyeMinchi",
                //    DisableBevTreeAttak = true,
                //    CountdownAttackInterval = 20,
                //    CountdownTime = 3,
                //}
            },

            Sfx = new
            {
                PlayerPunchSfx = "Music/Punch",
                PlayerBeHitSfxs = new[] { "Music/受击1", "Music/受击2" },
                WinSfx = "Music/MM/Battle.Win",
                CoundDown321Sfx = "Music/321",
            },
            Vfx = new
            {
                BeHitVfx = "Vfx/BeHitVfx",
                WinnerVfx = "Vfx/WinnderVfx",
            },
        };
    }
}