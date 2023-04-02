using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.SqlServer
{
	/// <summary>
	/// Class used when creating migrations.
	/// </summary>
	public class SqlServerPlatinaContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerPlatinaContext>
	{
		#region Methods

		public SqlServerPlatinaContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqlServerPlatinaContext>();
			optionsBuilder.UseSqlServer("A value that can not be empty just to be able to create/update migrations.");

			return new SqlServerPlatinaContext(new GuidFactory(), optionsBuilder.Options, new SystemClock());
		}

		#endregion
	}
}