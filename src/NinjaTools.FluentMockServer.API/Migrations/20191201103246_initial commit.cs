using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NinjaTools.FluentMockServer.API.Migrations
{
    public partial class initialcommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expectations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HttpRequest_Method = table.Column<string>(maxLength: 20, nullable: true),
                    HttpRequest_Headers = table.Column<string>(nullable: true),
                    HttpRequest_Cookies = table.Column<string>(nullable: true),
                    HttpRequest_Body = table.Column<string>(nullable: true),
                    HttpRequest_Path = table.Column<string>(maxLength: 100, nullable: true),
                    HttpRequest_Secure = table.Column<bool>(nullable: true),
                    HttpRequest_KeepAlive = table.Column<bool>(nullable: true),
                    HttpRequest_Id = table.Column<int>(nullable: true),
                    HttpRequest_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpRequest_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpRequest_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpResponse_StatusCode = table.Column<int>(nullable: true),
                    HttpResponse_Delay = table.Column<string>(nullable: true),
                    HttpResponse_ConnectionOptions_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpResponse_ConnectionOptions_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpResponse_ConnectionOptions_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpResponse_ConnectionOptions_CloseSocket = table.Column<bool>(nullable: true),
                    HttpResponse_ConnectionOptions_ContentLengthHeaderOverride = table.Column<long>(nullable: true),
                    HttpResponse_ConnectionOptions_SuppressContentLengthHeader = table.Column<bool>(nullable: true),
                    HttpResponse_ConnectionOptions_SuppressConnectionHeader = table.Column<bool>(nullable: true),
                    HttpResponse_ConnectionOptions_KeepAliveOverride = table.Column<bool>(nullable: true),
                    HttpResponse_Body = table.Column<string>(nullable: true),
                    HttpResponse_Headers = table.Column<string>(nullable: true),
                    HttpResponse_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpResponse_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpResponse_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpResponseTemplate_Template = table.Column<string>(nullable: true),
                    HttpResponseTemplate_Delay = table.Column<string>(nullable: true),
                    HttpResponseTemplate_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpResponseTemplate_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpResponseTemplate_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpForward_Host = table.Column<string>(maxLength: 100, nullable: true),
                    HttpForward_Port = table.Column<int>(nullable: true),
                    HttpForward_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpForward_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpForward_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpForwardTemplate_Template = table.Column<string>(nullable: true),
                    HttpForwardTemplate_Delay = table.Column<string>(nullable: true),
                    HttpForwardTemplate_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpForwardTemplate_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpForwardTemplate_Timestamp = table.Column<byte[]>(nullable: true),
                    HttpError_Delay = table.Column<string>(nullable: true),
                    HttpError_DropConnection = table.Column<bool>(nullable: true),
                    Base64Response = table.Column<string>(nullable: true),
                    HttpError_CreatedOn = table.Column<DateTime>(nullable: true),
                    HttpError_ModifiedOn = table.Column<DateTime>(nullable: true),
                    HttpError_Timestamp = table.Column<byte[]>(nullable: true),
                    Times_CreatedOn = table.Column<DateTime>(nullable: true),
                    Times_ModifiedOn = table.Column<DateTime>(nullable: true),
                    Times_Timestamp = table.Column<byte[]>(nullable: true),
                    Times_RemainingTimes = table.Column<int>(nullable: true),
                    Times_Unlimited = table.Column<bool>(nullable: true),
                    TimeToLive_CreatedOn = table.Column<DateTime>(nullable: true),
                    TimeToLive_ModifiedOn = table.Column<DateTime>(nullable: true),
                    TimeToLive_Timestamp = table.Column<byte[]>(nullable: true),
                    TimeToLive_TimeUnit = table.Column<int>(nullable: true),
                    TimeToLive_TimeToLive = table.Column<int>(nullable: true),
                    TimeToLive_Unlimited = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()"),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expectations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expectations_Id_CreatedOn_Timestamp",
                table: "Expectations",
                columns: new[] { "Id", "CreatedOn", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Expectations_HttpRequest_Body",
                table: "Expectations",
                column: "HttpRequest_Body");

            migrationBuilder.CreateIndex(
                name: "IX_Expectations_HttpRequest_Method",
                table: "Expectations",
                column: "HttpRequest_Method");

            migrationBuilder.CreateIndex(
                name: "IX_Expectations_HttpRequest_Path",
                table: "Expectations",
                column: "HttpRequest_Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expectations");
        }
    }
}
