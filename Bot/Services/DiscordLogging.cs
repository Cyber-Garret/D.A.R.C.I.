using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Bot.Services
{
	/// <summary>
	/// Provides logging for Discord.Net's API
	/// </summary>
	internal class DiscordLogging
	{
		// declare the fields used later in this class
		private readonly ILogger<DiscordLogging> _logger;
		private readonly DiscordSocketClient _discord;
		private readonly CommandService _commands;

		public DiscordLogging(ILogger<DiscordLogging> logger, DiscordSocketClient discord, CommandService commands)
		{
			_logger = logger;
			_discord = discord;
			_commands = commands;
		}


		public void Configure()
		{
			// hook into these events with the methods provided below
			_discord.Ready += OnReadyAsync;
			_discord.Log += OnLogAsync;
			_discord.Disconnected += OnDisconnectedAsync;
			_commands.Log += OnLogAsync;
		}

		// this method executes on the bot being connected/ready
		private Task OnReadyAsync()
		{
			_logger.LogInformation($"Connected as -> [{_discord.CurrentUser}] :)");
			return Task.CompletedTask;
		}

		// this method executes on the bot being disconnected from Discord API
		private Task OnDisconnectedAsync(Exception ex)
		{
			_logger.LogInformation($"Bot disconnected. [{ex.Message}]");
			return Task.CompletedTask;
		}

		// this method switches out the severity level from Discord.Net's API, and logs appropriately
		private Task OnLogAsync(LogMessage message)
		{
			var logText = $"{message.Source}: {message.Message}";
			switch (message.Severity)
			{
				case LogSeverity.Critical:
					_logger.LogCritical(logText);
					break;
				case LogSeverity.Error:
					_logger.LogError(logText);
					break;
				case LogSeverity.Warning:
					_logger.LogWarning(logText);
					break;
				case LogSeverity.Info:
					_logger.LogInformation(logText);
					break;
				case LogSeverity.Verbose:
					_logger.LogTrace(logText);
					break;
				case LogSeverity.Debug:
					_logger.LogDebug(logText);
					break;
				default:
					_logger.LogInformation(logText);
					break;
			}

			return Task.CompletedTask;
		}
	}
}
