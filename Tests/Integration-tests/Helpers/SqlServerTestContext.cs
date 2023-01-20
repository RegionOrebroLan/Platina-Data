using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace IntegrationTests.Helpers
{
	public class SqlServerTestContext : DatabaseTestContext<SqlServerDatabaseContext>
	{
		#region Constructors

		public SqlServerTestContext(bool useFakes = false) : base(useFakes)
		{
			var dataDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(dataDirectoryPath);
			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, dataDirectoryPath);

			this.ConnectionString = SqlServerHelper.ResolveConnectionString(Global.Configuration.GetConnectionString("SQLServer"), dataDirectoryPath);
			this.DataDirectoryPath = dataDirectoryPath;
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

			services.AddSqlServerDatabaseContext(builder => builder.UseSqlServer(this.ConnectionString));
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if(disposing)
				Directory.Delete(this.DataDirectoryPath, true);
		}

		#endregion
	}
}