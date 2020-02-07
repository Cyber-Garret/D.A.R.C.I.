using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bot.Entity.Milestone
{
	public class Raid : IMilestone
	{
		public ulong MessageId { get; set; }
		[MaxLength(1000)]
		public string Memo { get; set; }
		public DateTime DateExpire { get; set; }
		public MilestoneInfo Info { get; set; }
		public IEnumerable<ulong> Members { get; set; }
	}
}
