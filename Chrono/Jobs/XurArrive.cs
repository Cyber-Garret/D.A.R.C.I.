﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Chrono.Jobs
{
	[DisallowConcurrentExecution]
	public class XurArrive : IJob
	{
		private readonly ILogger _logger;
		private readonly DiscordSocketClient _discord;
		public XurArrive(IServiceProvider service)
		{
			_logger = service.GetRequiredService<ILogger<XurArrive>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
		}
		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
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