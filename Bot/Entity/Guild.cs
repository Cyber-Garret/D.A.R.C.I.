namespace Bot.Entity
{
	internal class Guild
	{
		internal ulong NotificationChannel { get; set; } = 0;
		internal ulong LoggingChannel { get; set; } = 0;
		internal ulong WelcomeChannel { get; set; } = 0;
		internal string WelcomeMessage { get; set; } = null;
		internal ulong AutoroleID { get; set; } = 0;
		internal string GlobalMention { get; set; } = null;
		internal ulong SelfRoleMessageId { get; set; } = 0;
	}
}