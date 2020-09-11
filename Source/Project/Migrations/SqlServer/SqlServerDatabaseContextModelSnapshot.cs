﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RegionOrebroLan.Platina.Data.Migrations.SqlServer
{
	[DbContext(typeof(SqlServerDatabaseContext))]
	partial class SqlServerDatabaseContextModelSnapshot : ModelSnapshot
	{
		#region Methods

		protected override void BuildModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasAnnotation("ProductVersion", "3.1.6")
				.HasAnnotation("Relational:MaxIdentifierLength", 128)
				.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

			modelBuilder.Entity("RegionOrebroLan.Platina.Data.Entities.Document", b =>
			{
				b.Property<int>("Id")
					.HasColumnType("int");

				b.Property<string>("Category")
					.HasColumnType("nvarchar(500)")
					.HasMaxLength(500);

				b.Property<DateTime>("Confirmed")
					.HasColumnType("datetime2");

				b.Property<DateTime>("Created")
					.HasColumnType("datetime2");

				b.Property<bool>("Disabled")
					.HasColumnType("bit");

				b.Property<byte[]>("File")
					.HasColumnType("varbinary(max)");

				b.Property<string>("FileExtension")
					.HasColumnType("nvarchar(10)")
					.HasMaxLength(10);

				b.Property<Guid>("Guid")
					.HasColumnType("uniqueidentifier");

				b.Property<string>("KeywordIdentities")
					.HasColumnType("nvarchar(1000)")
					.HasMaxLength(1000);

				b.Property<string>("Keywords")
					.HasColumnType("nvarchar(2000)")
					.HasMaxLength(2000);

				b.Property<string>("Organization")
					.HasColumnType("nvarchar(max)");

				b.Property<string>("OrganizationCode")
					.HasColumnType("nvarchar(1000)")
					.HasMaxLength(1000);

				b.Property<byte[]>("PdfFile")
					.HasColumnType("varbinary(max)");

				b.Property<string>("PdfFileExtension")
					.HasColumnType("nvarchar(10)")
					.HasMaxLength(10);

				b.Property<int>("RevisionNumber")
					.HasColumnType("int");

				b.Property<DateTime>("Saved")
					.HasColumnType("datetime2");

				b.Property<string>("Title")
					.HasColumnType("nvarchar(200)")
					.HasMaxLength(200);

				b.HasKey("Id");

				b.HasIndex("Guid")
					.IsUnique();

				b.ToTable("Documents");
			});
#pragma warning restore 612, 618
		}

		#endregion
	}
}