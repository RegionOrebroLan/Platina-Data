using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegionOrebroLan.Platina.Data.Migrations.Sqlite
{
	public partial class Platina : Migration
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
					Id = table.Column<int>(type: "INTEGER", nullable: false),
					Category = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
					Confirmed = table.Column<DateTime>(type: "TEXT", nullable: false),
					Created = table.Column<DateTime>(type: "TEXT", nullable: false),
					Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
					File = table.Column<byte[]>(type: "BLOB", nullable: true),
					FileExtension = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
					Guid = table.Column<Guid>(type: "TEXT", nullable: false),
					KeywordIdentities = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
					Keywords = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
					Organization = table.Column<string>(type: "TEXT", nullable: true),
					OrganizationCode = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
					PdfFile = table.Column<byte[]>(type: "BLOB", nullable: true),
					PdfFileExtension = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
					RevisionNumber = table.Column<int>(type: "INTEGER", nullable: false),
					Saved = table.Column<DateTime>(type: "TEXT", nullable: false),
					Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_Documents", x => x.Id); });

			migrationBuilder.CreateIndex(
				name: "IX_Documents_Guid",
				table: "Documents",
				column: "Guid",
				unique: true);
		}

		#endregion
	}
}