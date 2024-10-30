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
							TotalPlayerCount = 30,
							PlayerPrefab = @"Character/Kelly",
							PlayerPunchFrame = 8,
							PlayerPunchSfx = @"Music/Punch",
							PlayerBeHitSfx1 = @"Music/受击1",
							PlayerBeHitSfx2 = @"Music/受击2",
							LevelTestScene = @"TestLevel",
							LevelPunchPengScene = @"PunchPeng",
							LevelPunchPengBGM = @"Music/动感.wav",
							LevelConfig = new Dictionary<String, LevelConfig>
							{
								{@"PunchPeng", new LevelConfig {
									Camera = @"Level/PunchpengCamera",
								}},
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
			public String PlayerPunchSfx {get;set;}
			public String PlayerBeHitSfx1 {get;set;}
			public String PlayerBeHitSfx2 {get;set;}
			public String LevelTestScene {get;set;}
			public String LevelPunchPengScene {get;set;}
			public String LevelPunchPengBGM {get;set;}
			public Dictionary<String, LevelConfig> LevelConfig {get;set;}
		}
		public partial class LevelConfig
		{
			public String Camera {get;set;}
		}

    }
}