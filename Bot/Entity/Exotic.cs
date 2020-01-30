using System.ComponentModel.DataAnnotations;

namespace Bot.Entity
{
	internal class Exotic
	{
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(100)]
		public string Type { get; set; }
		[MaxLength(1000)]
		public string IconUrl { get; set; }
		[MaxLength(1000)]
		public string ImageUrl { get; set; }
		[MaxLength(1024)]
		public string Description { get; set; }
		[MaxLength(100)]
		public string Perk { get; set; }
		[MaxLength(1024)]
		public string PerkDescription { get; set; }
		[MaxLength(100)]
		public string SecondPerk { get; set; }
		[MaxLength(1000)]
		public string SecondPerkDescription { get; set; }
		[MaxLength(300)]
		public string DropLocation { get; set; }
		public bool isWeapon { get; set; } = false;
		public bool isHaveCatalyst { get; set; } = false;
	}
}
