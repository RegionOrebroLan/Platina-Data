using System;
using IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using RegionOrebroLan.Platina.Data;

namespace IntegrationTests.Helpers
{
	public abstract class DatabaseTestContext : IDisposable
	{
		#region Fields

		private IServiceProvider _serviceProvider;

		#endregion

		#region Constructors

		protected DatabaseTestContext(bool useFakes = false)
		{
			this.UseFakes = useFakes;
		}

		#endregion

		#region Properties

		protected internal abstract string ConnectionString { get; }
		protected internal abstract string DataDirectoryPath { get; }
		protected internal virtual FakedGuidFactory FakedGuidFactory { get; } = new FakedGuidFactory();
		protected internal virtual FakedSystemClock FakedSystemClock { get; } = new FakedSystemClock();

		protected internal virtual IServiceProvider ServiceProvider
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._serviceProvider == null)
				{
					var services = new ServiceCollection();

					this.AddServices(services);

					if(this.UseFakes)
					{
						services.AddSingleton<IGuidFactory>(this.FakedGuidFactory);
						services.AddSingleton<ISystemClock>(this.FakedSystemClock);
					}

					var serviceProvider = services.BuildServiceProvider();

					using(var scope = serviceProvider.CreateScope())
					{
						this.GetDatabaseContext(scope).Database.Migrate();
					}

					this._serviceProvider = serviceProvider;
				}
				// ReSharper restore InvertIf

				return this._serviceProvider;
			}
		}

		protected internal virtual bool UseFakes { get; }

		#endregion

		#region Methods

		protected internal abstract void AddServices(IServiceCollection services);

		protected virtual void Dispose(bool disposing)
		{
			if(!disposing)
				return;

			using(var scope = this.ServiceProvider.CreateScope())
			{
				this.GetDatabaseContext(scope).Database.EnsureDeleted();
			}

			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, Global.DataDirectoryPath);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected internal abstract DatabaseContextBase GetDatabaseContext(IServiceScope serviceScope);

		#endregion
	}

	public abstract class DatabaseTestContext<TDatabaseContext> : DatabaseTestContext where TDatabaseContext : DatabaseContextBase
	{
		#region Constructors

		protected DatabaseTestContext(bool useFakes = false) : base(useFakes) { }

		#endregion

		#region Methods

		protected internal override DatabaseContextBase GetDatabaseContext(IServiceScope serviceScope)
		{
			if(serviceScope == null)
				throw new ArgumentNullException(nameof(serviceScope));

			return serviceScope.ServiceProvider.GetRequiredService<TDatabaseContext>();
		}

		#endregion
	}
}