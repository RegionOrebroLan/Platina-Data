using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace Sqlite
{
	public class Startup
	{
		#region Methods

		public virtual void Configure() { }

		public virtual void ConfigureServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddSqliteDatabaseContext(builder => builder.UseSqlite("?"));
		}

		#endregion
	}
}