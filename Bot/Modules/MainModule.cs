using Bot.Core;
using Bot.Entity;
using Bot.Entity.Milestone;
using Bot.Helpers;

using Discord;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using Discord.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.Services.Storage;
using System.Globalization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Bot.Services;

namespace Bot.Modules
{
	public class MainModule : InteractiveBase
	{
		private readonly DiscordSocketClient _discord;
		private readonly MilestoneInfoStorage _infoStorage;
		private readonly MilestoneHandler _milestoneHandler;
		private readonly RaidStorage _raidStorage;
		private readonly CommandService _command;
		private readonly BotConfig _config;
		private readonly ILogger _logger;
		public MainModule(CommandService command, IServiceProvider service)
		{
			_discord = service.GetRequiredService<DiscordSocketClient>();
			_command = command;
			_config = service.GetRequiredService<IOptions<BotConfig>>().Value;
			_infoStorage = service.GetRequiredService<MilestoneInfoStorage>();
			_raidStorage = service.GetRequiredService<RaidStorage>();
			_logger = service.GetRequiredService<ILogger<MainModule>>();
		}
		[Command("help")]
		[Summary("Provides list of available commands.")]
		public async Task MainHelp()
		{
			var mainCommands = string.Empty;
			var adminCommands = string.Empty;
			var economyCommands = string.Empty;
			var selfRoleCommands = string.Empty;

			foreach (CommandInfo command in _command.Commands.ToList())
			{
				if (command.Module.Name == typeof(MainModule).Name)
					mainCommands += $"{_config.Prefix}{command.Name}, ";
				//else if (command.Module.Name == typeof(ModerationModule).Name)
				//	adminCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";
				//else if (command.Module.Name == typeof(EconomyModule).Name)
				//	economyCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";
				//else if (command.Module.Name == typeof(SelfRoleModule).Name)
				//	selfRoleCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";

			}

			var embed = new EmbedBuilder()
				.WithColor(Color.Green)
				.WithTitle("Thank you for using the Data Analysis, Reconnaissance, and Cooperative Intelligence bot. You may call me Darci.")
				.WithDescription(
				"Good day. My main target is push message if Xur arrive in solar system.\n" +
				"I can also provide information about exotic equipment and catalysts.\n" +
				$"[More info in online Docs]({Constants.Docs})");

			embed.AddField("Main commands", mainCommands[0..^2]);
			//embed.AddField("Admin commands", adminCommands[0..^2]);
			//embed.AddField("Self-Role commands", selfRoleCommands[0..^2]);

			await ReplyAsync(embed: embed.Build());
		}

		[Command("info")]
		[Summary("Provides a full info about command.")]
		[Remarks("Ex: !info <command>, like !info exotic")]
		public async Task Info([Remainder] string searchCommand = null)
		{
			if (searchCommand == null)
			{
				await ReplyAndDeleteAsync(":x: Please enter in full or in part the name of the command about which you want help.");
				return;
			}
			var command = _command.Commands.Where(c => c.Name.IndexOf(searchCommand, StringComparison.CurrentCultureIgnoreCase) != -1).FirstOrDefault();
			if (command == null)
			{
				await ReplyAndDeleteAsync($":x: Not found any info about **{searchCommand}**. ");
				return;
			}
			var embed = new EmbedBuilder();

			embed.WithAuthor(_discord.CurrentUser);
			embed.WithTitle($"Info about command: {command.Name}");
			embed.WithColor(Color.Gold);
			embed.WithDescription(command.Summary ?? "No description.");
			// First alias always command atribute
			if (command.Aliases.Count > 1)
			{
				string alias = string.Empty;
				//Skip command attribute
				foreach (var item in command.Aliases.Skip(1))
				{
					alias += $"{_config.Prefix}{item}, ";
				}
				embed.AddField("Command alias:", alias[0..^2]);
			}
			if (command.Remarks != null)
				embed.AddField("Remark:", command.Remarks);

			await ReplyAsync(embed: embed.Build());
		}

		[Command("exotic")]
		[Summary("Provide information about exotic gear. Can search by partial and full name")]
		[Remarks("Ex: !exotic sturm")]
		public async Task FindExotic([Remainder] string Input = null)
		{
			if (Input == null)
			{
				await ReplyAndDeleteAsync(":x: Please enter the full or partial name of the exotic equipment.");
				return;
			}

			var exotic = ExoticStorage.GetExotic(Input);

			if (exotic == null)
			{
				await ReplyAndDeleteAsync(":x: Sorry i don't know about this equipment.");
				return;
			}

			await ReplyAsync($"So, {Context.User.Username} this is what i know about this equipment.", embed: Embeds.BuildedExotic(exotic));
		}

