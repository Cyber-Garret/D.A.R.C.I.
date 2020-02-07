using System;
using System.Collections.Generic;

namespace Bot.Entity.Milestone
{
	interface IMilestone
	{
		ulong MessageId { get; set; }

		string Memo { get; set; }

		DateTime DateExpire { get; set; }

		MilestoneInfo Info { get; set; }

		IEnumerable<ulong> Members { get; set; }
	}
}
