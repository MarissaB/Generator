using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generator.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOutposts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artisan",
                columns: table => new
                {
                    ArtisanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artisan", x => x.ArtisanId);
                });

            migrationBuilder.CreateTable(
                name: "Outpost",
                columns: table => new
                {
                    OutpostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReligionCapacity = table.Column<int>(type: "int", nullable: true),
                    ArtisanCapacity = table.Column<int>(type: "int", nullable: true),
                    SpecialtyShopCapacity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outpost", x => x.OutpostId);
                });

            migrationBuilder.CreateTable(
                name: "ReligiousSite",
                columns: table => new
                {
                    ReligiousSiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligiousSite", x => x.ReligiousSiteId);
                });

            migrationBuilder.CreateTable(
                name: "SpecialtyShop",
                columns: table => new
                {
                    SpecialtyShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialtyShop", x => x.SpecialtyShopId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artisan");

            migrationBuilder.DropTable(
                name: "Outpost");

            migrationBuilder.DropTable(
                name: "ReligiousSite");

            migrationBuilder.DropTable(
                name: "SpecialtyShop");
        }
    }
}
