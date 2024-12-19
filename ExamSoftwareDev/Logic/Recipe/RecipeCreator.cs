
namespace ExamSoftwareDev.Logic.Recipe {
    internal class CreateRecipeService
    {
        private readonly Cookbook book;
        private List<RecipeIngredient> currentRecipeIngredients;
        private List<string> currentRecipeSteps;

        public CreateRecipeService(Cookbook book)
        {
            this.book = book;
            currentRecipeIngredients = new List<RecipeIngredient>();
            currentRecipeSteps = new List<string>();
        }

        public void AddIngredient(string name, double amount, string unit, string extra)
        {
            var ingredient = new RecipeIngredient(amount, unit, name, extra);
            currentRecipeIngredients.Add(ingredient);
        }

        public void AddStep(string step)
        {
            currentRecipeSteps.Add(step);
        }

        public void CreateRecipe(string title, string link)
        {
            var newRecipe = new Recipe(link, title, currentRecipeIngredients, currentRecipeSteps);
            book.AddRecipe(newRecipe);

            currentRecipeIngredients.Clear();
            currentRecipeSteps.Clear();
        }
    }
}
