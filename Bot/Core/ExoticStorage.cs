using Bot.Entity;

using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace Bot.Core
{
	internal class ExoticStorage
	{
		private static readonly ConcurrentDictionary<string, Exotic> exotics = new ConcurrentDictionary<string, Exotic>();

		static ExoticStorage()
		{
			var exoticsFolder = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder, Constants.ExoticsFolder));
			var files = exoticsFolder.GetFiles("*.json");
			if (files.Length > 0)
			{
				foreach (var file in files)
				{
					var exotic = DataStorage.RestoreObject<Exotic>(file.FullName);
					exotics.TryAdd(exotic.Name, exotic);
				}
			}
		}

		internal static Exotic GetExotic(string name)
		{
			var searchName = exotics.Keys.FirstOrDefault(key => key.Contains(Alias(name), System.StringComparison.InvariantCultureIgnoreCase));
			if (searchName != null)
			{
				exotics.TryGetValue(searchName, out Exotic exotic);
				return exotic;
			}
			else
				return null;
		}

		private static string Alias(string name)
		{
			return name switch
			{
				"дарси" => "Д.А.Р.С.И.",
				"мида" => "MIDA",
				"сурос" => "SUROS",
				"морозники" => "M0р03ники",
				"топотуны" => "Т0п0тунЬI",
				_ => name,
			};
		}
	}
}
