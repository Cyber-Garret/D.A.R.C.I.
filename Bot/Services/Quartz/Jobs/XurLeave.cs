﻿using Bot.Core;
using Bot.Helpers;

using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services.Quartz.Jobs
{
	[DisallowConcurrentExecution]
	public class XurLeave : IJob
	{
		private readonly ILogger _logger;
		private readonly DiscordSocketClient _discord;
		internal XurLeave(IServiceProvider service)
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
					await newsChannel.SendMessageAsync(GuildConfig.settings.GlobalMention, embed: Embeds.XurLeave());
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Xur Leave Job");
			}
		}
	}
}
