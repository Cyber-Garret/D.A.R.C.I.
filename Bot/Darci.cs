using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bot.Entity;
using Bot.Services;
using Bot.Services.Storage;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot
{
	public class Darci : BackgroundService
	{
		private readonly IServiceProvider _service;
		private readonly ILogger<Darci> _logger;
		private readonly DiscordSocketClient _discord;
		private readonly BotConfig _config;
		private readonly RaidStorage _raidStorage;

		public Darci(IServiceProvider service)
		{
			_service = service;
			_logger = service.GetRequiredService<ILogger<Darci>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_config = service.GetRequiredService<IOptions<BotConfig>>().Value;
			_raidStorage = service.GetRequiredService<RaidStorage>();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			//Initialize service
			_service.GetRequiredService<DiscordLogging>().Configure();
			_service.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
			await _service.GetRequiredService<CommandHandler>().ConfigureAsync();

			await _discord.LoginAsync(TokenType.Bot, _config.Token);
			await _discord.StartAsync();
			await _discord.SetStatusAsync(UserStatus.Online);
			await Task.Delay(-1, stoppingToken);
		}
		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			_raidStorage.SaveRaids();
			await _discord.SetStatusAsync(UserStatus.Offline);
			await _discord.LogoutAsync();
		}
	}
}
