//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
namespace ConfigAuto
{
	public partial class Config_Player
	{
		private static Config_Player _inst;
		public static Config_Player Inst
		{
			get
			{
				if (_inst == null)
					_inst = new()
					{
						PlayerPrefab = @"Character/Kelly",
					};
				return _inst;
			}
		}
		public String PlayerPrefab {get;set;}

    }
}