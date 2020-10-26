using System.Threading.Tasks;

using Neiralink.Entities;

namespace Neiralink
{
	public interface ICatalyst
	{
		Task<Catalyst> GetCatalyst(string arg);
	}
}
