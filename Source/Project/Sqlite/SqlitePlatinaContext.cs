using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.Sqlite
{
	public class SqlitePlatinaContext : PlatinaContext<SqlitePlatinaContext>
	{
		#region Constructors

		public SqlitePlatinaContext(IGuidFactory guidFactory, DbContextOptions<SqlitePlatinaContext> options, ISystemClock systemClock) : base(guidFactory, options, systemClock) { }

		#endregion
	}
}