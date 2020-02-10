using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Entity.Milestone
{
	public class Strike : IMilestone
	{
		public ulong MessageId { get; set; }
		public string Memo { get; set; }
		public DateTime DateExpire { get; set; }
		public MilestoneInfo Info { get; set; }
		public List<ulong> Members { get; set; }
	}
}
