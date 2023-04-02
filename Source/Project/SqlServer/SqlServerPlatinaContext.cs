using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.SqlServer
{
	public class SqlServerPlatinaContext : PlatinaContext<SqlServerPlatinaContext>
	{
		#region Constructors

		public SqlServerPlatinaContext(IGuidFactory guidFactory, DbContextOptions<SqlServerPlatinaContext> options, ISystemClock systemClock) : base(guidFactory, options, systemClock) { }

		#endregion
	}
}