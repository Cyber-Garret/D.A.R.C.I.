using System;
using System.Collections.Generic;
using System.Text;

namespace Chrono
{
	public class ChronoConfiguration
	{
		/// <summary>
		/// appsettings.json section name
		/// </summary>
		public const string Chrono = "Chrono";

		/// <summary>
		/// Quartz cron Expression
		/// </summary>
		public string XurArrive { get; set; }

		/// <summary>
		/// Quartz cron Expression
		/// </summary>
		public string XurLeave { get; set; }

		/// <summary>
		/// Quartz cron Expression
		/// </summary>
		public string MilestoneReminder { get; set; }
	}
}
