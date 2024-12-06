using PunchPeng;

namespace ConfigAuto
{
    public partial class Config_Global
    {
        public object data = new
        {
            TargetFrameRate = 60,

            PlayerPrefab = "Character/PlayerPrefab",
            PlayerPunchAtkTime = 0.13, // 攻击开始时间, 动画资源是按照30帧的第4帧算伤害
            PlayerPunchAtkDuration = 0.1, // 攻击判定持续时间
            WinPoint = 3, // 胜利目标分数

            EveryLevelPlayerBuffs = new[] { 2 }, // disbale attack for a period

            LevelCfg = new object[]
            {
                new {
                    Scene = "PunchPeng_Caodi",
                    PreloadImg = "UI/Loading_Caodi",
                    BGMRes = "Music/MM/Town.Village",
                    PawnCount = 40,
                },
                new {
                    Scene = "PunchPeng",
                    PreloadImg = "UI/Loading",
                    BGMRes = "Music/MM/Town.Bar",
                    PawnCount = 20,
                    AIAttackPct = 0.1,
                },
                new {
                    Scene = "PunchPeng_Gym",
                    PreloadImg = "UI/Loading_Gym",
                    BGMRes = "Music/MM/Town.GoodbyeMinchi",
                    PawnCount = 20,
                    LevelBuffs = new[]{1}, // AI 倒计时一起出拳
                },
                new {
                    Scene = "PunchPeng_Xuedi",
                    PreloadImg = "UI/Loading_Xuedi",
                    BGMRes = "Music/MM/Town.Dance",
                    PawnCount = 20,
                    PlayerBuffs = new[]{3}, // 冰面移动模拟
                    AIAttackPct = 0.2,
                    Locomotion = PlayerLocomotionType.Ice,
                }
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