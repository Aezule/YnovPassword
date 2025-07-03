using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnovPassword.Migrations
{
    /// <inheritdoc />
    public partial class _13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DBUtilisateur",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAdmin",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DBUtilisateur",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAdmin",
                value: false);
        }
    }
}
