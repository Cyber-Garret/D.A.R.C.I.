using System.Threading.Tasks;

using Neiralink.Entities;

namespace Neiralink
{
	public interface IWish
	{
		Task<Wish> GetWish(int wishNumber);
	}
}
