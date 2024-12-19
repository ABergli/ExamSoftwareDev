using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExamSoftwareDev.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Amount", "Category", "Name", "Unit" },
                values: new object[,]
                {
                    { 1, 1000.0, "Produce", "New potatoes", "g" },
                    { 2, 8.0, "Liquids", "Oil", "tbsp" },
                    { 3, 8.0, "Produce", "Leeks", "large" },
                    { 4, 400.0, "Meat", "Peppered smoked mackerel", "g" },
                    { 5, 16.0, "Dairy", "Eggs", "normal" },
                    { 6, 8.0, "Other", "Creamed horseradish", "tbsp" },
                    { 7, 1200.0, "Other", "Gnocchi", "g" },
                    { 8, 8.0, "Dairy", "Unsalted butter", "tbsp" },
                    { 9, 240.0, "Dairy", "Parmesan", "g" },
                    { 10, 8.0, "Spices", "Black pepper", "tsp" },
                    { 11, 4.0, "Liquids", "Rapeseed oil", "tbsp" },
                    { 12, 8.0, "Produce", "Red onions", "units" },
                    { 13, 1200.0, "Produce", "Kale", "g" },
                    { 14, 1200.0, "Other", "Wholemeal pasta", "g" },
                    { 15, 16.0, "Dairy", "Soft cheese", "tbsp" },
                    { 16, 16.0, "Other", "Pesto", "tbsp" },
                    { 17, 4800.0, "Produce", "Spinach", "g" },
                    { 18, 60.0, "Dairy", "Butter", "g" },
                    { 19, 24.0, "Meat", "Salmon", "fillets" },
                    { 20, 1200.0, "Dairy", "Double cream", "ml" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");
        }
    }
}
