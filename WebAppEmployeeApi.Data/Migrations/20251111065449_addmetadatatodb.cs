using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEmployeeApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class addmetadatatodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EmployeesAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeesAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "EmployeesAddress",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EmployeesAddress",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmployeesAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EmployeesAddress");
        }
    }
}
