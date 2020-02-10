using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Bot.Entity.Milestone;

namespace Bot.Services
{
	public class MilestoneHandler
	{
		public readonly IEmote Plus;
		private readonly DiscordSocketClient _discord;
		public MilestoneHandler(IServiceProvider service)
		{
			Plus = new Emoji(@"➕");
			_discord = service.GetRequiredService<DiscordSocketClient>();
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
				var discordUser = _discord.GetUser(user);
				embedFieldUsers.Value += $"#{count} {discordUser.Mention} - {discordUser.Username}\n";
				count++;
			}
			if (embedFieldUsers.Value != null)
				embed.AddField(embedFieldUsers);


			embed.WithFooter($"Чтобы за вами закрепили место нажмите на реакцию - {Plus}");

			return embed.Build();
		}
	}
}
