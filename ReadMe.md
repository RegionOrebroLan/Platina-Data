# Platina-Data

Database for Platina integration-data, Region Örebro län. That is, for the moment, EF Core entities and db-contexts. There are three db-contexts:
- DatabaseContext
- SqliteDatabaseContext
- SqlServerDatabaseContext

Migrations exist for:
- SqliteDatabaseContext
- SqlServerDatabaseContext

With DatabaseContext you can create your own migrations.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.Platina.Data.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.Platina.Data)

## 1 Development

### 1.1 Signing

Drop the "StrongName.snk" file in the repository-root. The file should not be included in source control.

### 1.2 Migrations

#### 1.2.1 Create migrations

We might want to create/recreate migrations. If we can accept data-loss we can recreate the migrations otherwhise we will have to update them.

Copy all the commands below and run them in the Package Manager Console.

	Write-Host "Removing migrations...";
	Remove-Migration -Context SqliteDatabaseContext -Force -Project Project -StartupProject Sqlite;
	Remove-Migration -Context SqlServerDatabaseContext -Force -Project Project -StartupProject SqlServer;
	Write-Host "Removing current migrations-directory...";
	Remove-Item "Project\Migrations" -ErrorAction Ignore -Force -Recurse;
	Write-Host "Creating migrations...";
	Add-Migration Create -Context SqliteDatabaseContext -OutputDir Migrations/Sqlite -Project Project -StartupProject Sqlite;
	Add-Migration Create -Context SqlServerDatabaseContext -OutputDir Migrations/SqlServer -Project Project -StartupProject SqlServer;
	Write-Host "Finnished";

If you want more migration-information you can add the -Verbose parameter:

	Add-Migration Create -Context SqliteDatabaseContext -OutputDir Migrations/Sqlite -Project Project -StartupProject Sqlite -Verbose;

#### 1.2.2 Update migrations

Copy all the commands below and run them in the Package Manager Console.

	Write-Host "Updating migrations...";
	Add-Migration Update -Context SqliteDatabaseContext -OutputDir Migrations/Sqlite -Project Project -StartupProject Sqlite;
	Add-Migration Update -Context SqlServerDatabaseContext -OutputDir Migrations/SqlServer -Project Project -StartupProject SqlServer;
	Write-Host "Finnished";

### 1.3 Scaffold-DbContext

If we want to know how we should code our DbContexts / Entities we can use **Scaffold-DbContext**. Create a database first with eg. **Microsoft SQL Server Management Studio** through the designer or with scripts. Then scaffold a db-context from it. Then we can look at the generated classes and see how we should "code first".

- Create a LocalDB-database named eg. "07390b07-29ed-4944-aced-df2ad5b4dbc2".
- Create your database tables etc.
- Run: Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=07390b07-29ed-4944-aced-df2ad5b4dbc2" Microsoft.EntityFrameworkCore.SqlServer -ContextDir "" -OutputDir "Your-directory"
- Delete the database

## 2 Links

- [Migrations with Multiple Providers](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers/)