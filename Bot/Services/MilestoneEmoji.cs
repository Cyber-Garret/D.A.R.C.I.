using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bot.Services
{
	/// <summary>
	/// Provides service for use server emoji in bot commands
	/// </summary>
	internal class MilestoneEmoji
	{
		internal IEmote Raid = null;
		internal IEmote ReactSecond => new Emoji("2\u20e3");
		internal IEmote ReactThird => new Emoji("3\u20e3");
		internal IEmote ReactFourth => new Emoji("4\u20e3");
		internal IEmote ReactFifth => new Emoji("5\u20e3");
		internal IEmote ReactSixth => new Emoji("6\u20e3");

		private readonly DiscordSocketClient _discord;
		public MilestoneEmoji(DiscordSocketClient discord)
		{
			_discord = discord;
		}

		internal void Configure()
		{
			var home = _discord.Guilds.First();
			Raid = home.Emotes.First(e => e.Name == "Bot_Raid");
		}
	}
}
