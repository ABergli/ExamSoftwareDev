using ExamSoftwareDev.Logic.Ingredient;
using ExamSoftwareDev.Logic.Recipe;


namespace ExamSoftwareDev.UI.InteractionsRecipe {
    public class RecipeUIHandler
    {
        ExPrint p;
        Refrigerator fridge;
        Cookbook book;
        private readonly RecipeService service;
        private readonly CreateRecipeService createrecipe;

        internal RecipeUIHandler(RecipeService service, ExPrint p, CreateRecipeService createrecipe, Refrigerator fridge, Cookbook cookbook)
        {
            this.service = service;
            this.p = p;
            this.createrecipe = createrecipe;
            this.fridge = fridge;
            this.book = cookbook;
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void ViewAllRecipes()
        {
            var sortedRecipes = service.GetAllRecipesSortedByTitle();

            string output = "List of Recipes:\n";
            int lineNumber = 1;
            foreach (var recipe in sortedRecipes)
            {
                output += $"{lineNumber}. {recipe.Title}\n";
                lineNumber++;
            }

            p.print(output);

            p.print("\nDo you want to select a recipe? (Y/n):", "white");
            string confirmation = Console.ReadLine()?.Trim();

            if (confirmation?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
            {
                SelectFromList(sortedRecipes);
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void Display(Recipe recipe)
        {
            p.print($"\n--- Recipe: {recipe.Title} ---", "cyan");
            p.print($"\nLink: {recipe.Link}\n", "cyan");

            p.print("\nIngredients:", "yellow");
            foreach (var ingredient in recipe.Ingredients)
            {
                p.print($"\n- {ingredient.Name}: {ingredient.Amount} {ingredient.Unit} {ingredient.Extra}");
            }

            p.print("\nSteps:", "yellow");
            int stepNumber = 1;
            foreach (var step in recipe.Steps)
            {
                p.print($"\n{stepNumber}. {step}");
                stepNumber++;
            }
            p.print("\n--------------------------", "cyan");
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void PerformSearch()
        {
            p.print("Enter your search query:", "white");
            string query = Console.ReadLine()?.Trim();

            try
            {
                var results = service.SearchRecipes(query);

                if (results.Any())
                {
                    string output = $"Found {results.Count} recipe(s) matching '{query}':\n";

                    int lineNumber = 1;
                    foreach (var recipe in results)
                    {
                        output += $"{lineNumber}. {recipe.Title}\n";
                        lineNumber++;
                    }

                    p.print(output, "yellow");
                }
                else
                {
                    p.print($"\nNo recipes found matching '{query}'.", "red");
                }
            }
            catch (ArgumentException ex)
            {
                p.print($"\nError: {ex.Message}", "red");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void Search()
        {
            while (true)
            {
                p.print("\nEnter your search query: ", "white");
                string query = Console.ReadLine()?.Trim();

                List<Recipe> combinedResults;
                try
                {
                    combinedResults = service.SearchRecipes(query);
                }
                catch (ArgumentException ex)
                {
                    p.print($"\nError: {ex.Message}", "red");
                    continue;
                }

                if (combinedResults.Any())
                {
                    p.print($"\nFound {combinedResults.Count} recipe(s):", "yellow");
                    int lineNumber = 1;
                    foreach (var recipe in combinedResults)
                    {
                        p.print($"\n{lineNumber}. {recipe.Title}");
                        lineNumber++;
                    }

                    bool continueOptionsSearch = SearchMenu(combinedResults);
                    if (!continueOptionsSearch)
                    {
                        break;
                    }
                }
                else
                {
                    p.print("\n1 - Search Again", "yellow");
                    p.print("\n0 - Exit to Main Menu", "red");
                    p.print("\nChoose an option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            continue;
                        case "0":
                            return;
                        default:
                            p.print("\nInvalid option. Please try again.", "white");
                            break;
                    }
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void SelectFromList(List<Recipe> recipes)
        {
            p.print("\nEnter the index-number of the recipe you want to select:", "white");
            if (int.TryParse(Console.ReadLine(), out int selectedIndex))
            {
                try
                {
                    Recipe selectedRecipe = service.GetSelectedRecipe(recipes, selectedIndex);
                    ManageSelectedRecipe(selectedRecipe);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    p.print($"\nError: {ex.Message}", "red");
                }
            }
            else
            {
                p.print("\nInvalid input. Please enter a valid number.", "red");
            }
        }


        // ------------------------------------------------------------------------------------------------------------------ //

        public void DeleteRecipe(Cookbook book, Recipe recipe)
        {
            if (book == null || recipe == null)
            {
                p.print("Cookbook or recipe is null. Cannot delete recipe.", "red");
                return;
            }

            p.print($"Are you sure you want to delete the recipe '{recipe.Title}'? (Y/n):", "white");
            string confirmation = Console.ReadLine()?.Trim();

            if (confirmation == "Y" || confirmation == "Yes" || confirmation == "yes")
            {
                try
                {
                    book.RemoveRecipe(recipe);
                    p.print($"Recipe '{recipe.Title}' has been deleted successfully.", "green");
                }
                catch (Exception ex)
                {
                    p.print($"Failed to delete recipe '{recipe.Title}'. Error: {ex.Message}", "red");
                }
            }
            else
            {
                p.print("Deletion canceled.", "yellow");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void CreateRecipe()
        {
            p.print("Enter the title of the recipe:\n");
            string title = Console.ReadLine();

            p.print("Enter the link of the recipe (or leave blank):\n");
            string link = Console.ReadLine();

            GatherRecipeIngredients();
            GatherRecipeSteps();

            createrecipe.CreateRecipe(title, link);
        }

        private void GatherRecipeIngredients()
        {
            while (true)
            {
                p.print("Enter ingredient name (or 'done' to finish):");
                string name = Console.ReadLine();
                if (name.ToLower() == "done")
                    break;

                p.print("Enter ingredient amount:");
                string inputAmount = Console.ReadLine();
                if (!double.TryParse(inputAmount, out double amount))
                {
                    p.print("Invalid amount. Please enter a number.");
                    continue;
                }

                p.print("Enter ingredient unit (e.g., grams, cups):");
                string unit = Console.ReadLine();

                p.print("Enter any extra info (or leave blank):");
                string extra = Console.ReadLine();

                createrecipe.AddIngredient(name, amount, unit, extra);
            }
        }

        private void GatherRecipeSteps()
        {
            while (true)
            {
                p.print("Enter a recipe step (or 'done' to finish):");
                string step = Console.ReadLine();
                if (step.ToLower() == "done")
                    break;

                createrecipe.AddStep(step);
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void UpdateTitle(Recipe recipe)
        {
            p.print($"Current Title: {recipe.Title}");
            p.print("Enter new title (or leave blank to keep current):");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                recipe.Title = newTitle;
                p.print("Title updated successfully!", "green");
            }
            else
            {
                p.print("Title unchanged.", "yellow");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void UpdateLink(Recipe recipe)
        {
            p.print($"Current Link: {recipe.Link}");
            p.print("Enter new link (or leave blank to keep current):");
            string newLink = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newLink))
            {
                recipe.Link = newLink;
                p.print("Link updated successfully!", "green");
            }
            else
            {
                p.print("Link unchanged.", "yellow");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void AddIngredient(Recipe recipe)
        {
            p.print("Enter ingredient name:");
            string name = Console.ReadLine();

            p.print("Enter ingredient amount:");
            if (!double.TryParse(Console.ReadLine(), out double amount))
            {
                p.print("Invalid amount. Operation canceled.", "red");
                return;
            }

            p.print("Enter ingredient unit (e.g., grams, cups):");
            string unit = Console.ReadLine();

            p.print("Enter extra info (or leave blank):");
            string extra = Console.ReadLine();

            service.AddIngredient(recipe, amount, unit, name, extra);

            p.print("Ingredient added successfully!", "green");
        }

        public void EditIngredient(Recipe recipe)
        {
            p.print("Select an ingredient to edit (enter number):");
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                p.print($"\n{i + 1}. {recipe.Ingredients[i]}", "yellow");
            }

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= recipe.Ingredients.Count)
            {
                RecipeIngredient ingredient = recipe.Ingredients[index - 1];
                p.print($"Editing Ingredient: {ingredient}");

                p.print("Enter new name (or leave blank to keep current):");
                string newName = Console.ReadLine();

                p.print("Enter new amount (or leave blank to keep current):");
                string amountInput = Console.ReadLine();
                double? newAmount = double.TryParse(amountInput, out double parsedAmount) ? parsedAmount : (double?)null;

                p.print("Enter new unit (or leave blank to keep current):");
                string newUnit = Console.ReadLine();

                p.print("Enter new extra info (or leave blank to keep current):");
                string newExtra = Console.ReadLine();

                service.UpdateIngredient(ingredient, newName, newAmount, newUnit, newExtra);

                p.print("Ingredient updated successfully!", "green");
            }
            else
            {
                p.print("Invalid selection.", "red");
            }
        }

        public void RemoveIngredient(Recipe recipe)
        {
            p.print("Select an ingredient to remove (enter number):");
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                p.print($"\n{i + 1}. {recipe.Ingredients[i]}", "yellow");
            }

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= recipe.Ingredients.Count)
            {
                service.RemoveIngredient(recipe, index - 1);

                p.print("Ingredient removed successfully!", "green");
            }
            else
            {
                p.print("Invalid selection.", "red");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void EditStep(Recipe recipe)
        {
            p.print("Select a step to edit (enter number):");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                p.print($"\n{i + 1}. {recipe.Steps[i]}");
            }

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= recipe.Steps.Count)
            {
                p.print("Enter the new text for the step:");
                string newStep = Console.ReadLine();

                service.UpdateStep(recipe, index - 1, newStep);

                if (!string.IsNullOrWhiteSpace(newStep))
                {
                    p.print("Step updated successfully!", "green");
                }
                else
                {
                    p.print("Step unchanged.", "yellow");
                }
            }
            else
            {
                p.print("Invalid selection.", "red");
            }
        }

        public void RemoveStep(Recipe recipe)
        {
            p.print("Select a step to remove (enter number):");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                p.print($"\n{i + 1}. {recipe.Steps[i]}");
            }

            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= recipe.Steps.Count)
            {
                service.RemoveStep(recipe, index - 1);

                p.print("Step removed successfully!", "green");
            }
            else
            {
                p.print("Invalid selection.", "red");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void DeductIngredients(Recipe recipe)
        {
            foreach (var recipeIngredient in recipe.Ingredients)
            {
                p.print($"\nIngredient: {recipeIngredient.Name}");
                p.print($"\nRequired Amount: {recipeIngredient.Amount} {recipeIngredient.Unit}");
                p.print("\nEnter amount to use (leave blank to use the default amount, or enter 0 to skip):", "white");

                while (true)
                {
                    string input = Console.ReadLine()?.Trim();
                    double amountToSpend;

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        amountToSpend = recipeIngredient.Amount;
                    }
                    else
                    {
                        if (!double.TryParse(input, out amountToSpend) || amountToSpend < 0)
                        {
                            p.print("\nInvalid input. Please enter a non-negative number or leave blank to use the default amount.", "red");
                            continue;
                        }
                    }

                    var result = service.TryDeductIngredient(recipeIngredient, amountToSpend);

                    if (result.IsInvalidInput)
                    {
                        p.print("\nInvalid input. Please enter a valid amount.", "red");
                    }
                    else if (result.IsSkipped)
                    {
                        p.print($"\nSkipped '{recipeIngredient.Name}'.", "yellow");
                        break;
                    }
                    else if (result.IsInsufficient)
                    {
                        p.print($"\nInsufficient '{recipeIngredient.Name}' ({recipeIngredient.Unit}) in fridge.", "red");
                        p.print($"\nAvailable: {result.AvailableAmount} {recipeIngredient.Unit}", "yellow");
                        p.print("\nEnter a new amount or leave blank to use the default amount:", "white");
                    }
                    else if (result.IsSuccess)
                    {
                        p.print($"\nUsed {amountToSpend} {recipeIngredient.Unit} of '{recipeIngredient.Name}'.", "green");
                        break;
                    }
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void PrepareDish(Recipe recipe)
        {
            if (recipe == null)
            {
                p.print("\nNo recipe selected for preparation.", "red");
                return;
            }

            p.print($"\nPreparing Dish: {recipe.Title}", "cyan");

            foreach (var ingredient in recipe.Ingredients)
            {
                p.print($"\n- {ingredient.Name}: {ingredient.Amount} {ingredient.Unit} {ingredient.Extra}");
            }

            var missingIngredients = service.GetMissingIngredients(recipe);

            if (missingIngredients.Any())
            {
                p.print("\nYou are missing the following ingredients:", "yellow");
                foreach (var missing in missingIngredients)
                {
                    // Show how much is needed and how much is available
                    double available = 0;
                    var fridgeIngredients = fridge.GetIngredientsByName(missing.Name);
                    var match = fridgeIngredients.FirstOrDefault(i => i.Unit.Equals(missing.Unit, StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                        available = match.Amount;

                    p.print($"\n- {missing.Name} Needed: {missing.Amount}{missing.Unit}, Available: {available}{missing.Unit}", "red");
                }
            }
            else
            {
                p.print("\nAll required ingredients are available!", "green");
            }

            p.print("\nDo you want to proceed with making this dish? (Y/n):", "white");
            string confirmation = Console.ReadLine()?.Trim();

            if (confirmation?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true
                || confirmation?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true)
            {
                DeductIngredients(recipe);

                Display(recipe);
            }
            else
            {
                p.print("Dish preparation canceled.", "red");
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void PrintRecipes()
        {
            var recipes = service.GetAllRecipesAsStrings();
            foreach (var recipe in recipes)
            {
                p.print(recipe);
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public bool SearchMenu(List<Recipe> searchResults)
        {
            while (true)
            {
                p.print("\nOptions-Search Menu:", "cyan");
                p.print("\n1 - Search", "yellow");
                p.print("\n2 - Select a Recipe", "yellow");
                p.print("\n0 - Back");
                p.print("\nChoose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Search Again
                        return true;

                    case "2":
                        SelectFromList(searchResults);
                        break;

                    case "0":
                        return false;

                    default:
                        p.print("Invalid option. Please try again.", "white");
                        break;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void ManageSelectedRecipe(Recipe recipe)
        {
            while (true)
            {
                p.print($"\nManaging Recipe: {recipe.Title}", "cyan");
                p.print("\n1 - View Recipe", "yellow");
                p.print("\n2 - Update Recipe", "yellow");
                p.print("\n3 - Create Dish (Spend Ingredients)", "yellow");
                p.print("\n4 - Delete Recipe", "red");
                p.print("\n0 - Back");
                p.print("\nChoose an option: ");

                string actionChoice = Console.ReadLine();

                switch (actionChoice)
                {
                    case "1":
                        Display(recipe);
                        break;

                    case "2": // Update Recipe
                        UpdateRecipeMenu(recipe);
                        break;

                    case "3": // MakeDish, spend ingredients
                        PrepareDish(recipe);

                        break;


                    case "4": // Delete Recipe
                        DeleteRecipe(book, recipe);
                        break;

                    case "0":
                        return;

                    default:
                        p.print($"Invalid option. Please try again.", "white");
                        break;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void UpdateRecipeMenu(Recipe recipe)
        {
            if (recipe == null)
            {
                p.print("No recipe selected for updating.", "red");
                return;
            }

            while (true)
            {
                p.print($"\nUpdating Recipe: {recipe.Title}", "cyan");
                p.print("\n1 - Update Title", "yellow");
                p.print("\n2 - Update Link", "yellow");
                p.print("\n3 - Manage Ingredients (Add, Remove, Edit)", "yellow");
                p.print("\n4 - Manage Steps (Add, Remove, Edit)", "yellow");
                p.print("\n0 - Exit");
                p.print("\nChoose an option:");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Update Title
                        UpdateTitle(recipe);
                        break;

                    case "2": // Update Link
                        UpdateLink(recipe);
                        break;

                    case "3": // Manage Ingredients
                        ManageRecipeIngredients(recipe);
                        break;

                    case "4": // Manage Steps
                        ManageRecipeSteps(recipe);
                        break;

                    case "0": // exit
                        return;

                    default:
                        p.print("Invalid option. Please try again.", "red");
                        break;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void ManageRecipeIngredients(Recipe recipe)
        {
            while (true)
            {
                p.print("\nIngredients Management:", "yellow");
                p.print("\n1 - Add Ingredient", "yellow");
                p.print("\n2 - Edit Ingredient", "yellow");
                p.print("\n3 - Remove Ingredient", "yellow");
                p.print("\n0 - Back");
                p.print("\nChoose an option:");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Add Ingredient
                        AddIngredient(recipe);
                        break;

                    case "2": // Edit Ingredient
                        EditIngredient(recipe);
                        break;

                    case "3": // Remove Ingredient
                        RemoveIngredient(recipe);
                        break;

                    case "0": // Back
                        return;

                    default:
                        p.print("Invalid option. Please try again.", "red");
                        break;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

        public void ManageRecipeSteps(Recipe recipe)
        {
            while (true)
            {
                p.print("\nSteps Management:", "yellow");
                p.print("\n1 - Add Step", "yellow");
                p.print("\n2 - Edit Step", "yellow");
                p.print("\n3 - Remove Step", "yellow");
                p.print("\n0 - Back");
                p.print("\nChoose an option:");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Add Step
                        p.print("Enter the new step:");
                        string step = Console.ReadLine();
                        recipe.Steps.Add(step);
                        p.print("Step added successfully!", "green");
                        break;

                    case "2": // Edit Step
                        EditStep(recipe);
                        break;

                    case "3": // Remove Step
                        RemoveStep(recipe);
                        break;

                    case "0": // Back
                        return;

                    default:
                        p.print("Invalid option. Please try again.", "red");
                        break;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------ //

    }
}
