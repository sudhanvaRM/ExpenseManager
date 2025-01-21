using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class debtTableAltered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_trip_TripId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Debts_users_CreditorId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Debts_users_DebtorId",
                table: "Debts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Debts",
                table: "Debts");

            migrationBuilder.RenameTable(
                name: "Debts",
                newName: "Debt");

            migrationBuilder.RenameIndex(
                name: "IX_Debts_TripId",
                table: "Debt",
                newName: "IX_Debt_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_Debts_DebtorId",
                table: "Debt",
                newName: "IX_Debt_DebtorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debt",
                table: "Debt",
                columns: new[] { "CreditorId", "DebtorId", "TripId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_trip_TripId",
                table: "Debt",
                column: "TripId",
                principalTable: "trip",
                principalColumn: "trip_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_users_CreditorId",
                table: "Debt",
                column: "CreditorId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_users_DebtorId",
                table: "Debt",
                column: "DebtorId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debt_trip_TripId",
                table: "Debt");

            migrationBuilder.DropForeignKey(
                name: "FK_Debt_users_CreditorId",
                table: "Debt");

            migrationBuilder.DropForeignKey(
                name: "FK_Debt_users_DebtorId",
                table: "Debt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Debt",
                table: "Debt");

            migrationBuilder.RenameTable(
                name: "Debt",
                newName: "Debts");

            migrationBuilder.RenameIndex(
                name: "IX_Debt_TripId",
                table: "Debts",
                newName: "IX_Debts_TripId");

            migrationBuilder.RenameIndex(
                name: "IX_Debt_DebtorId",
                table: "Debts",
                newName: "IX_Debts_DebtorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debts",
                table: "Debts",
                columns: new[] { "CreditorId", "DebtorId", "TripId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_trip_TripId",
                table: "Debts",
                column: "TripId",
                principalTable: "trip",
                principalColumn: "trip_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_users_CreditorId",
                table: "Debts",
                column: "CreditorId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_users_DebtorId",
                table: "Debts",
                column: "DebtorId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
