using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orion.DAL.EF.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AccessGroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RoleName = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TaskName = table.Column<string>(nullable: true),
                    Duration = table.Column<decimal>(type: "decimal(18, 1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: true),
                    ChangePasswordOnNextLogin = table.Column<bool>(nullable: true),
                    EmployeeNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    LockoutEnabled = table.Column<bool>(nullable: true),
                    AppointmentDate = table.Column<DateTime>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    AccessGroupId = table.Column<int>(nullable: true),
                    ProfilePicture = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AccessGroup",
                        column: x => x.AccessGroupId,
                        principalTable: "AccessGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CapturedTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    Color = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapturedTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CapturedTime_Task",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapturedTime_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccessGroup",
                columns: new[] { "Id", "AccessGroupName", "CreatedAt", "CreatedBy", "IsDeleted", "LastUpdatedAt", "LastUpdatedBy" },
                values: new object[,]
                {
                    { 1, "Admin", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1 },
                    { 2, "User", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1 }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "IsDeleted", "LastUpdatedAt", "LastUpdatedBy", "Rate", "RoleName" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, 25m, "Casual Employee Level 1" },
                    { 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, 50m, "Casual Employee Level 2" },
                    { 3, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, 50m, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Duration", "IsDeleted", "LastUpdatedAt", "LastUpdatedBy", "TaskName" },
                values: new object[] { 1, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, 1m, false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Default Task" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessGroupId", "AppointmentDate", "ChangePasswordOnNextLogin", "CreatedAt", "CreatedBy", "Email", "EmployeeNumber", "FirstName", "IsActive", "IsDeleted", "LastName", "LastUpdatedAt", "LastUpdatedBy", "LockoutEnabled", "PasswordHash", "ProfilePicture", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "UserName" },
                values: new object[,]
                {
                    { 3, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "amanda.beukes@orion.com", "EMP00002", "Amanda", true, false, "Beukes", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 1, "Amanda01" },
                    { 6, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Jackie.Oswalt@orion.com", "EMP00005", "Jackie", true, false, "Oswalt", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 1, "JackieO" },
                    { 4, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Sandra.Swart@orion.com", "EMP00003", "Sandra", true, false, "Swart", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 2, "SandyS" },
                    { 5, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Carl.Ross@orion.com", "EMP00004", "Carl", true, false, "Ross", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 2, "Carl001" },
                    { 8, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Grace.Winters@orion.com", "EMP00007", "Grace", true, false, "Winters", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 2, "Winters01" },
                    { 9, 2, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Gert.Lombard@orion.com", "EMP00008", "Gert", true, false, "Lombard", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 2, "lombard01" },
                    { 1, 1, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "admin@admin.com", "XXXXXXXXX", "Root", true, false, "Admin", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 3, "SuperUser" },
                    { 2, 1, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "jan.groenewald@orion.com", "EMP00001", "Jan", true, false, "Groenewald", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 3, "JGroenewald" },
                    { 7, 1, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), false, new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, "Loy.McDonald@orion.com", "EMP00006", "Loy", true, false, "McDonald", new DateTime(2021, 3, 3, 0, 0, 0, 0, DateTimeKind.Local), 1, false, "CWa/NbR02Squq2Np65dn4Q==", null, null, null, 3, "LMcD01" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CapturedTime_TaskId",
                table: "CapturedTime",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CapturedTime_UserId",
                table: "CapturedTime",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccessGroupId",
                table: "User",
                column: "AccessGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CapturedTime");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AccessGroup");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
