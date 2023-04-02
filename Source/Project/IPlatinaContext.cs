using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegionOrebroLan.Platina.Data.Entities;

namespace RegionOrebroLan.Platina.Data
{
	public interface IPlatinaContext : IDisposable
	{
		#region Properties

		DbSet<Document> Documents { get; set; }

		#endregion

		#region Methods

		int SaveChanges();
		int SaveChanges(bool acceptAllChangesOnSuccess);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

		#endregion
	}
}