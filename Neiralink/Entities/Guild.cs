using System;
using System.Collections.Generic;
using System.Text;

namespace Neiralink.Entities
{
	/// <summary>
	/// Discord guild settings
	/// </summary>
	public class Guild
	{
		/// <summary>
		/// Guild id
		/// </summary>
		public ulong Id { get; set; }
		/// <summary>
		/// Text channel id for news
		/// </summary>
		public ulong NotificationChannelId { get; set; } = 0;

		public string MilestoneMention { get; set; } = "@here";
		/// <summary>
		/// Self role message id
		/// </summary>
		public ulong SelfRoleMessageId { get; set; } = 0;

		public ICollection<GuildSelfRole> SelfRoles { get; set; }
	}
}
