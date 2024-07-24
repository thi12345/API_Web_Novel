﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCrud.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoryandGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenreStory",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    StoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreStory", x => new { x.GenresId, x.StoriesId });
                    table.ForeignKey(
                        name: "FK_GenreStory_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreStory_Stories_StoriesId",
                        column: x => x.StoriesId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreStory_StoriesId",
                table: "GenreStory",
                column: "StoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreStory");
        }
    }
}
