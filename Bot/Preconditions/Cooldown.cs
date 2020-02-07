﻿using Discord.Commands;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bot.Preconditions
{
	public sealed class Cooldown : PreconditionAttribute
	{
		TimeSpan CooldownLength { get; set; }
		readonly ConcurrentDictionary<CooldownInfo, DateTime> _cooldowns = new ConcurrentDictionary<CooldownInfo, DateTime>();

		/// <summary>
		/// Add cooldown for command.
		/// </summary>
		/// <param name="seconds">command cooldown in seconds.</param>
		public Cooldown(int seconds)
		{
			CooldownLength = TimeSpan.FromSeconds(seconds);
		}

		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			var key = new CooldownInfo(context.User.Id, command.GetHashCode());
			// Check if user command cooldown now?
			if (_cooldowns.TryGetValue(key, out DateTime endsAt))
			{
				var difference = endsAt.Subtract(DateTime.UtcNow);
				// if command in cooldown.
				if (difference.Ticks > 0)
				{
					return Task.FromResult(PreconditionResult.FromError($"You can use this command again after {difference:%s} sec."));
				}
				// update time
				var time = DateTime.UtcNow.Add(CooldownLength);
				_cooldowns.TryUpdate(key, time, endsAt);
			}
			else
			{
				_cooldowns.TryAdd(key, DateTime.UtcNow.Add(CooldownLength));
			}

			return Task.FromResult(PreconditionResult.FromSuccess());
		}
		internal struct CooldownInfo
		{
			public ulong UserId { get; }
			public int CommandHashCode { get; }

			public CooldownInfo(ulong userId, int commandHashCode)
			{
				UserId = userId;
				CommandHashCode = commandHashCode;
			}
		}
	}
}