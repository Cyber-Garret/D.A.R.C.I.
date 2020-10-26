using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neiralink.Entities;

namespace Neiralink.FileProviders
{
	internal class WishFileProvider : IWish
	{
		public Task<Wish> GetWish(int wishNumber)
		{
			throw new NotImplementedException();
		}
	}
}
