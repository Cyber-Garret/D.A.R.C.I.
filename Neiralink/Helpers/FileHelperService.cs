using Newtonsoft.Json;

using System;
using System.IO;

namespace Neiralink.Helpers
{
	internal class FileHelperService
	{
		private const string RootFolderName = "resources";

		public FileHelperService()
		{
			var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? string.Empty, RootFolderName);

			if (!Directory.Exists(rootPath))
				Directory.CreateDirectory(rootPath);
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
			if (File.Exists(filePath)) return File.ReadAllText(filePath);
			File.WriteAllText(filePath, "");
			return "";
		}
	}
}
