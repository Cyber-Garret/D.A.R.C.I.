using System;
using System.Threading.Tasks;

using Discord.WebSocket;

using Microsoft.Extensions.Logging;

using Neiralink;

using Quartz;

using Vex;

namespace Chrono.Jobs
{
	[DisallowConcurrentExecution]
	public class XurArrive : IJob
	{
		private readonly ILogger<XurArrive> _logger;
		private readonly DiscordSocketClient _discord;
		private readonly IGuildConfig _guildConfig;

		public XurArrive(ILogger<XurArrive> logger, DiscordSocketClient discord, IGuildConfig guildConfig)
		{
			_logger = logger;
			_discord = discord;
			_guildConfig = guildConfig;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				//Get guild config
				var guildConfig = await _guildConfig.GetGuildConfig();

				//We have saved text channel?
				if (guildConfig.NotificationChannelId == 0) return;

				//Write friday message
				await _discord.GetGuild(guildConfig.Id).GetTextChannel(guildConfig.NotificationChannelId)
					.SendMessageAsync(embed: XurEmbeds.XurArriveEmbed());

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Xur Arrive Job");
			}
		}
	}
}
