using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services
{
	internal class GuildEventHandler
	{
		private readonly DiscordSocketClient _discord;
		private readonly CommandHandler _command;

		public GuildEventHandler(IServiceProvider service)
		{
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_command = service.GetRequiredService<CommandHandler>();
		}

		internal void InitDiscordEvents()
		{
			_discord.MessageReceived += _discord_MessageReceived;
		}

		private Task _discord_MessageReceived(SocketMessage msg)
		{
			// ignore messages from bots
			if (msg.Author.IsBot) return Task.CompletedTask;

			Task.Run(async () =>
			{
				await _command.HandleCommandAsync(msg);
			});
			return Task.CompletedTask;
		}
	}
}
