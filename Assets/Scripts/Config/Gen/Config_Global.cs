//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
namespace ConfigAuto
{
	public partial class Config_Global
	{
		private static Config_Global _inst;
		public static Config_Global Inst
		{
			get
			{
				if (_inst == null)
					_inst = new()
					{
						data = new()
						{
							TargetFrameRate = 60,
							PlayerPrefab = @"Character/Kelly",
							PlayerPunchFrame = 8,
							EveryLevelPlayerBuffs = new()
							{
								2,
							},
							LevelCfg = new()
							{

								new()
								{
									Scene = @"PunchPeng_Caodi",
									PreloadImg = @"UI/Loading_Caodi",
									BGMRes = @"Music/MM/Town.Village",
									PawnCount = 40,
								},
								new()
								{
									Scene = @"PunchPeng",
									PreloadImg = @"UI/Loading",
									BGMRes = @"Music/MM/Town.Bar",
									PawnCount = 20,
									AIAttackPct = 0.1f,
								},
								new()
								{
									Scene = @"PunchPeng_Gym",
									PreloadImg = @"UI/Loading_Gym",
									BGMRes = @"Music/MM/Town.GoodbyeMinchi",
									PawnCount = 20,
									LevelBuffs = new()
									{
										1,
									},
								},
								new()
								{
									Scene = @"PunchPeng_Xuedi",
									PreloadImg = @"UI/Loading_Gym",
									BGMRes = @"Music/MM/Town.Dance",
									PawnCount = 20,
									PlayerBuffs = new()
									{
										3,
									},
									AIAttackPct = 0.2f,
								},
							},
							Sfx = new()
							{
								PlayerPunchSfx = @"Music/Punch",
								PlayerBeHitSfxs = new()
								{
									@"Music/受击1",@"Music/受击2",
								},
								WinSfx = @"Music/MM/Battle.Win",
								CoundDown321Sfx = @"Music/321",
							},
							Vfx = new()
							{
								BeHitVfx = @"Vfx/BeHitVfx",
								WinnerVfx = @"Vfx/WinnderVfx",
							},
						},
					};
				return _inst;
			}
		}
		public Rootdata data {get;set;}
		public partial class Rootdata
		{
			public Int32 TargetFrameRate {get;set;}
			public String PlayerPrefab {get;set;}
			public Int32 PlayerPunchFrame {get;set;}
			public List<Int32> EveryLevelPlayerBuffs {get;set;}
			public List<LevelCfg> LevelCfg {get;set;}
			public Sfx Sfx {get;set;}
			public Vfx Vfx {get;set;}
		}
		public partial class LevelCfg
		{
			public String Scene {get;set;}
			public String PreloadImg {get;set;}
			public String BGMRes {get;set;}
			public Int32 PawnCount {get;set;}
			public float AIAttackPct {get;set;}
			public List<Int32> LevelBuffs {get;set;}
			public List<Int32> PlayerBuffs {get;set;}
		}
		public partial class Sfx
		{
			public String PlayerPunchSfx {get;set;}
			public List<String> PlayerBeHitSfxs {get;set;}
			public String WinSfx {get;set;}
			public String CoundDown321Sfx {get;set;}
		}
		public partial class Vfx
		{
			public String BeHitVfx {get;set;}
			public String WinnerVfx {get;set;}
		}

    }
}