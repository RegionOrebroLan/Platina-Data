using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using RegionOrebroLan.Platina.Data.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RegionOrebroLan.Platina.Data
{
	public abstract class DatabaseContextBase : DbContext
	{
		#region Fields

		public const string DocumentsTableName = "Documents";

		#endregion

		#region Constructors

		protected DatabaseContextBase(IGuidFactory guidFactory, DbContextOptions options, ISystemClock systemClock) : base(options)
		{
			this.GuidFactory = guidFactory ?? throw new ArgumentNullException(nameof(guidFactory));
			this.SystemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
		}

		#endregion

		#region Properties

		public virtual DbSet<Document> Documents { get; set; }
		protected internal virtual IGuidFactory GuidFactory { get; }
		protected internal virtual ISystemClock SystemClock { get; }

		#endregion

		#region Methods

		protected internal virtual void CreateEntryModel(ModelBuilder modelBuilder)
		{
			if(modelBuilder == null)
				throw new ArgumentNullException(nameof(modelBuilder));

			modelBuilder.Entity<Document>(entity =>
			{
				entity.HasIndex(document => document.Guid).IsUnique();

				entity.HasKey(document => document.Id);
				entity.Property(document => document.Id).ValueGeneratedNever();

				entity.ToTable(DocumentsTableName);
			});
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			this.CreateEntryModel(modelBuilder);
		}

		protected internal virtual void PrepareSaveChanges()
		{
			var now = this.SystemClock.UtcNow.UtcDateTime;

			foreach(var entityEntry in this.ChangeTracker.Entries().Where(entityEntry => entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified))
			{
				if(!(entityEntry.Entity is Document document))
					continue;

				if(entityEntry.State == EntityState.Added)
				{
					document.Created = now;
					document.Guid = this.GuidFactory.Create();
				}

				document.Saved = now;
			}
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.PrepareSaveChanges();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
		{
			this.PrepareSaveChanges();

			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		#endregion
	}

	public abstract class DatabaseContextBase<T> : DatabaseContextBase where T : DatabaseContextBase
	{
		#region Constructors

		protected DatabaseContextBase(IGuidFactory guidFactory, DbContextOptions<T> options, ISystemClock systemClock) : base(guidFactory, options, systemClock) { }

		#endregion
	}
}