		[Command("raid")]
		[Summary("Command for announcing raid, for register clan mates with inline reaction.")]
		[Remarks("!raid <name> <reserved place> <time> <memo(can be empty)>\nEx: !raid lw 03.02.20.00 ")]
		public async Task GoRaid(string milestoneName, byte reserved, string raidTime, [Remainder]string memo = null)
		{
			try
			{
				var milestone = _infoStorage.SearchMilestone(milestoneName, MilestoneType.Raid);

				if (milestone == null)
				{
					var AvailableRaids = "Доступные для регистрации рейды:\n\n";
					var raids = _infoStorage.GetAllRaids();

					foreach (var raid in raids)
					{
						AvailableRaids += $"**{raid.Name}** или просто **{raid.Alias}**\n";
					}

					var message = new EmbedBuilder()
						.WithTitle("Страж, я не разобрала в какую активность ты хочешь пойти")
						.WithColor(Color.Red)
						.WithDescription(AvailableRaids += "\nПример: !raid пж")
						.WithFooter("Хочу напомнить, что я ищу как по полному названию рейда так и частичному. Это сообщение будет автоматически удалено через 2 минуты.");
					await ReplyAndDeleteAsync(null, embed: message.Build(), timeout: TimeSpan.FromMinutes(2));
					return;
				}

				string[] formats = { "dd.MM-HH:mm", "dd,MM-HH,mm", "dd.MM.HH.mm", "dd,MM,HH,mm" };

				DateTime.TryParseExact(raidTime, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

				if (dateTime == new DateTime())
				{
					var message = new EmbedBuilder()
						.WithTitle("Страж, ты указал неизвестный мне формат времени")
						.WithColor(Color.Gold)
						.AddField("Я понимаю время начала рейда в таком формате",
						"Формат времени: **<день>.<месяц>-<час>:<минута>**\n" +
						"**День:** от 01 до 31\n" +
						"**Месяц:** от 01 до 12\n" +
						"**Час:** от 00 до 23\n" +
						"**Минута:** от 00 до 59\n" +
						"В итоге у тебя должно получиться: **05.07-20:05** Пример: !сбор пж 21.05-20:00")
						.AddField("Уведомление", "Время начала активности учитывается только по московскому времени. Также за 15 минут до начала активности, я уведомлю участников личным сообщением.")
						.WithFooter("Это сообщение будет автоматически удалено через 2 минуты.");
					await ReplyAndDeleteAsync(null, embed: message.Build(), timeout: TimeSpan.FromMinutes(2));
					return;
				}
				if (dateTime < DateTime.Now)
				{
					var message = new EmbedBuilder()
						.WithColor(Color.Red)
						.WithDescription($"Собрался в прошлое? Тебя ждет увлекательное шоу \"остаться в живых\" в исполнении моей команды Золотого Века. Не забудь попкорн\nБип...Удачи и передай привет моему капитану.");
					await ReplyAndDeleteAsync(null, embed: message.Build());
					return;
				}

				var newRaid = new Raid
				{
					Info = milestone,
					DateExpire = dateTime,
					Memo = memo,
				};
				newRaid.Members.Add(Context.User.Id);
				newRaid.Info.MaxSpace -= reserved;
				var msg = await ReplyAsync(message: GuildConfig.settings.GlobalMention, embed: MilestoneEmbed(newRaid, _milestoneHandler.Plus));

				newRaid.MessageId = msg.Id;
				_raidStorage.AddRaid(newRaid);
				//Slots
				await msg.AddReactionAsync(_milestoneHandler.Plus);
			}
			catch (Exception ex)
			{
				//reply to user if any error
				await ReplyAndDeleteAsync($"Страж, произошла критическая ошибка, я не могу в данный момент выполнить команду.\nMessage: {ex.Message}.");
				//Log full exception in console
				_logger.LogCritical(ex, "Milestone command");
			}
		}

		private Embed MilestoneEmbed(Raid raid, IEmote raidEmote = null)
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


			embed.WithFooter($"Чтобы за вами закрепили место нажмите на реакцию - {raidEmote}");

			return embed.Build();
		}
	}
}
