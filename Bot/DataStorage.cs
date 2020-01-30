using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot
{
	internal class DataStorage
	{
		internal static T RestoreObject<T>(string filePath)
		{
			var json = GetOrCreateFileContents(filePath);
			return JsonConvert.DeserializeObject<T>(json);
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
