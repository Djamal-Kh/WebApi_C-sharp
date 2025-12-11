using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployeeIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_animals_employees_EmployeeId",
                table: "animals");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "animals",
                newName: "employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_animals_EmployeeId",
                table: "animals",
                newName: "IX_animals_employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_animals_employees_employee_id",
                table: "animals",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_animals_employees_employee_id",
                table: "animals");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "animals",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_animals_employee_id",
                table: "animals",
                newName: "IX_animals_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_animals_employees_EmployeeId",
                table: "animals",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
