using Bot.Core;
using Bot.Helpers;

using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules
{
	public class Main : InteractiveBase
	{
		private readonly Discord.WebSocket.DiscordSocketClient _discord;
		private readonly CommandService _command;
		private readonly Entity.BotConfig _config;
		public Main(IServiceProvider service)
		{
			_discord = service.GetRequiredService<Discord.WebSocket.DiscordSocketClient>();
			_command = service.GetRequiredService<CommandService>();
			_config = service.GetRequiredService<IOptions<Entity.BotConfig>>().Value;
		}
		[Command("справка")]
		[Summary("Основная справочная команда.")]
		public async Task MainHelp()
		{
			var app = await _discord.GetApplicationInfoAsync();

			var mainCommands = string.Empty;
			var adminCommands = string.Empty;
			var economyCommands = string.Empty;
			var selfRoleCommands = string.Empty;

			foreach (CommandInfo command in _command.Commands.ToList())
			{
				if (command.Module.Name == typeof(Main).Name)
					mainCommands += $"{_config.Prefix}{command.Name}, ";
				//else if (command.Module.Name == typeof(ModerationModule).Name)
				//	adminCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";
				//else if (command.Module.Name == typeof(EconomyModule).Name)
				//	economyCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";
				//else if (command.Module.Name == typeof(SelfRoleModule).Name)
				//	selfRoleCommands += $"{guild.CommandPrefix ?? "!"}{command.Name}, ";

			}

			var embed = new EmbedBuilder()
				.WithColor(Color.Gold)
				.WithTitle($"Доброго времени суток. Меня зовут Нейроматрица, я ИИ \"Черного исхода\" адаптированный для Discord. Успешно функционирую с {app.CreatedAt.ToString("dd.MM.yyyy")}")
				.WithDescription(
				"Моя основная цель - своевременно сообщать когда прибывает или улетает посланник девяти Зур.\n" +
				"Также я могу предоставить информацию о экзотическом снаряжении,катализаторах.\n" +
				"Больше информации ты можешь найти в моей [группе ВК](https://vk.com/failsafe_bot)\n" +
				"и в [документации](https://docs.neira.su/)")
				.AddField("Основные команды", mainCommands[0..^2])
				.AddField("Команды администраторов сервера", adminCommands[0..^2])
				.AddField("Команды экономики и репутации", economyCommands[0..^2])
				.AddField("Команды настройки Автороли", selfRoleCommands[0..^2]);

			await ReplyAsync(embed: embed.Build());
		}

		[Command("экзот")]
		[Summary("Отображает информацию о экзотическом снаряжении. Ищет как по полному названию, так и частичному.")]
		[Remarks("Пример: !экзот буря")]
		public async Task FindExotic([Remainder] string Input = null)
		{
			if (Input == null)
			{
				await ReplyAndDeleteAsync(":x: Пожалуйста, введите полное или частичное название экзотического снаряжения.");
				return;
			}

			var exotic = ExoticStorage.GetExotic(Input);

			if (exotic == null)
			{
				await ReplyAndDeleteAsync(":x: Этой информации в моей базе данных нет.");
				return;
			}

			await ReplyAsync($"Итак, {Context.User.Username}, вот что мне известно про это снаряжение.", embed: Embeds.BuildedExotic(exotic));
		}
	}
}
