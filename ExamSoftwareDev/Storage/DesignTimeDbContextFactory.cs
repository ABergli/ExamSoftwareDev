using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExamSoftwareDev.Storage {
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IngredientContext> {
        public IngredientContext CreateDbContext(string[] args) {
            // Set up the options for DbContext
            var optionsBuilder = new DbContextOptionsBuilder<IngredientContext>();

            // Use the same connection string as in your Program.cs
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Data", "ingredients.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new IngredientContext(optionsBuilder.Options);
        }
    }
}
