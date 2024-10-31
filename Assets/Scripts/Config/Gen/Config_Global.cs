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
							PlayerPunchSfx = @"Music/Punch",
							PlayerBeHitSfxs = new()
							{
								@"Music/受击1",@"Music/受击2",
							},
							LevelNames = new()
							{
								@"PunchPeng",@"PunchPeng_Caodi",
							},
							LevelConfig = new Dictionary<String, LevelConfig>
							{
								{@"PunchPeng", new LevelConfig {
									Camera = @"Level/PunchpengCamera",
									BGMRes = @"Music/MM/Town.Bar",
								}},
								{@"PunchPeng_Caodi", new LevelConfig {
									Camera = @"Level/PunchpengCamera",
									BGMRes = @"Music/MM/Town.Village",
								}},
							},
							BeHitVfx = @"Vfx/BeHitVfx",
							WinnerVfx = @"Vfx/WinnderVfx",
							WinSfx = @"Music/MM/Battle.Win",
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
			public String PlayerPunchSfx {get;set;}
			public List<String> PlayerBeHitSfxs {get;set;}
			public List<String> LevelNames {get;set;}
			public Dictionary<String, LevelConfig> LevelConfig {get;set;}
			public String BeHitVfx {get;set;}
			public String WinnerVfx {get;set;}
			public String WinSfx {get;set;}
		}
		public partial class LevelConfig
		{
			public String Camera {get;set;}
			public String BGMRes {get;set;}
		}

    }
}