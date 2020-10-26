﻿using Microsoft.Extensions.Configuration;

using Neiralink;
using Neiralink.FileProviders;
using Neiralink.Helpers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static void AddNeiralink(this IServiceCollection services, IConfiguration configuration)
		{
			var neiralinkConfiguration = new NeiralinkConfiguration();
			configuration.GetSection(NeiralinkConfiguration.Neiralink).Bind(neiralinkConfiguration);

			//Add internal file helper service
			services.AddSingleton<FileHelperService>();

			AddGuild(services, neiralinkConfiguration.GuildId);
			AddCatalystService(services);
			AddWishService(services);
		}

		private static void AddGuild(IServiceCollection service, ulong guildId) =>
			service.AddSingleton<IGuildConfig, GuildFileProvider>(provider =>
				new GuildFileProvider(guildId));

		private static void AddCatalystService(IServiceCollection service) =>
			service.AddSingleton<ICatalyst, CatalystFileProvider>();

		private static void AddWishService(IServiceCollection service) =>
			service.AddSingleton<IWish, WishFileProvider>();
	}
}
