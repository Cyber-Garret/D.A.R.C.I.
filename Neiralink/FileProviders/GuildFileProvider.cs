using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Neiralink.Entities;
using Newtonsoft.Json;

namespace Neiralink.FileProviders
{
	internal class GuildFileProvider : IGuildConfig
	{
		private static Guild _guild;

		public GuildFileProvider(ulong homeGuild)
		{
			// if directory not exist create him
			if (!Directory.Exists(Constants.ResourceFolder))
				Directory.CreateDirectory(Constants.ResourceFolder);

			var guildPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, $"{homeGuild}.json");

			//Read or create file and in memory guild settings
			if (File.Exists(guildPath))
			{
				var json = File.ReadAllText(guildPath, Encoding.Unicode);
				_guild = JsonConvert.DeserializeObject<Guild>(json);
			}
			else
			{
				_guild = new Guild();
				var json = JsonConvert.SerializeObject(settings);
				File.WriteAllText(guildPath, json, Encoding.Unicode);
			}
		}

		public Task<Guild> GetGuildConfig()
		{
			throw new NotImplementedException();
		}

		public Task<Guild> SaveGuildConfig()
		{
			throw new NotImplementedException();
		}
	}
}
