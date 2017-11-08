using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LabLog.Migrations
{
    public partial class ComputerUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerUserModel",
                columns: table => new
                {
                    UsernameAssigned = table.Column<string>(type: "TEXT", nullable: false),
                    TimeAssigned = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ComputerModelSerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DetectedUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerUserModel", x => new { x.UsernameAssigned, x.TimeAssigned });
                    table.ForeignKey(
                        name: "FK_ComputerUserModel_ComputerModel_ComputerModelSerialNumber",
                        column: x => x.ComputerModelSerialNumber,
                        principalTable: "ComputerModel",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputerUserModel_ComputerModelSerialNumber",
                table: "ComputerUserModel",
                column: "ComputerModelSerialNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerUserModel");
        }
    }
}
