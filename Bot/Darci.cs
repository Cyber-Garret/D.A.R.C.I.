using System;
using System.Threading;
using System.Threading.Tasks;

using Bot.Entity;
using Bot.Services;
using Bot.Services.Storage;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bot
{
	public class Darci : BackgroundService
	{
		private readonly IServiceProvider _service;
		private readonly IConfiguration _config;
		private readonly ILogger<Darci> _logger;
		private readonly DiscordSocketClient _discord;

		public Darci(IServiceProvider service, IConfiguration config, ILogger<Darci> logger, DiscordSocketClient discord)
		{
			_service = service;
			_config = config;
			_logger = logger;
			_discord = discord;
		}


		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				//Initialize service
				_service.GetRequiredService<DiscordLogging>().Configure();
				_service.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
				await _service.GetRequiredService<CommandHandler>().ConfigureAsync();

				await _discord.LoginAsync(TokenType.Bot, _config["Bot:Token"]);
				await _discord.StartAsync();
				await _discord.SetStatusAsync(UserStatus.Online);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message, "Start D.A.R.C.I.");
				throw;
			}
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			try
			{
				await _discord.SetStatusAsync(UserStatus.Offline);
				await _discord.LogoutAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message, "Stop D.A.R.C.I.");
				throw;
			}
		}
	}
}
