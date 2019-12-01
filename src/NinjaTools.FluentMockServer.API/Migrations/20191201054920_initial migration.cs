using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NinjaTools.FluentMockServer.API.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "httpError",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Delay = table.Column<string>(nullable: true),
                    DropConnection = table.Column<bool>(nullable: true),
                    ResponseBytes = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_httpError", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "httpForward",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Host = table.Column<string>(maxLength: 100, nullable: true),
                    Port = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_httpForward", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "httpRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Method = table.Column<string>(maxLength: 20, nullable: true),
                    Body = table.Column<string>(type: "NVARCHAR", nullable: true),
                    Path = table.Column<string>(maxLength: 100, nullable: true),
                    Secure = table.Column<bool>(nullable: true),
                    KeepAlive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_httpRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "httpResponse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusCode = table.Column<int>(nullable: true),
                    Delay = table.Column<string>(nullable: true),
                    Body = table.Column<string>(type: "NVARCHAR", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_httpResponse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "httpTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Template = table.Column<string>(nullable: true),
                    Delay = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_httpTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lifeTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false),
                    TimeUnit = table.Column<ushort>(nullable: true),
                    TimeToLive = table.Column<int>(nullable: true),
                    Unlimited = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lifeTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "times",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false),
                    RemainingTimes = table.Column<int>(nullable: false),
                    Unlimited = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_times", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "verify",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HttpRequestId = table.Column<int>(nullable: true),
                    Times = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verify", x => x.Id);
                    table.ForeignKey(
                        name: "FK_verify_httpRequest_HttpRequestId",
                        column: x => x.HttpRequestId,
                        principalTable: "httpRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "expectation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HttpRequestId = table.Column<int>(nullable: true),
                    HttpResponseId = table.Column<int>(nullable: true),
                    HttpForwardId = table.Column<int>(nullable: true),
                    HttpErrorId = table.Column<int>(nullable: true),
                    TimesId = table.Column<int>(nullable: true),
                    TimeToLiveId = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "TimeStamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expectation_httpError_HttpErrorId",
                        column: x => x.HttpErrorId,
                        principalTable: "httpError",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expectation_httpForward_HttpForwardId",
                        column: x => x.HttpForwardId,
                        principalTable: "httpForward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expectation_httpRequest_HttpRequestId",
                        column: x => x.HttpRequestId,
                        principalTable: "httpRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expectation_httpResponse_HttpResponseId",
                        column: x => x.HttpResponseId,
                        principalTable: "httpResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expectation_lifeTime_TimeToLiveId",
                        column: x => x.TimeToLiveId,
                        principalTable: "lifeTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expectation_times_TimesId",
                        column: x => x.TimesId,
                        principalTable: "times",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_expectation_HttpErrorId",
                table: "expectation",
                column: "HttpErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_expectation_HttpForwardId",
                table: "expectation",
                column: "HttpForwardId");

            migrationBuilder.CreateIndex(
                name: "IX_expectation_HttpRequestId",
                table: "expectation",
                column: "HttpRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_expectation_HttpResponseId",
                table: "expectation",
                column: "HttpResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_expectation_TimeToLiveId",
                table: "expectation",
                column: "TimeToLiveId");

            migrationBuilder.CreateIndex(
                name: "IX_expectation_TimesId",
                table: "expectation",
                column: "TimesId");

            migrationBuilder.CreateIndex(
                name: "IX_httpError_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "httpError",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_httpForward_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "httpForward",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_httpRequest_Path",
                table: "httpRequest",
                column: "Path");

            migrationBuilder.CreateIndex(
                name: "IX_httpRequest_Path_Id_Method_CreatedOn",
                table: "httpRequest",
                columns: new[] { "Path", "Id", "Method", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_httpResponse_Id_Timestamp_CreatedOn_ModifiedOn_StatusCode",
                table: "httpResponse",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn", "StatusCode" });

            migrationBuilder.CreateIndex(
                name: "IX_httpTemplate_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "httpTemplate",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_lifeTime_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "lifeTime",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_times_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "times",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_verify_HttpRequestId",
                table: "verify",
                column: "HttpRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_verify_Id_Timestamp_CreatedOn_ModifiedOn",
                table: "verify",
                columns: new[] { "Id", "Timestamp", "CreatedOn", "ModifiedOn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expectation");

            migrationBuilder.DropTable(
                name: "httpTemplate");

            migrationBuilder.DropTable(
                name: "verify");

            migrationBuilder.DropTable(
                name: "httpError");

            migrationBuilder.DropTable(
                name: "httpForward");

            migrationBuilder.DropTable(
                name: "httpResponse");

            migrationBuilder.DropTable(
                name: "lifeTime");

            migrationBuilder.DropTable(
                name: "times");

            migrationBuilder.DropTable(
                name: "httpRequest");
        }
    }
}
