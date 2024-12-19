namespace ExamSoftwareDesign.Logic.RecipeServices {
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




    public class RecipeIngredient {
        public string Name { get; private set; } // Ingredient name
        //public IngredientCategory Category { get; private set; }
        public double Amount { get; private set; } // Quantity required
        public string Unit { get; private set; } // Unit of measurement
        public string Extra { get; private set; } // Optional notes (e.g., "chopped finely")

        public RecipeIngredient(double amount, string unit, string name, string extra = null) {
            Amount = amount;
            Unit = unit;
            Name = name;
            Extra = extra;
        }

        // Public methods to update properties
        public void UpdateName(string newName) {
            if (!string.IsNullOrWhiteSpace(newName)) {
                Name = newName;
            }
        }

        public void UpdateAmount(double newAmount) {
            if (newAmount > 0) {
                Amount = newAmount;
            }
        }

        public void UpdateUnit(string newUnit) {
            if (!string.IsNullOrWhiteSpace(newUnit)) {
                Unit = newUnit;
            }
        }

        public void UpdateExtra(string newExtra) {
            Extra = newExtra;
        }

        public override string ToString() {
            return $"{Name} ({Amount} {Unit}) {Extra}";
        }
    }
}
