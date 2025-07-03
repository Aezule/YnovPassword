using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YnovPassword.Migrations
{
    /// <inheritdoc />
    public partial class _11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeProfile",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "TypeProfileId",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeProfiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TypeProfiles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Netflix" },
                    { 2, "Discord" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TypeProfileId",
                table: "Accounts",
                column: "TypeProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_TypeProfiles_TypeProfileId",
                table: "Accounts",
                column: "TypeProfileId",
                principalTable: "TypeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_TypeProfiles_TypeProfileId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "TypeProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_TypeProfileId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TypeProfileId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "TypeProfile",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
