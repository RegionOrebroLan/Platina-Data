using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Platina.Data.Builder.Extensions
{
	public static class ApplicationBuilderExtension
	{
		#region Methods

		public static IApplicationBuilder UsePlatinaContext(this IApplicationBuilder applicationBuilder)
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			using(var scope = applicationBuilder.ApplicationServices.CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<PlatinaContext>().Database.Migrate();
			}

			return applicationBuilder;
		}

		#endregion
	}
}