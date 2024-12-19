using ExamSoftwareDev.Storage;

namespace ExamSoftwareDev.Logic.Recipe {
    public class Cookbook
    {
        public List<Recipe> CookbookRecipes { get; private set; } = new List<Recipe>();
        private readonly RecipeRepository repository;

        public Cookbook()
        {
            repository = new RecipeRepository();
            CookbookRecipes = repository.LoadRecipes();
        }

        public Cookbook(List<Recipe> recipes) // for unit testing
        {
            repository = new RecipeRepository();
            CookbookRecipes = recipes;
        }

        public void AddRecipe(Recipe recipe)
        {
            CookbookRecipes.Add(recipe);
            repository.SaveRecipes(CookbookRecipes);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            CookbookRecipes.Remove(recipe);
            repository.SaveRecipes(CookbookRecipes);
        }

        public void Clear()
        {
            CookbookRecipes.Clear();
            repository.SaveRecipes(CookbookRecipes);
        }

        public List<string> GetRecipesAsStrings()
        {
            return CookbookRecipes.Select(recipe => recipe.ToString()).ToList();
        }
    }
}
