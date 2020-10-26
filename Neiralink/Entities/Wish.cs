namespace Neiralink.Entities
{
	/// <summary>
	/// Info about wish for the Wall of Wishes in the Last Wish raid in Destiny 2
	/// </summary>
	public class Wish
	{
		public int Id { get; set; }
		/// <summary>
		/// Wish number
		/// </summary>
		public int WishNumber { get; set; }
		/// <summary>
		/// Wish title
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// Wish description
		/// </summary>
		public string Desc { get; set; }
		/// <summary>
		/// Url of screenshot
		/// </summary>
		public string WallScreenshot { get; set; }
	}
}
