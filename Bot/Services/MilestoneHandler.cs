using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Bot.Entity.Milestone;
using System.Threading.Tasks;
using Bot.Services.Storage;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Bot.Services
{
	public class MilestoneHandler
	{
		public readonly IEmote Plus;
		private readonly DiscordSocketClient discord;
		private readonly RaidStorage raidStorage;
		private readonly ILogger logger;
		public MilestoneHandler(IServiceProvider service)
		{
			Plus = new Emoji(@"➕");
			discord = service.GetRequiredService<DiscordSocketClient>();
			raidStorage = service.GetRequiredService<RaidStorage>();
			logger = service.GetRequiredService<ILogger<MilestoneHandler>>();
		}

		public async Task MilestoneReactionAdded(Cacheable<IUserMessage, ulong> cache, SocketReaction reaction)
		{
			try
			{
				var msg = await cache.GetOrDownloadAsync();
				//get milestone
				var raid = raidStorage.GetRaid(msg.Id);

				if (raid == null) return;

				if (reaction.Emote.Equals(Plus))
				{
					//check reaction
					var UserExist = raid.Members.Any(u => u == reaction.UserId);

					if (!UserExist && raid.Members.Count < raid.Info.MaxSpace)
					{
						raid.Members.Add(reaction.UserId);
						raidStorage.SaveRaids(msg.Id);

						HandleReaction(msg, raid);
					}
					else
					{
						var user = discord.GetUser(reaction.UserId);
						await msg.RemoveReactionAsync(Plus, user);
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Reaction Added in Milestone");
			}
		}

		private async void HandleReaction(IUserMessage message, Raid activeRaid)
		{
			var newEmbed = RaidEmbed(activeRaid);
			if (newEmbed.Length != 0)
				await message.ModifyAsync(m => m.Embed = newEmbed);
			if (activeRaid.DateExpire > DateTime.Now)
			{
				await message.RemoveAllReactionsAsync();
				raidStorage.RemoveRaid(message.Id);
			}
		}
		public async Task RaidNotificationAsync(Raid raid)
		{
			try
			{
				var Guild = discord.Guilds.FirstOrDefault();

				foreach (var user in raid.Members)
				{
					try
					{
						var LoadedUser = discord.GetUser(user);

						var DM = await LoadedUser.GetOrCreateDMChannelAsync();
						await DM.SendMessageAsync(embed: MilestoneRemindEmbed(raid));
					}
					catch (Exception ex)
					{
						logger.LogWarning(ex, "RaidNotification in DM of user");
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "RaidNotification Global");
			}

		}

		public Embed RaidEmbed(Raid raid)
		{
			var embed = new EmbedBuilder
			{
				Title = $"{raid.DateExpire.Date.ToString("dd.MM.yyyy")}, в {raid.DateExpire.ToString("HH:mm")}. {raid.Info.DisplayType}: {raid.Info.Name}",
				ThumbnailUrl = raid.Info.Icon,
				Color = Color.DarkMagenta
			};
			//Add milestone leader memo if represent
			if (!string.IsNullOrWhiteSpace(raid.Memo))
				embed.WithDescription($"**Заметка:** {raid.Memo}");

			var embedFieldUsers = new EmbedFieldBuilder
			{
				Name = $"В боевую группу записались"
			};
			int count = 1;
			foreach (var user in raid.Members)
			{
				var discordUser = discord.GetUser(user);
				embedFieldUsers.Value += $"#{count} {discordUser.Mention} - {discordUser.Username}\n";
				count++;
			}
			if (embedFieldUsers.Value != null)
				embed.AddField(embedFieldUsers);


			embed.WithFooter($"Чтобы за вами закрепили место нажмите на реакцию - {Plus}");

			return embed.Build();
		}

		private Embed MilestoneRemindEmbed(Raid raid)
		{
			var guild = discord.Guilds.FirstOrDefault();

			var authorBuilder = new EmbedAuthorBuilder
			{
				Name = $"Доброго времени суток, страж.",
				IconUrl = discord.CurrentUser.GetAvatarUrl() ?? discord.CurrentUser.GetDefaultAvatarUrl()
			};

			var embed = new EmbedBuilder()
			{
				Title = $"Хочу вам напомнить, что у вас через 15 минут начнется {raid.Info.DisplayType.ToLower()}.",
				Author = authorBuilder,
				Color = Color.DarkMagenta,
				ThumbnailUrl = raid.Info.Icon
			};
			if (raid.Memo != null)
				embed.WithDescription($"**Заметка:** {raid.Memo}");

			var embedFieldUsers = new EmbedFieldBuilder
			{
				Name = $"Состав боевой группы"
			};

			int count = 1;
			foreach (var user in raid.Members)
			{

				var discordUser = discord.GetUser(user);
				embedFieldUsers.Value += $"#{count} {discordUser.Mention} - {discordUser.Username}\n";

				count++;
			}
			if (embedFieldUsers.Value != null)
				embed.AddField(embedFieldUsers);

			embed.WithFooter($"{raid.Info.DisplayType}: {raid.Info.Name}. Сервер: {guild.Name}", guild.IconUrl);
			embed.WithCurrentTimestamp();

			return embed.Build();
		}
	}
}
