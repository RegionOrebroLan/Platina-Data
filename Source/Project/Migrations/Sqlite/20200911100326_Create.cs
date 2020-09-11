using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegionOrebroLan.Platina.Data.Migrations.Sqlite
{
	public partial class Create : Migration
	{
		#region Methods

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Documents");
		}

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Documents",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false),
					Category = table.Column<string>(maxLength: 500, nullable: true),
					Confirmed = table.Column<DateTime>(nullable: false),
					Created = table.Column<DateTime>(nullable: false),
					Disabled = table.Column<bool>(nullable: false),
					File = table.Column<byte[]>(nullable: true),
					FileExtension = table.Column<string>(maxLength: 10, nullable: true),
					Guid = table.Column<Guid>(nullable: false),
					KeywordIdentities = table.Column<string>(maxLength: 1000, nullable: true),
					Keywords = table.Column<string>(maxLength: 2000, nullable: true),
					Organization = table.Column<string>(nullable: true),
					OrganizationCode = table.Column<string>(maxLength: 1000, nullable: true),
					PdfFile = table.Column<byte[]>(nullable: true),
					PdfFileExtension = table.Column<string>(maxLength: 10, nullable: true),
					RevisionNumber = table.Column<int>(nullable: false),
					Saved = table.Column<DateTime>(nullable: false),
					Title = table.Column<string>(maxLength: 200, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Documents", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Documents_Guid",
				table: "Documents",
				column: "Guid",
				unique: true);
		}

		#endregion
	}
}