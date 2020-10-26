using System;
using System.Collections.Generic;
using System.Text;

namespace Neiralink.Entities
{
	/// <summary>
	/// Info about Destiny 2 weapon catalyst
	/// </summary>
	public class Catalyst
	{
		public int Id { get; set; }
		/// <summary>
		/// Weapon name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Alias for search
		/// </summary>
		public string Alias { get; set; }
		/// <summary>
		/// URL of icon
		/// </summary>
		public string Icon { get; set; }
		/// <summary>
		/// Weapon description
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// How to obtain
		/// </summary>
		public string DropLocation { get; set; }
		/// <summary>
		/// Quest objectives
		/// </summary>
		public string Objectives { get; set; }
		/// <summary>
		/// Bonuses of catalyst
		/// </summary>
		public string Masterwork { get; set; }
	}
}
