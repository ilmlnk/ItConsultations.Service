using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ItConsultations.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramUrlToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_Consultations_ConsultationId",
                table: "Conferences");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_ConsultationId",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "AssigneeEmails",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MeetingProvider",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MeetingUrl",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ConsultationId",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "AttachmentConsId",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "BeginDateTime",
                table: "Events",
                newName: "StartDateTime");

            migrationBuilder.AddColumn<string>(
                name: "TelegramUrl",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramUrl",
                table: "Students",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConferenceId",
                table: "Events",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ConsultationConsId",
                table: "Conferences",
                type: "character varying(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Conferences",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingProvider",
                table: "Conferences",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachApplicationStatus",
                table: "Coaches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompanyImageUrl",
                table: "Coaches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Coaches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPosition",
                table: "Coaches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "Skills",
                table: "Coaches",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<string>(
                name: "TelegramUrl",
                table: "Coaches",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Topics",
                table: "Coaches",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<string>(
                name: "VideoCardUrl",
                table: "Coaches",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentId",
                table: "Attachments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "ThumbnailId",
                table: "Attachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Consultations_ConsId",
                table: "Consultations",
                column: "ConsId");

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CoachId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Language_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ConferenceId",
                table: "Events",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LocationId",
                table: "Events",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_ConsultationConsId",
                table: "Conferences",
                column: "ConsultationConsId");

            migrationBuilder.CreateIndex(
                name: "IX_Language_CoachId",
                table: "Language",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_Consultations_ConsultationConsId",
                table: "Conferences",
                column: "ConsultationConsId",
                principalTable: "Consultations",
                principalColumn: "ConsId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Conferences_ConferenceId",
                table: "Events",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Location_LocationId",
                table: "Events",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_Consultations_ConsultationConsId",
                table: "Conferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Conferences_ConferenceId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Location_LocationId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Events_ConferenceId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_LocationId",
                table: "Events");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Consultations_ConsId",
                table: "Consultations");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_ConsultationConsId",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "TelegramUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TelegramUrl",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "MeetingProvider",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "CoachApplicationStatus",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CompanyImageUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CompanyPosition",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "TelegramUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "Topics",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "VideoCardUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Events",
                newName: "BeginDateTime");

            migrationBuilder.AddColumn<List<string>>(
                name: "AssigneeEmails",
                table: "Events",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingProvider",
                table: "Events",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingUrl",
                table: "Events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConsultationConsId",
                table: "Conferences",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConsultationId",
                table: "Conferences",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentId",
                table: "Attachments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentConsId",
                table: "Attachments",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_ConsultationId",
                table: "Conferences",
                column: "ConsultationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_Consultations_ConsultationId",
                table: "Conferences",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
