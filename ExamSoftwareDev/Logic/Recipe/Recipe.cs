using ExamSoftwareDev.Logic.Ingredient;


namespace ExamSoftwareDev.Logic.Recipe {
    public class Recipe {
        public string Title { get; set; }
        public string Link { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe() { }
        public Recipe(string link, string title, List<RecipeIngredient> ingredients, List<string> steps) {
            Link = link;
            Title = title;
            Ingredients = ingredients;
            Steps = steps;
        }


        public override string ToString() {
            var ingredientsList = string.Join("\n", Ingredients.Where(i => i != null).Select(i => i.ToString()));

            return $"\n\n{Title}:\nIngredients:\n{ingredientsList}";
        }
    }
}
