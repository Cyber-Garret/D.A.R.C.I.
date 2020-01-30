using Bot.Entity;

using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Bot.Services
{
	/// <summary>
	/// Provides service for handle bot command.
	/// </summary>
	internal class CommandHandler
	{
		private readonly IServiceProvider _service;
		private readonly DiscordSocketClient _discord;
		private CommandService _command;
		private readonly BotConfig _config;
		public CommandHandler(IServiceProvider service)
		{
			_service = service;
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_command = service.GetRequiredService<CommandService>();
			_config = service.GetRequiredService<IOptions<BotConfig>>().Value;
		}

		internal async Task ConfigureAsync()
		{
			_command = new CommandService(new CommandServiceConfig
			{
				DefaultRunMode = RunMode.Async,
				CaseSensitiveCommands = false
			});
			await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
		}

		internal async Task HandleCommandAsync(SocketMessage message)
		{
			// ignore if not SocketUserMessage
			if (!(message is SocketUserMessage msg)) return;

			var context = new SocketCommandContext(_discord, msg);

			var argPos = 0;
			// ignore if command not start from prefix
			if (!msg.HasStringPrefix(_config.Prefix, ref argPos)) return;

			// search command
			var cmdSearchResult = _command.Search(context, argPos);
			// if command not found just finish Task
			if (cmdSearchResult.Commands == null) return;
			//Execute command in current discord context
			var executionTask = _command.ExecuteAsync(context, argPos, _service);

			await executionTask.ContinueWith(task =>
			{
				// if Success or command unknown just finish Task
				if (task.Result.IsSuccess || task.Result.Error == CommandError.UnknownCommand) return;

				context.Channel.SendMessageAsync($"{context.User.Mention} Ошибка: {task.Result.ErrorReason}");
			});
		}
	}
}
