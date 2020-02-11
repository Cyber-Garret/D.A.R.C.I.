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
	public class RaidStorage
	{
		private const string RaidsFolderName = "raids";
		private static readonly ConcurrentDictionary<ulong, Raid> Raids = new ConcurrentDictionary<ulong, Raid>();
		static RaidStorage()
		{
			var raidsFolder = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, RaidsFolderName));
			var files = raidsFolder.GetFiles("*.json");

			if (files.Length > 0)
			{
				foreach (var file in files)
				{
					var raid = DataStorage.RestoreObject<Raid>(file.FullName);
					Raids.TryAdd(raid.MessageId, raid);
				}
			}
		}

		/// <summary>
		/// Get all Raids ordered by DateExpire for old to new
		/// </summary>
		/// <returns></returns>
		internal List<Raid> GetAllRaids()
		{
			return Raids.Values
				.OrderBy(r => r.DateExpire)
				.ToList();
		}

		internal Raid GetRaid(ulong messageId)
		{
			Raids.TryGetValue(messageId, out Raid raid);

			return raid;
		}

		internal void AddRaid(Raid raid)
		{
			Raids.TryAdd(raid.MessageId, raid);
		}

		internal void RemoveRaid(ulong MessageId)
		{
			//remove from dictionary
			Raids.TryRemove(MessageId, out Raid value);
			//remove file
			DataStorage.RemoveObject(Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, RaidsFolderName, $"{MessageId}.json"));
		}

		internal void SaveRaids(params ulong[] ids)
		{
			foreach (var id in ids)
			{
				DataStorage.StoreObject(GetRaid(id), Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, RaidsFolderName, $"{id}.json"), useIndentations: true);
			}
		}

		internal void SaveRaids()
		{
			foreach (var id in Raids.Keys)
			{
				SaveRaids(id);
			}
		}
	}
}
