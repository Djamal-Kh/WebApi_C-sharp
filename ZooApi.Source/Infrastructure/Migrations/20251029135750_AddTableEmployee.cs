using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTableEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AnimalsOfZoo",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeesOfZoo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesOfZoo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalsOfZoo_EmployeeId",
                table: "AnimalsOfZoo",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalsOfZoo_EmployeesOfZoo_EmployeeId",
                table: "AnimalsOfZoo",
                column: "EmployeeId",
                principalTable: "EmployeesOfZoo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalsOfZoo_EmployeesOfZoo_EmployeeId",
                table: "AnimalsOfZoo");

            migrationBuilder.DropTable(
                name: "EmployeesOfZoo");

            migrationBuilder.DropIndex(
                name: "IX_AnimalsOfZoo_EmployeeId",
                table: "AnimalsOfZoo");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AnimalsOfZoo");
        }
    }
}
