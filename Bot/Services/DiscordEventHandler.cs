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
		private readonly RaidStorage _raidStorage;
		private readonly IEmote Plus = new Emoji(@"➕");

		public DiscordEventHandler(IServiceProvider service)
		{
			_logger = service.GetRequiredService<ILogger<DiscordEventHandler>>();
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_command = service.GetRequiredService<CommandHandler>();
			_raidStorage = service.GetRequiredService<RaidStorage>();
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
				await MilestoneReactionAdded(cacheable, reaction);
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

		private async Task MilestoneReactionAdded(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();
				//get milestone
				var milestone = _raidStorage.GetRaid(msg.Id);

				if (milestone == null) return;

				if (reaction.Emote.Equals(Plus))
				{
					//check reaction
					var UserExist = milestone.Members.Any(u => u == reaction.UserId);

					if (!UserExist && milestone.Members.Count + 1 < milestone.Info.MaxSpace)
					{
						milestone.Members.Add(reaction.UserId);
						_raidStorage.SaveRaids(msg.Id);
						
						HandleReaction(msg, milestone);
					}
					else
					{
						var user = _discord.GetUser(reaction.UserId);
						await msg.RemoveReactionAsync(Plus, user);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Reaction Added in Milestone");
			}
		}

		private async void HandleReaction(IUserMessage message, Raid activeRaid)
		{
			var newEmbed = embed.MilestoneRebuild(_discord, activeMilestone, _emote.Raid);
			if (newEmbed.Length != 0)
				await message.ModifyAsync(m => m.Embed = newEmbed);
			if (activeMilestone.Milestone.MaxSpace == activeMilestone.MilestoneUsers.Count + 1 && activeMilestone.DateExpire < DateTime.Now.AddMinutes(15))
			{
				await message.RemoveAllReactionsAsync();
				await message.ModifyAsync(c => c.Embed = EmbedsHelper.MilestoneEnd(_discord, activeMilestone));

				await RaidNotificationAsync(activeMilestone, RemindType.FullCount);
			}
		}
	}
}
