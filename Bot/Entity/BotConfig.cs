namespace Bot.Entity
{
	/// <summary>
	/// Represent bot settings from appsettings.json
	/// </summary>
	public class BotConfig
	{
		/// <summary>
		/// Discord Bot Token from https://discordapp.com/developers/applications/
		/// </summary>
		public string Token { get; set; } = null;

		/// <summary>
		/// Prefix for bot commands
		/// </summary>
		public string Prefix { get; set; } = "!";
		/// <summary>
		/// Xur arrive and leave hour(0-24 format) for push message.
		/// </summary>
		public int XurHour { get; set; } = 20;
	}
}
