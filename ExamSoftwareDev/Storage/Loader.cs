using System.Text.Json;
using ExamSoftwareDesign.Logic.IngredientLogic;
using ExamSoftwareDesign.Logic.RecipeServices;
using ExamSoftwareDesign.UI;

namespace ExamSoftwareDesign.Storage {
    internal class Loader {
        ExPrint p = new();


        public List<Recipe> LoadRecipe() {

            // Define the path to the JSON file
            //string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipes_bbc.json");
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Data", "recipes_bbc.json");


            // Read the JSON content
            var json = File.ReadAllText(file);

            // use case-insensitive deserialization
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize directly to the Recipe model
            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options);

            return recipes ?? new List<Recipe>();
        }



        public List<Ingredient> LoadIngredient() {
            // Load ingredients stored in a CSV file
            // Each line in the file contains the name, category, amount, and unit of an ingredient
            // Example:
            // New potatoes, Produce, 1000, g, 

            List<Ingredient> fromFile = new List<Ingredient>();

            // Load the ingredients from the file
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "load_ingredients.csv");
            var lines = File.ReadAllLines(file);

            foreach (var line in lines) {
                p.log(0, $"CSV_line: {line}\n"); // [hide?]
                var part = line.Split(',');

                // Create an Ingredient by passing values into the constructor
                Ingredient item = new Ingredient(Int32.Parse(part[0]), part[1], part[2], double.Parse(part[3]), part[4]);

                fromFile.Add(item);
            }
            return fromFile;
        }
    }
}
