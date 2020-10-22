using Bot.Entity;
using Bot.Services;
using Bot.Services.Storage;
using Chrono.Extensions;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bot
{
	public static class Program
	{
		public static void Main()
		{
			CreateHostBuilder().Build().Run();
		}

		private static IHostBuilder CreateHostBuilder() =>
			Host.CreateDefaultBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					//load and map bot settings to class
					services.Configure<BotConfig>(options => hostContext.Configuration.GetSection("BotConfig").Bind(options));

					// Bot services
					services.AddHostedService<Darci>();

					services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
					{
						ExclusiveBulkDelete = true,
						AlwaysDownloadUsers = true,
						LogLevel = LogSeverity.Verbose,
						DefaultRetryMode = RetryMode.AlwaysRetry,
						MessageCacheSize = 1000
					}));
					services.AddSingleton<CommandService>();
					services.AddSingleton<DiscordLogging>();
					services.AddSingleton<CommandHandler>();
					services.AddSingleton<InteractiveService>();
					services.AddSingleton<DiscordEventHandler>();
					services.AddSingleton<MilestoneHandler>();
					//Data Storage services
					services.AddSingleton<MilestoneInfoStorage>();
					services.AddSingleton<RaidStorage>();

					services.AddChrono();
					services.AddNeiralink();
				});
	}
}
