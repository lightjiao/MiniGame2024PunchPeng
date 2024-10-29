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
							LevelTestScene = @"TestLevel",
							LevelPunchPengScene = @"PunchPeng",
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
			public String PlayerPrefab {get;set;}
			public String LevelTestScene {get;set;}
			public String LevelPunchPengScene {get;set;}
			public Dictionary<String, LevelConfig> LevelConfig {get;set;}
		}
		public partial class LevelConfig
		{
			public String Camera {get;set;}
		}

    }
}