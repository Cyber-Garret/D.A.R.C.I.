using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Bot.Services.Storage;
using System.Linq;
using Microsoft.Extensions.Logging;
using Bot.Entity.Milestone;

namespace Bot.Services
{
	internal class DiscordEventHandler
	{
		private readonly ILogger _logger;
		private readonly DiscordSocketClient _discord;
		private readonly CommandHandler _command;
		private readonly MilestoneHandler _milestoneHandler;
		private readonly IEmote Plus = new Emoji(@"➕");

		public DiscordEventHandler(IServiceProvider service)
		{
			_logger = service.GetRequiredService<ILogger<DiscordEventHandler>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_command = service.GetRequiredService<CommandHandler>();
			_milestoneHandler = service.GetRequiredService<MilestoneHandler>();
		}

		internal void InitDiscordEvents()
		{
			_discord.MessageReceived += _discord_MessageReceived;
			//Reactions
			_discord.ReactionAdded += _discord_ReactionAdded;
			_discord.ReactionRemoved += _discord_ReactionRemoved;
		}

		private Task _discord_ReactionRemoved(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
		{
			throw new NotImplementedException();
		}

		private Task _discord_ReactionAdded(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel channel, SocketReaction reaction)
		{
			Task.Run(async () =>
			{
				await _milestoneHandler.MilestoneReactionAdded(cacheable, reaction);
			});
			return Task.CompletedTask;
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
