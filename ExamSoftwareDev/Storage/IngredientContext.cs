using ExamSoftwareDesign.Logic.IngredientLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Emit;

namespace ExamSoftwareDesign.Storage {
    //The IngredientContext acts as the data access layer.
    //It is responsible for interacting with the SQLite database
    //it represents the bridge between the application's domain objects (Ingredient) and the database
    public class IngredientContext : DbContext {

        public IngredientContext(DbContextOptions<IngredientContext> options) : base(options) { }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Configure the table and its properties
            modelBuilder.Entity<Ingredient>(entity => {
                entity.HasKey(i => i.Id); // Primary key
                entity.Property(i => i.Name).IsRequired();
                entity.Property(i => i.Category).HasConversion<string>(); // Convert Enum to string in the DB
                entity.Property(i => i.Amount).IsRequired();
                entity.Property(i => i.Unit).IsRequired();
            });

            // Seed the database with some initial data
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "New potatoes", Category = IngredientCategory.Produce, Amount = 1000, Unit = "g" },
                new Ingredient { Id = 2, Name = "Oil", Category = IngredientCategory.Liquids, Amount = 8, Unit = "tbsp" },
                new Ingredient { Id = 3, Name = "Leeks", Category = IngredientCategory.Produce, Amount = 8, Unit = "large" },
                new Ingredient { Id = 4, Name = "Peppered smoked mackerel", Category = IngredientCategory.Meat, Amount = 400, Unit = "g" },
                new Ingredient { Id = 5, Name = "Eggs", Category = IngredientCategory.Dairy, Amount = 16, Unit = "normal" },
                new Ingredient { Id = 6, Name = "Creamed horseradish", Category = IngredientCategory.Other, Amount = 8, Unit = "tbsp" },
                new Ingredient { Id = 7, Name = "Gnocchi", Category = IngredientCategory.Other, Amount = 1200, Unit = "g" },
                new Ingredient { Id = 8, Name = "Unsalted butter", Category = IngredientCategory.Dairy, Amount = 8, Unit = "tbsp" },
                new Ingredient { Id = 9, Name = "Parmesan", Category = IngredientCategory.Dairy, Amount = 240, Unit = "g" },
                new Ingredient { Id = 10, Name = "Black pepper", Category = IngredientCategory.Spices, Amount = 8, Unit = "tsp" },
                new Ingredient { Id = 11, Name = "Rapeseed oil", Category = IngredientCategory.Liquids, Amount = 4, Unit = "tbsp" },
                new Ingredient { Id = 12, Name = "Red onions", Category = IngredientCategory.Produce, Amount = 8, Unit = "units" },
                new Ingredient { Id = 13, Name = "Kale", Category = IngredientCategory.Produce, Amount = 1200, Unit = "g" },
                new Ingredient { Id = 14, Name = "Wholemeal pasta", Category = IngredientCategory.Other, Amount = 1200, Unit = "g" },
                new Ingredient { Id = 15, Name = "Soft cheese", Category = IngredientCategory.Dairy, Amount = 16, Unit = "tbsp" },
                new Ingredient { Id = 16, Name = "Pesto", Category = IngredientCategory.Other, Amount = 16, Unit = "tbsp" },
                new Ingredient { Id = 17, Name = "Spinach", Category = IngredientCategory.Produce, Amount = 4800, Unit = "g" },
                new Ingredient { Id = 18, Name = "Butter", Category = IngredientCategory.Dairy, Amount = 60, Unit = "g" },
                new Ingredient { Id = 19, Name = "Salmon", Category = IngredientCategory.Meat, Amount = 24, Unit = "fillets" },
                new Ingredient { Id = 20, Name = "Double cream", Category = IngredientCategory.Dairy, Amount = 1200, Unit = "ml" }
            );
        }
    }
}
