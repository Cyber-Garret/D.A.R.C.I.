using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bot.Entity
{
	internal class Milestone
	{
		[Key]
		public byte Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(50)]
		public string Alias { get; set; }
		[MaxLength(50)]
		public string Type { get; set; }
		[MaxLength(1000)]
		public string Icon { get; set; }
		/// <summary>
		/// Raid have 5 free space without leader, need for cycle
		/// </summary>
		public byte MaxSpace { get; set; }

		public List<ActiveMilestone> ActiveMilestones { get; set; }
	}

	internal class MilestoneUser
	{
		[Key]
		public int Id { get; set; }
		public ulong ActiveMilestoneMessageId { get; set; }
		public ActiveMilestone ActiveMilestone { get; set; }
		public ulong UserId { get; set; }
		public byte? Place { get; set; }
	}

	internal class ActiveMilestone
	{
		public ulong MessageId { get; set; }
		[Required]
		public byte MilestoneId { get; set; }
		public Milestone Milestone { get; set; }

		[MaxLength(1000)]
		public string Memo { get; set; }
		[Required]
		public ulong Leader { get; set; }
		[Required]
		public DateTime DateExpire { get; set; }

		public IEnumerable<MilestoneUser> MilestoneUsers { get; set; }
	}
}
