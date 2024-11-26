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
							TotalPlayerCount = 20,
							PlayerPrefab = @"Character/Kelly",
							PlayerPunchFrame = 8,
							LevelCfg = new()
							{

								new()
								{
									Scene = @"PunchPeng_Caodi",
									BGMRes = @"Music/MM/Town.Village",
								},
								new()
								{
									Scene = @"PunchPeng",
									BGMRes = @"Music/MM/Town.Bar",
								},
								new()
								{
									Scene = @"PunchPeng_Gym",
									BGMRes = @"Music/MM/Town.GoodbyeMinchi",
									BuffIds = new()
									{
										1,
									},
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
			public Int32 TotalPlayerCount {get;set;}
			public String PlayerPrefab {get;set;}
			public Int32 PlayerPunchFrame {get;set;}
			public List<LevelCfg> LevelCfg {get;set;}
			public Sfx Sfx {get;set;}
			public Vfx Vfx {get;set;}
		}
		public partial class LevelCfg
		{
			public String Scene {get;set;}
			public String BGMRes {get;set;}
			public List<Int32> BuffIds {get;set;}
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