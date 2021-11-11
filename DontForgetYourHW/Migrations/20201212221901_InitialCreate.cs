using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DontForgetYourHW.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anime",
                columns: table => new
                {
                    AnimeId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source = table.Column<int>(nullable: false),
                    SourceLink = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameLink = table.Column<string>(nullable: true),
                    EpisodeLink = table.Column<string>(nullable: true),
                    LatestEpisode = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anime", x => x.AnimeId);
                });

            migrationBuilder.CreateTable(
                name: "Artifact",
                columns: table => new
                {
                    ArtifactId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    Due = table.Column<DateTime>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false),
                    IsRemindEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifact", x => x.ArtifactId);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: true),
                    CourseName = table.Column<string>(nullable: true),
                    DayIndex = table.Column<int>(nullable: false),
                    Days = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Crystal",
                columns: table => new
                {
                    ArtifactId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    Due = table.Column<DateTime>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false),
                    IsRemindEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crystal", x => x.ArtifactId);
                });

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    DomainId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false),
                    Due = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialImage = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.DomainId);
                });

            migrationBuilder.CreateTable(
                name: "Homework",
                columns: table => new
                {
                    HomeworkId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    CourseName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Instruction = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsRemindEnabled = table.Column<bool>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false),
                    Due = table.Column<DateTime>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homework", x => x.HomeworkId);
                });

            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manga",
                columns: table => new
                {
                    MangaId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source = table.Column<int>(nullable: false),
                    SourceLink = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameLink = table.Column<string>(nullable: true),
                    LatestEpisode = table.Column<int>(nullable: false),
                    GuildChannelId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manga", x => x.MangaId);
                });

            migrationBuilder.CreateTable(
                name: "Resin",
                columns: table => new
                {
                    ResinId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    ReminderAt = table.Column<int>(nullable: false),
                    Due = table.Column<DateTime>(nullable: true),
                    GuildChannelId = table.Column<long>(nullable: false),
                    IsRemindEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resin", x => x.ResinId);
                });

            migrationBuilder.CreateTable(
                name: "Timezone",
                columns: table => new
                {
                    TimezoneId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    Abbrevlation = table.Column<string>(nullable: true),
                    TimezoneString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezone", x => x.TimezoneId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anime");

            migrationBuilder.DropTable(
                name: "Artifact");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Crystal");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.DropTable(
                name: "Homework");

            migrationBuilder.DropTable(
                name: "Link");

            migrationBuilder.DropTable(
                name: "Manga");

            migrationBuilder.DropTable(
                name: "Resin");

            migrationBuilder.DropTable(
                name: "Timezone");
        }
    }
}
