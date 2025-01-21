using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class debtTableModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Debts",
                table: "Debts");

            migrationBuilder.DropIndex(
                name: "IX_Debts_CreditorId",
                table: "Debts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debts",
                table: "Debts",
                columns: new[] { "CreditorId", "DebtorId", "TripId" });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_TripId",
                table: "Debts",
                column: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Debts",
                table: "Debts");

            migrationBuilder.DropIndex(
                name: "IX_Debts_TripId",
                table: "Debts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debts",
                table: "Debts",
                columns: new[] { "TripId", "DebtorId", "CreditorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Debts_CreditorId",
                table: "Debts",
                column: "CreditorId");
        }
    }
}
