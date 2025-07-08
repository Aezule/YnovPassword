using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YnovPassword.Migrations
{
    /// <inheritdoc />
    public partial class _10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DBUtilisateur",
                columns: new[] { "Id", "Email", "MotDePasse", "Nom", "Prenom" },
                values: new object[] { 1, "Robert", "a854b1b2a415b94f545c9490e86eb92c7da6c643f2afce6b69c672ba0ba3be0c", "DeMadrido", "Roberto" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DBUtilisateur",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
