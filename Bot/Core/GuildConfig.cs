using Bot.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot.Core
{
	internal class GuildConfig
	{
		private const string configFile = "guild.json";

		internal static Guild settings;

		static GuildConfig()
		{
			// if directory not exist create him
			if (!Directory.Exists(Constants.ResourceFolder))
				Directory.CreateDirectory(Constants.ResourceFolder);

			var guildPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, configFile);

			if (File.Exists(guildPath))
			{
				var json = File.ReadAllText(guildPath, Encoding.Unicode);
				settings = JsonConvert.DeserializeObject<Guild>(json);
			}
			else
			{
				settings = new Guild();
				var json = JsonConvert.SerializeObject(settings);
				File.WriteAllText(guildPath, json, Encoding.Unicode);
			}
		}
	}
}
