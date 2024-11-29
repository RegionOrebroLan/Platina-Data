using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace Application
{
	public class Startup(IConfiguration configuration)
	{
		#region Properties

		public IConfiguration Configuration { get; } = configuration;

		#endregion

		#region Methods

		public virtual void Configure(IApplicationBuilder app)
		{
			if(app == null)
				throw new ArgumentNullException(nameof(app));

			app.UseDeveloperExceptionPage();

			using(var scope = app.ApplicationServices.CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<PlatinaContext>().Database.Migrate();
			}

			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddRazorPages();
			services.AddSqlitePlatinaContext(options => options.UseSqlite(this.Configuration.GetConnectionString("Platina")));
		}

		#endregion
	}
}