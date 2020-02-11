using Bot.Entity;
using Bot.Services;
using Bot.Services.Quartz;
using Bot.Services.Quartz.Jobs;
using Bot.Services.Storage;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Bot
{
	public class Program
	{
		public static void Main()
		{
			CreateHostBuilder().Build().Run();
		}

		public static IHostBuilder CreateHostBuilder() =>
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

					// Quartz services
					services.AddHostedService<Quartz>();
					services.AddSingleton<IJobFactory, SingletonJobFactory>();
					services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
					// Quartz jobs
					services.AddSingleton<XurArrive>();
					services.AddSingleton<XurLeave>();
					services.AddSingleton<MilestoneRemind>();
					// Quartz triggers
					var hour = hostContext.Configuration.GetSection("BotConfig:XurHour").Get<int>();
					services.AddSingleton(new JobSchedule(typeof(XurArrive), $"0 0 {hour} ? * FRI")); // run every Friday in 20:00
					services.AddSingleton(new JobSchedule(typeof(XurLeave), $"0 0 {hour} ? * TUE")); // run every Tuesday in 20:00

					services.AddSingleton(new JobSchedule(typeof(MilestoneRemind), "0/10 * * * * ?")); // run every 10 seconds.

				});
	}
}
