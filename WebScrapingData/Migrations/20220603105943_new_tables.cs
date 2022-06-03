using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebScrapingData.Migrations
{
    public partial class new_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    id_product = table.Column<string>(type: "TEXT", nullable: false),
                    product_name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.id_product);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id_review = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    comment = table.Column<string>(type: "TEXT", nullable: true),
                    stars = table.Column<int>(type: "INTEGER", nullable: false),
                    review_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id_review);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
