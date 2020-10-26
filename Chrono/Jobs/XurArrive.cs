using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neiralink;
using Quartz;

namespace Chrono.Jobs
{
	[DisallowConcurrentExecution]
	public class XurArrive : IJob
	{
		private readonly ILogger<XurArrive> _logger;
		private readonly DiscordSocketClient _discord;
		private readonly NeiraDatabase _neira;

		public XurArrive(ILogger<XurArrive> logger, DiscordSocketClient discord, NeiraDatabase neira)
		{
			_logger = logger;
			_discord = discord;
			_neira = neira;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				var channel = _neira.Guilds.First().NotificationChannelId;

				if (GuildConfig.settings.NotificationChannel != 0)
				{
					var newsChannel = _discord.Guilds.FirstOrDefault().GetTextChannel(GuildConfig.settings.NotificationChannel);
					await newsChannel.SendMessageAsync(GuildConfig.settings.GlobalMention, embed: Embeds.XurArrive());
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Xur Arrive Job");
			}
		}
	}
}
