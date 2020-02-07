using Bot.Core;
using Bot.Entity.Milestone;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bot.Services.Storage
{
	public class MilestoneInfoStorage
	{
		private static readonly ConcurrentDictionary<string, MilestoneInfo> Milestones = new ConcurrentDictionary<string, MilestoneInfo>();
		static MilestoneInfoStorage()
		{
			var milestonesFolder = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, "milestones"));
			var files = milestonesFolder.GetFiles("*.json");

			if (files.Length > 0)
			{
				foreach (var file in files)
				{
					var milestone = DataStorage.RestoreObject<MilestoneInfo>(file.FullName);
					Milestones.TryAdd(milestone.Alias, milestone);
				}
			}
		}

		public MilestoneInfo SearchMilestone(string searchName, MilestoneType milestoneType)
		{
			return Milestones.Values.FirstOrDefault(m => (m.Alias == searchName || m.Name.Contains(searchName, StringComparison.InvariantCultureIgnoreCase)) && m.Type == milestoneType);
		}

		public IEnumerable<MilestoneInfo> GetAllRaids()
		{
			return Milestones.Values.Where(t => t.Type == MilestoneType.Raid);
		}

		public IEnumerable<MilestoneInfo> GetAllStrikes()
		{
			return Milestones.Values.Where(t => t.Type == MilestoneType.Strike);
		}
	}
}
