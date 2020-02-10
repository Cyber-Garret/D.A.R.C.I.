using Bot.Entity;
using Bot.Entity.Milestone;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Helpers
{
	internal static class Embeds
	{
		internal static Embed BuildedExotic(Exotic exotic)
		{
			var embed = new EmbedBuilder
			{
				Color = Color.Gold,
				Title = exotic.Type + " - " + exotic.Name,
				ThumbnailUrl = exotic.IconUrl,
				ImageUrl = exotic.ImageUrl,
				Description = exotic.Description
			};
			// only weapon can have catalyst
			if (exotic.isWeapon)
			{
				embed.AddField("Катализатор", exotic.isHaveCatalyst == true ? "**Есть**" : "**Отсутствует**");
			}
			// main exotic perk and description
			embed.AddField(exotic.Perk, exotic.PerkDescription, true);
			// second perk if him unique
			if (exotic.SecondPerk != null && exotic.SecondPerkDescription != null)
				embed.AddField(exotic.SecondPerk, exotic.SecondPerkDescription, true);
			// where exotic gear can obtain
			embed.AddField("Как получить:", exotic.DropLocation, true);
			// simple footer =)
			embed.WithFooter($"Если нашли какие либо неточности, сообщите моему создателю: Cyber_Garret#5898",
				"https://www.bungie.net/common/destiny2_content/icons/ee21b5bc72f9e48366c9addff163a187.png");

			return embed.Build();
		}

		internal static Embed XurArrive()
		{
			var embed = new EmbedBuilder
			{
				Title = "Стражи! Зур прибыл в солнечную систему!",
				Color = Color.Gold,
				ThumbnailUrl = "https://www.bungie.net/common/destiny2_content/icons/5659e5fc95912c079962376dfe4504ab.png",
				Description =
				"Определить точное местоположение Зур-а я не могу.\n" +
				"[Тут ты сможешь отыскать его положение](https://whereisxur.com/)\n" +
				"[Или тут](https://ftw.in/game/destiny-2/find-xur)"
			};

			return embed.Build();
		}

		internal static Embed XurLeave()
		{
			var embed = new EmbedBuilder
			{
				Title = "Внимание! Зур покинул солнечную систему.",
				Color = Color.Red,
				ThumbnailUrl = "https://www.bungie.net/common/destiny2_content/icons/5659e5fc95912c079962376dfe4504ab.png",
				Description = "Он просто испарился! :open_mouth: "
			};

			return embed.Build();
		}
	}
}
