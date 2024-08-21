using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.SqlServer
{
	public class SqlServerPlatinaContext(IGuidFactory guidFactory, DbContextOptions<SqlServerPlatinaContext> options, ISystemClock systemClock) : PlatinaContext<SqlServerPlatinaContext>(guidFactory, options, systemClock) { }
}