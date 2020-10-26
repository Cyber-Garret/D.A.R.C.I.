using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Bot.Services
{
	internal class DiscordEventHandler
	{
		private readonly ILogger<DiscordEventHandler> _logger;
		private readonly DiscordSocketClient _discord;
		private readonly CommandHandler _command;

		public DiscordEventHandler(ILogger<DiscordEventHandler> logger, DiscordSocketClient discord, CommandHandler command)
		{
			_logger = logger;
			_discord = discord;
			_command = command;
		}


		internal void InitDiscordEvents()
		{
			_discord.MessageReceived += Discord_MessageReceived;
			//Reactions
			_discord.ReactionAdded += Discord_ReactionAdded;
			_discord.ReactionRemoved += Discord_ReactionRemoved;
		}

		private static Task Discord_ReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
		{
			throw new NotImplementedException();
		}

		private static Task Discord_ReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
		{
			throw new NotImplementedException();
		}

		private async Task Discord_MessageReceived(SocketMessage msg)
		{
			// ignore messages from bots
			if (msg.Author.IsBot) return;

			await _command.HandleCommandAsync(msg);
		}
	}
}
