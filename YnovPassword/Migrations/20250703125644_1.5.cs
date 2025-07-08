using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YnovPassword.Migrations
{
    /// <inheritdoc />
    public partial class _15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Dictionnaires",
                columns: new[] { "Id", "Mot" },
                values: new object[,]
                {
                    { 2, "banane" },
                    { 3, "cerise" },
                    { 4, "datte" },
                    { 5, "fraise" },
                    { 6, "raisin" },
                    { 7, "melon" },
                    { 8, "kiwi" },
                    { 9, "citron" },
                    { 10, "mangue" },
                    { 11, "abricot" },
                    { 12, "pêche" },
                    { 13, "prune" },
                    { 14, "poire" },
                    { 15, "orange" },
                    { 16, "framboise" },
                    { 17, "myrtille" },
                    { 18, "groseille" },
                    { 19, "pastèque" },
                    { 20, "pamplemousse" },
                    { 21, "noix" },
                    { 22, "amande" },
                    { 23, "châtaigne" },
                    { 24, "figue" },
                    { 25, "mûre" }
                });

            migrationBuilder.UpdateData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Amazon");

            migrationBuilder.InsertData(
                table: "TypeProfiles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Google" },
                    { 4, "Facebook" },
                    { 5, "Twitter" },
                    { 6, "LinkedIn" },
                    { 7, "Microsoft" },
                    { 8, "Apple" },
                    { 9, "Spotify" },
                    { 10, "Adobe" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Dictionnaires",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "TypeProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Discord");

            migrationBuilder.InsertData(
                table: "TypeProfiles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Netflix" });
        }
    }
}
