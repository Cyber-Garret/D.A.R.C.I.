using Bot.Core;
using Bot.Helpers;

using Discord.Addons.Interactive;
using Discord.Commands;

using System.Threading.Tasks;

namespace Bot.Modules
{
	public class Main : InteractiveBase
	{
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
