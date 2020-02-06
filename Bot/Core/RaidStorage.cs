using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Bot.Core
{
	internal static class RaidStorage
	{
		private static readonly ConcurrentDictionary<string, Exotic> exotics = new ConcurrentDictionary<string, Exotic>();
		static RaidStorage()
		{

		}
	}
}
