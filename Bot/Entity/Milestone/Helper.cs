using System.ComponentModel.DataAnnotations;

namespace Bot.Entity.Milestone
{
	public class MilestoneInfo
	{

		[MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(50)]
		public string Alias { get; set; }

		public string DisplayType { get; set; }
		public MilestoneType Type { get; set; }

		[MaxLength(1000)]
		public string Icon { get; set; }

		public byte MaxSpace { get; set; }
	}

	public enum MilestoneType
	{
		Raid,
		Strike
	}
}
