using ExamSoftwareDev.Logic.Ingredient;

namespace ExamSoftwareDev.Logic.Recipe {
    internal class RecipeService
    {
        Cookbook book;
        Refrigerator fridge;

        internal RecipeService(Cookbook book, Refrigerator fridge)
        {
            this.book = book;
            this.fridge = fridge;
        }

        public List<Recipe> GetAllRecipesSortedByTitle()
        {
            return book.CookbookRecipes.OrderBy(recipe => recipe.Title).ToList();

        }
        public Recipe GetSelectedRecipe(List<Recipe> searchResults, int selectedIndex)
        {
            if (selectedIndex >= 1 && selectedIndex <= searchResults.Count)
            {
                return searchResults[selectedIndex - 1];
            }
            throw new ArgumentOutOfRangeException(nameof(selectedIndex), "Invalid selection index.");
        }



        public void AddIngredient(Recipe recipe, double amount, string unit, string name, string extra)
        {
            var ingredient = new RecipeIngredient(amount, unit, name, extra);
            recipe.Ingredients.Add(ingredient);
        }

        public void UpdateIngredient(RecipeIngredient ingredient, string newName, double? newAmount, string newUnit, string newExtra)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                ingredient.UpdateName(newName);
            }

            if (newAmount.HasValue)
            {
                ingredient.UpdateAmount(newAmount.Value);
            }

            if (!string.IsNullOrWhiteSpace(newUnit))
            {
                ingredient.UpdateUnit(newUnit);
            }

            if (!string.IsNullOrWhiteSpace(newExtra))
            {
                ingredient.UpdateExtra(newExtra);
            }
        }

        public void RemoveIngredient(Recipe recipe, int index)
        {
            if (index >= 0 && index < recipe.Ingredients.Count)
            {
                recipe.Ingredients.RemoveAt(index);
            }
        }

        public void UpdateStep(Recipe recipe, int index, string newStep)
        {
            if (index >= 0 && index < recipe.Steps.Count && !string.IsNullOrWhiteSpace(newStep))
            {
                recipe.Steps[index] = newStep;
            }
        }

        public void RemoveStep(Recipe recipe, int index)
        {
            if (index >= 0 && index < recipe.Steps.Count)
            {
                recipe.Steps.RemoveAt(index);
            }
        }

        public List<Recipe> SearchRecipes(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Search query cannot be empty.");
            }

            string lowerQuery = query.ToLower();

            var titleMatches = book.CookbookRecipes
                .Where(r => r.Title != null && r.Title.ToLower().Contains(lowerQuery))
                .ToList();

            var stepMatches = book.CookbookRecipes
                .Where(r => !titleMatches.Contains(r) && r.Steps != null && r.Steps.Any(step => step.ToLower().Contains(lowerQuery)))
                .ToList();

            return titleMatches.Concat(stepMatches).ToList();
        }
        public DeductResult TryDeductIngredient(RecipeIngredient recipeIngredient, double amountToSpend)
        {
            // Validate input
            if (amountToSpend < 0)
            {
                return DeductResult.InvalidInput;
            }
            else if (amountToSpend == 0)
            {
                return DeductResult.Skipped;
            }

            // Fetch the ingredient from the fridge
            var fridgeIngredient = fridge.GetIngredientsByName(recipeIngredient.Name)
                .FirstOrDefault(ingredient =>
                    ingredient.Unit.Equals(recipeIngredient.Unit, StringComparison.OrdinalIgnoreCase));

            if (fridgeIngredient == null)
            {
                return DeductResult.NotFound;
            }

            if (fridgeIngredient.Amount < amountToSpend)
            {
                // Not enough available
                return DeductResult.Insufficient(fridgeIngredient.Amount);
            }

            // Deduct the amount
            double newAmount = fridgeIngredient.Amount - amountToSpend;
            fridge.UpdateIngredient(
                fridgeIngredient.Id,
                fridgeIngredient.Name,
                fridgeIngredient.Category.ToString(),
                newAmount,
                fridgeIngredient.Unit
            );

            return DeductResult.Success;
        }
        public class DeductResult
        {
            public bool IsSuccess { get; private set; }
            public bool IsInvalidInput { get; private set; }
            public bool IsNotFound { get; private set; }
            public bool IsInsufficient { get; private set; }
            public bool IsSkipped { get; private set; }
            public double AvailableAmount { get; private set; }


            public static DeductResult Success
                => new DeductResult { IsSuccess = true };
            public static DeductResult InvalidInput
                => new DeductResult { IsInvalidInput = true };
            public static DeductResult NotFound
                => new DeductResult { IsNotFound = true };
            public static DeductResult Skipped 
                => new DeductResult { IsSkipped = true};
        public static DeductResult Insufficient(double availableAmount)
                => new DeductResult { IsInsufficient = true, AvailableAmount = availableAmount };
        }

        public bool AllIngredientsAvailable(Recipe recipe)
        {
            return !GetMissingIngredients(recipe).Any();
        }
        public List<RecipeIngredient> GetMissingIngredients(Recipe recipe)
        {
            var missingIngredients = recipe.Ingredients
                .Where(recipeIngredient =>
                {
                    var fridgeIngredients = fridge.GetIngredientsByName(recipeIngredient.Name);
                    var matchingIngredient = fridgeIngredients.FirstOrDefault(ingredient =>
                        ingredient.Unit.Equals(recipeIngredient.Unit, StringComparison.OrdinalIgnoreCase) &&
                        ingredient.Amount >= recipeIngredient.Amount);

                    return matchingIngredient == null;
                })
                .ToList();

            return missingIngredients;
        }

        public List<string> GetAllRecipesAsStrings()
        {
            return book.GetRecipesAsStrings();
        }
    }
}
