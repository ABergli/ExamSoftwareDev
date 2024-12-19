using System.Text.Json;
using ExamSoftwareDev.Logic.Recipe;

namespace ExamSoftwareDev.Storage {
    internal class RecipeRepository
    {
        private readonly Loader loader;
        private readonly string filePath;

        public RecipeRepository()
        {
            loader = new Loader();
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Data", "recipes_bbc.json");
        }

        public List<Recipe> LoadRecipes() // Load recipes
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<Recipe>();
                }

                return loader.LoadRecipe();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while loading recipes.", ex);
            }
        }

        public void SaveRecipes(List<Recipe> recipes) // Save recipes
        {
            try
            {
                string jsonCookbook = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonCookbook);
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while saving recipes.", ex);
            }
        }
    }
}
