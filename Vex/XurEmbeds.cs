using Discord;

using Vex.Properties;

namespace Vex
{
	public static class XurEmbeds
	{
		private const string XurIcon = "https://www.bungie.net/common/destiny2_content/icons/5659e5fc95912c079962376dfe4504ab.png";

		public static Embed XurArriveEmbed() =>
			new EmbedBuilder
			{
				Title = Resources.XurArriveEmbTitle,
				Color = Color.Gold,
				ThumbnailUrl = XurIcon,
				Description = Resources.XurArriveEmbDesc
			}.Build();

		public static Embed XurLeaveEmbed() =>
			new EmbedBuilder
			{
				Title = Resources.XurLeaveEmbTitle,
				Color = Color.Red,
				ThumbnailUrl = XurIcon,
				Description = Resources.XurLeaveEmbDesc
			}.Build();

	}
}
