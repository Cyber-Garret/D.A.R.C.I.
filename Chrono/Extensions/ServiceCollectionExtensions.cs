using System;
using System.Collections.Generic;
using System.Text;

using Chrono.Jobs;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Chrono.Extensions
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Quartz DI services for discord bot
		/// </summary>
		public static void AddChrono(this IServiceCollection services, IConfiguration config)
		{
			var chronoConfig = new ChronoConfiguration();
			config.GetSection(ChronoConfiguration.Chrono).Bind(chronoConfig);

			AddQuartzBaseServices(services);
			AddQuartzJobs(services, chronoConfig);
		}

		private static void AddQuartzBaseServices(IServiceCollection services)
		{
			services.AddHostedService<ChronoHostedService>();
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
		}

		private static void AddQuartzJobs(IServiceCollection services, ChronoConfiguration config)
		{
			// Quartz jobs
			services.AddSingleton<XurArrive>();
			services.AddSingleton<XurLeave>();
			services.AddSingleton<MilestoneRemind>();
			// Quartz triggers
			services.AddSingleton(new JobSchedule(typeof(XurArrive), config.XurArrive)); // run every Friday in 20:00
			services.AddSingleton(new JobSchedule(typeof(XurLeave), config.XurLeave)); // run every Tuesday in 20:00

			services.AddSingleton(new JobSchedule(typeof(MilestoneRemind), "0/10 * * * * ?")); // run every 10 seconds.
		}
	}
}
