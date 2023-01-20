using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace IntegrationTests.Helpers
{
	public class SqliteTestContext : DatabaseTestContext<SqliteDatabaseContext>
	{
		#region Constructors

		public SqliteTestContext(bool useFakes = false) : base(useFakes)
		{
			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, Global.DataDirectoryPath);

			this.ConnectionString = Global.Configuration.GetConnectionString("Sqlite");
			this.DataDirectoryPath = Global.DataDirectoryPath;
		}

		#endregion

		#region Properties

		protected internal override string ConnectionString { get; }
		protected internal override string DataDirectoryPath { get; }

		#endregion

		#region Methods

		protected internal override void AddServices(IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddSqliteDatabaseContext(builder => builder.UseSqlite(this.ConnectionString));
		}

		#endregion
	}
}