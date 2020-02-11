using Bot.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot
{
	internal class DataStorage
	{
		static DataStorage()
		{
			if (!Directory.Exists(Constants.ResourceFolder))
			{
				Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Constants.ResourceFolder));
			}
		}

		internal static void StoreObject(object obj, string filePath, bool useIndentations)
		{
			var formatting = (useIndentations) ? Formatting.Indented : Formatting.None;
			string json = JsonConvert.SerializeObject(obj, formatting);
			File.WriteAllText(filePath, json);
		}

		internal static T RestoreObject<T>(string filePath)
		{
			var json = GetOrCreateFileContents(filePath);
			return JsonConvert.DeserializeObject<T>(json);
		}
		internal static void RemoveObject(string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
		}

		private static string GetOrCreateFileContents(string filePath)
		{
			if (!File.Exists(filePath))
			{
				File.WriteAllText(filePath, "");
				return "";
			}
			return File.ReadAllText(filePath);
		}
	}
}
