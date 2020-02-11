using Bot.Services.Storage;
using Discord.WebSocket;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Services.Quartz.Jobs
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
