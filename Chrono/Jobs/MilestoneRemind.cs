using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Chrono.Jobs
{
	[DisallowConcurrentExecution]
	public class MilestoneRemind : IJob
	{
		private readonly DiscordSocketClient discord;
		private readonly RaidStorage raidStorage;
		private readonly MilestoneHandler milestoneHandler;
		public MilestoneRemind(IServiceProvider service)
		{
			discord = service.GetRequiredService<DiscordSocketClient>();
			raidStorage = service.GetRequiredService<RaidStorage>();
			milestoneHandler = service.GetRequiredService<MilestoneHandler>();
		}
		public async Task Execute(IJobExecutionContext context)
		{
			var timer = DateTime.Now.AddMinutes(15);

			var raids = raidStorage.GetAllRaids();

			if (raids.Count > 0)
			{
				foreach (var raid in raids)
				{
					if (timer.Date == raid.DateExpire.Date && timer.Hour == raid.DateExpire.Hour && timer.Minute == raid.DateExpire.Minute && timer.Second < 10)
					{
						await milestoneHandler.RaidNotificationAsync(raid);
					}
				}
			}
		}
	}
}
