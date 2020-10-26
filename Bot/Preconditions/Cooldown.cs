using Discord.Commands;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Vex.Properties;

namespace Bot.Preconditions
{
	public sealed class Cooldown : PreconditionAttribute
	{
		private TimeSpan CooldownLength { get; }
		private readonly ConcurrentDictionary<CooldownInfo, DateTime> _cooldowns = new ConcurrentDictionary<CooldownInfo, DateTime>();

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
			if (_cooldowns.TryGetValue(key, out var endsAt))
			{
				var difference = endsAt.Subtract(DateTime.UtcNow);
				// if command in cooldown.
				if (difference.Ticks > 0)
				{
					return Task.FromResult(PreconditionResult.FromError(string.Format(Resources.CooldownMessage, difference.Seconds)));
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

		private readonly struct CooldownInfo
		{
			private ulong UserId { get; }
			private int CommandHashCode { get; }

			public CooldownInfo(ulong userId, int commandHashCode)
			{
				UserId = userId;
				CommandHashCode = commandHashCode;
			}
		}
	}
}
