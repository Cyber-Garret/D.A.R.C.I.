using System.Threading.Tasks;

using Neiralink.Entities;

namespace Neiralink
{
	public interface IGuildConfig
	{
		Task<Guild> GetGuildConfig();
		Task<Guild> SaveGuildConfig();
	}
}
