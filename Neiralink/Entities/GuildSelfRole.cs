using System;
using System.Collections.Generic;
using System.Text;

namespace Neiralink.Entities
{
	public class GuildSelfRole
	{
		/// <summary>
		/// Navigation property
		/// </summary>
		public ulong GuildId { get; set; }
		public Guild Guild { get; set; }

		/// <summary>
		/// Guild emote id
		/// </summary>
		public ulong EmoteID { get; set; }
		/// <summary>
		/// Guild role id
		/// </summary>
		public ulong RoleID { get; set; }
	}
}
