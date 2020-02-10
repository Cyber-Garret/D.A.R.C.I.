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
		[Required]
		public DateTime DateExpire { get; set; }
		[Required]
		public MilestoneInfo Info { get; set; }
		public List<ulong> Members { get; set; } = new List<ulong>();
	}
}
