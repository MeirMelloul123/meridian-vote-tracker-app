using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meridian.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberStates",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStates", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssemblyVoteId = table.Column<string>(type: "text", nullable: false),
                    Session = table.Column<int>(type: "integer", nullable: false),
                    VoteDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ResolutionTitle = table.Column<string>(type: "text", nullable: false),
                    DesiredOutcome = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StoredFilePath = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalRows = table.Column<int>(type: "integer", nullable: false),
                    ProcessedRows = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imports_AppUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    MemberFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    MemberTo = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPeriods_MemberStates_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "MemberStates",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conflicts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConflictType = table.Column<string>(type: "text", nullable: false),
                    VoteId = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    CurrentValue = table.Column<string>(type: "text", nullable: false),
                    ProposedValue = table.Column<string>(type: "text", nullable: false),
                    ProposedImportId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConflictStatus = table.Column<string>(type: "text", nullable: false),
                    DecidedByUserId = table.Column<int>(type: "integer", nullable: true),
                    DecidedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conflicts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conflicts_AppUsers_DecidedByUserId",
                        column: x => x.DecidedByUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conflicts_Imports_ProposedImportId",
                        column: x => x.ProposedImportId,
                        principalTable: "Imports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conflicts_MemberStates_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "MemberStates",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Conflicts_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoteId = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    Ballot = table.Column<string>(type: "text", nullable: false),
                    SourceImportId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryVotes_Imports_SourceImportId",
                        column: x => x.SourceImportId,
                        principalTable: "Imports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryVotes_MemberStates_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "MemberStates",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryVotes_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    VoteId = table.Column<int>(type: "integer", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ImportId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryEvents_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryEvents_Imports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Imports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryEvents_MemberStates_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "MemberStates",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_HistoryEvents_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImportRows",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImportId = table.Column<int>(type: "integer", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    AssemblyVoteId = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    Ballot = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportRows_Imports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Imports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conflicts_CountryCode",
                table: "Conflicts",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Conflicts_DecidedByUserId",
                table: "Conflicts",
                column: "DecidedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conflicts_ProposedImportId",
                table: "Conflicts",
                column: "ProposedImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Conflicts_VoteId",
                table: "Conflicts",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryVotes_CountryCode",
                table: "CountryVotes",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_CountryVotes_SourceImportId",
                table: "CountryVotes",
                column: "SourceImportId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryVotes_VoteId_CountryCode",
                table: "CountryVotes",
                columns: new[] { "VoteId", "CountryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEvents_CountryCode",
                table: "HistoryEvents",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEvents_ImportId",
                table: "HistoryEvents",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEvents_UserId",
                table: "HistoryEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryEvents_VoteId",
                table: "HistoryEvents",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportRows_ImportId",
                table: "ImportRows",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_UploadedByUserId",
                table: "Imports",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPeriods_CountryCode",
                table: "MembershipPeriods",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_AssemblyVoteId",
                table: "Votes",
                column: "AssemblyVoteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conflicts");

            migrationBuilder.DropTable(
                name: "CountryVotes");

            migrationBuilder.DropTable(
                name: "HistoryEvents");

            migrationBuilder.DropTable(
                name: "ImportRows");

            migrationBuilder.DropTable(
                name: "MembershipPeriods");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropTable(
                name: "MemberStates");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
