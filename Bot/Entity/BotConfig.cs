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
	}
}
