using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class debtTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Debt_TripId",
                table: "Debt");

            migrationBuilder.RenameTable(
                name: "Debt",
                newName: "debt");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "debt",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "debt",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "debt",
                newName: "trip_id");

            migrationBuilder.RenameColumn(
                name: "DebtorId",
                table: "debt",
                newName: "debtor_id");

            migrationBuilder.RenameColumn(
                name: "CreditorId",
                table: "debt",
                newName: "creditor_id");

            migrationBuilder.RenameIndex(
                name: "IX_Debt_DebtorId",
                table: "debt",
                newName: "IX_debt_debtor_id");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "debt",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_debt",
                table: "debt",
                columns: new[] { "trip_id", "debtor_id", "creditor_id" });

            migrationBuilder.CreateIndex(
                name: "IX_debt_creditor_id",
                table: "debt",
                column: "creditor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_debt_trip_trip_id",
                table: "debt",
                column: "trip_id",
                principalTable: "trip",
                principalColumn: "trip_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_debt_users_creditor_id",
                table: "debt",
                column: "creditor_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_debt_users_debtor_id",
                table: "debt",
                column: "debtor_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_debt_trip_trip_id",
                table: "debt");

            migrationBuilder.DropForeignKey(
                name: "FK_debt_users_creditor_id",
                table: "debt");

            migrationBuilder.DropForeignKey(
                name: "FK_debt_users_debtor_id",
                table: "debt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_debt",
                table: "debt");

            migrationBuilder.DropIndex(
                name: "IX_debt_creditor_id",
                table: "debt");

            migrationBuilder.RenameTable(
                name: "debt",
                newName: "Debt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Debt",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Debt",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "creditor_id",
                table: "Debt",
                newName: "CreditorId");

            migrationBuilder.RenameColumn(
                name: "debtor_id",
                table: "Debt",
                newName: "DebtorId");

            migrationBuilder.RenameColumn(
                name: "trip_id",
                table: "Debt",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_debt_debtor_id",
                table: "Debt",
                newName: "IX_Debt_DebtorId");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Debt",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Debt",
                table: "Debt",
                columns: new[] { "CreditorId", "DebtorId", "TripId" });

            migrationBuilder.CreateIndex(
                name: "IX_Debt_TripId",
                table: "Debt",
                column: "TripId");

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
    }
}
