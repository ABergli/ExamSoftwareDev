using ExamSoftwareDesign.Logic.IngredientLogic;
using ExamSoftwareDesign.Logic.RecipeServices;
using ExamSoftwareDesign.Storage;
using ExamSoftwareDesign.UI;
using Microsoft.EntityFrameworkCore;

namespace ExamSoftwareDesign
{
    internal class Program {
        static void Main(string[] args) {
            //Console.WriteLine("Let's go, Exam Cooks!");

            // Set up the path for the SQLite database file
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Data", "ingredients.db");

            // Set up DbContextOptions to pass into IngredientContext
            var options = new DbContextOptionsBuilder<IngredientContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            // Initialize the database context
            IngredientContext context = new(options);

            // Ensure the database is created, creating it if it does not exist
            context.Database.EnsureCreated();

            // Create the repository and service for ingredients
            IngredientRepository repo = new(context);
            IngredientService service = new(repo);

            // Create the refrigerator
            Refrigerator fridge = new("Exam Cooks", service);

            // Create the cookbook for recipes
            Cookbook book = new();

            // Start the user interface, passing in the necessary dependencies
            UserInteractions ui = new(fridge, book);
            ui.Run();
        }
    }
}
