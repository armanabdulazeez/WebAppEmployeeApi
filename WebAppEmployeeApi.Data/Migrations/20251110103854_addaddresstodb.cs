using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppEmployeeApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class addaddresstodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeesAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HouseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Place = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeesAddress_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesAddress_EmployeeId",
                table: "EmployeesAddress",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesAddress");
        }
    }
}
