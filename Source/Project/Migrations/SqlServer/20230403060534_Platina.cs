using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegionOrebroLan.Platina.Data.Migrations.SqlServer
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
					Id = table.Column<int>(type: "int", nullable: false),
					Category = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
					Confirmed = table.Column<DateTime>(type: "datetime2", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Disabled = table.Column<bool>(type: "bit", nullable: false),
					File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
					FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
					Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					KeywordIdentities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
					Keywords = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
					Organization = table.Column<string>(type: "nvarchar(max)", nullable: true),
					OrganizationCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
					PdfFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
					PdfFileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
					RevisionNumber = table.Column<int>(type: "int", nullable: false),
					Saved = table.Column<DateTime>(type: "datetime2", nullable: false),
					Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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