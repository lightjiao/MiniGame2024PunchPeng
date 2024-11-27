namespace ConfigAuto
{
    public partial class Config_Global
    {
        public object data = new
        {
            TargetFrameRate = 60,

            PlayerPrefab = "Character/Kelly",
            PlayerPunchFrame = 8, // 动画资源是按照30帧的第4帧算伤害

            LevelCfg = new object[]
            {
                new {
                    Scene = "PunchPeng_Caodi",
                    PreloadImg = "UI/Loading_Caodi",
                    BGMRes = "Music/MM/Town.Village",
                    PawnCount = 40,
                    AIAttackPct = 0.5,
                },
                new {
                    Scene = "PunchPeng",
                    PreloadImg = "UI/Loading",
                    BGMRes = "Music/MM/Town.Bar",
                    PawnCount = 20,
                    AIAttackPct = 0.2,
                },
                new {
                    Scene = "PunchPeng_Gym",
                    PreloadImg = "UI/Loading_Gym",
                    BGMRes = "Music/MM/Town.GoodbyeMinchi",
                    BuffIds = new []{ 1,},
                    PawnCount = 20,
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