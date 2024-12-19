using ExamSoftwareDev.Logic.Ingredient;


namespace ExamSoftwareDev.UI.Ingredients {
    public class IngredientHandler {
        private readonly Refrigerator fridge;
        private readonly ExPrint p;

        public IngredientHandler(Refrigerator fridge, ExPrint p) {
            this.fridge = fridge;
            this.p = p;
        }

        public void AddIngredient() {
            p.print("Add Ingredient\n\n");
            try {
                string name = GetInput("Enter ingredient name:");
                string category = GetInput("Enter category (e.g., Produce, Dairy, Meat, etc.):");
                p.print("Enter amount (double):");
                if (!double.TryParse(Console.ReadLine(), out double amount)) {
                    p.print("Invalid input for amount. Please try again.", "red");
                    return;
                }
                string unit = GetInput("Enter unit (e.g., gram, tbs, l, ml):");

                fridge.AddIngredient(name, category, amount, unit);
                p.print("Ingredient added successfully!", "green");
            } catch (Exception ex) {
                p.print($"An error occurred while adding the ingredient: {ex.Message}", "red");
            }
        }

        public void LookupIngredient() {
            p.print("Look-up Ingredient\n\n");
            try {
                string lookupName = GetInput("Enter the ingredient name to search:");
                var ingredients = fridge.GetIngredientsByName(lookupName).ToList();

                if (ingredients.Any()) {
                    p.print($"Found {ingredients.Count} matching ingredient(s):\n", "green");
                    foreach (var ingredient in ingredients) {
                        p.print(ingredient.ToString());
                    }
                } else {
                    p.print("No matching ingredient found.", "yellow");
                }
            } catch (Exception ex) {
                p.print($"An error occurred while searching for the ingredient: {ex.Message}", "red");
            }
        }

        public void DeleteIngredient() {
            p.print("Delete Ingredient\n\n");
            try {
                string deleteName = GetInput("Enter the name of the ingredient to delete:");

                var matchingIngredients = fridge.GetIngredientsByName(deleteName).ToList();

                if (matchingIngredients.Count == 0) {
                    p.print("No ingredient found to delete.\n", "yellow");
                    return;
                }
                if (matchingIngredients.Count == 1) {
                    fridge.RemoveIngredient(matchingIngredients.First().Id);
                    p.print("Ingredient deleted successfully!", "green");
                } else {
                    p.print($"Multiple ingredients found with the name '{deleteName}'. Please refine your search or use IDs:\n", "yellow");
                    foreach (var ingredient in matchingIngredients) {
                        p.print(ingredient.ToString());
                    }

                    p.print("Enter the ID of the ingredient you want to delete:");
                    if (int.TryParse(Console.ReadLine(), out var id)) {
                        fridge.RemoveIngredient(id);
                        p.print("Ingredient deleted successfully!", "green");
                    } else {
                        p.print("Invalid ID.", "red");
                    }
                }
            } catch (Exception ex) {
                p.print($"An error occurred while deleting the ingredient: {ex.Message}", "red");
            }
        }

        public void ViewAllIngredients() {
            p.print("View All Ingredients\n\n");
            try {
                fridge.PrintIngredients();
            } catch (Exception ex) {
                p.print($"An error occurred while retrieving the ingredients: {ex.Message}", "red");
            }
        }

        public void UpdateIngredient() {
            p.print("Update Ingredient\n\n");
            try {
                int id = int.Parse(GetInput("Enter the ID of the ingredient to update:"));
                var ingredient = fridge.Ingredients.FirstOrDefault(i => i.Id == id);

                if (ingredient != null) {
                    p.print($"Updating ingredient: {ingredient.Name}");

                    string name = GetInput($"Enter new name (or press Enter to keep '{ingredient.Name}'): ");
                    if (string.IsNullOrEmpty(name)) name = ingredient.Name;

                    string category = GetInput($"Enter new category (or press Enter to keep '{ingredient.Category}'): ");
                    if (string.IsNullOrEmpty(category)) category = ingredient.Category.ToString();

                    string amountInput = GetInput($"Enter new amount (or press Enter to keep {ingredient.Amount}): ");

                    double amount = ingredient.Amount;
                    if (double.TryParse(amountInput, out double newAmount) && newAmount > 0) amount = newAmount;

                    string unit = GetInput($"Enter new unit (or press Enter to keep '{ingredient.Unit}'): ");
                    if (string.IsNullOrEmpty(unit)) unit = ingredient.Unit;

                    bool updated = fridge.UpdateIngredient(id, name, category, amount, unit);
                    if (updated) p.print("Ingredient updated successfully!", "green");
                    else p.print("Failed to update ingredient. It might not exist.", "yellow");
                } else {
                    p.print("Ingredient not found.", "yellow");
                }
            } catch (Exception ex) {
                p.print($"An error occurred: {ex.Message}", "red");
            }
        }

        public string GetInput(string prompt) {
            p.print(prompt);
            return Console.ReadLine();
        }
    }
}

