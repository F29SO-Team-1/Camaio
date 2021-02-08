using Microsoft.EntityFrameworkCore.Migrations;

namespace Login.Migrations.Thread
{
    public partial class ThreadsUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Thread",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Thread");
        }
    }
}
