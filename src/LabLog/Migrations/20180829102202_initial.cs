using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LabLog.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabEvents",
                columns: table => new
                {
                    SchoolId = table.Column<Guid>(type: "BLOB", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    EventAuthor = table.Column<string>(type: "TEXT", nullable: true),
                    EventBody = table.Column<string>(type: "TEXT", nullable: true),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabEvents", x => new { x.SchoolId, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    _latestVersion = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SchoolModelId = table.Column<Guid>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomModel_Schools_SchoolModelId",
                        column: x => x.SchoolModelId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComputerModel",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomModelId = table.Column<Guid>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerModel", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_ComputerModel_RoomModel_RoomModelId",
                        column: x => x.RoomModelId,
                        principalTable: "RoomModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "DamageModel",
                columns: table => new
                {
                    DamageId = table.Column<Guid>(type: "BLOB", nullable: false),
                    ComputerModelSerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    GLPITicketNum = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Resolved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageModel", x => x.DamageId);
                    table.ForeignKey(
                        name: "FK_DamageModel_ComputerModel_ComputerModelSerialNumber",
                        column: x => x.ComputerModelSerialNumber,
                        principalTable: "ComputerModel",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputerModel_RoomModelId",
                table: "ComputerModel",
                column: "RoomModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerUserModel_ComputerModelSerialNumber",
                table: "ComputerUserModel",
                column: "ComputerModelSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DamageModel_ComputerModelSerialNumber",
                table: "DamageModel",
                column: "ComputerModelSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RoomModel_SchoolModelId",
                table: "RoomModel",
                column: "SchoolModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerUserModel");

            migrationBuilder.DropTable(
                name: "DamageModel");

            migrationBuilder.DropTable(
                name: "LabEvents");

            migrationBuilder.DropTable(
                name: "ComputerModel");

            migrationBuilder.DropTable(
                name: "RoomModel");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
