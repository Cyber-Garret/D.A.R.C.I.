using System.Threading.Tasks;

using Neiralink.Entities;

namespace Neiralink
{
	public interface IGuildConfig
	{
		/// <summary>
		/// Return guild configuration
		/// </summary>
		/// <returns></returns>
		Task<Guild> GetGuildConfig();
		/// <summary>
		/// Save to file and in memory guild configuration
		/// </summary>
		/// <param name="guild"></param>
		/// <returns></returns>
		Task<Guild> SaveGuildConfig(Guild guild);
	}
}
