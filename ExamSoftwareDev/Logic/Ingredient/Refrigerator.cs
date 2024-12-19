using ExamSoftwareDesign.Storage;
using ExamSoftwareDesign.UI;

namespace ExamSoftwareDesign.Logic.IngredientLogic {
    public class Refrigerator : IRefrigerator {
        private ExPrint p = new();
        public string Owner { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        private IngredientService _ingredientService;

        public Refrigerator() {
            // Parameterless constructor required for mocking/Unit Testing
        }
        public Refrigerator(string users, IngredientService ingredientService) {
            Owner = users;
            _ingredientService = ingredientService;
            Ingredients = _ingredientService.GetAllIngredients().ToList(); // Load ingredients from service/ database
            //PrintIngredients(); // Display ingredients
            p.print(ToString(), "red");
        }

        // Add an ingredient using the service
        public void AddIngredient(string name, string category, double amount, string unit) {
            _ingredientService.AddNewIngredient(name, category, amount, unit); // Delegate to service for adding
            Ingredients = _ingredientService.GetAllIngredients().ToList(); // Refresh ingredient list from service
        }

        // Remove an ingredient using the service
        public void RemoveIngredient(int id) {
            _ingredientService.DeleteIngredient(id); // Delegate to service for removal
            Ingredients = _ingredientService.GetAllIngredients().ToList(); // Refresh ingredient list from service
        }

        // Update an ingredient using the service
        public bool UpdateIngredient(int id, string name, string category, double amount, string unit) {
            bool updated = _ingredientService.UpdateIngredient(id, name, category, amount, unit); // Delegate to service for update
            if (updated) {
                Ingredients = _ingredientService.GetAllIngredients().ToList(); // Refresh ingredient list from service
            }
            return updated;
        }

        // Get ingredients by name from the service
        public IEnumerable<Ingredient> GetIngredientsByName(string name) {
            return _ingredientService.GetIngredientsByName(name);
        }

        // Get ingredients by category from the service
        public IEnumerable<Ingredient> GetIngredientsByCategory(IngredientCategory category) {
            return _ingredientService.GetIngredientsByCategory(category);
        }

        // Print all ingredients
        public void PrintIngredients() {
            foreach (var ingredient in Ingredients) {
                p.print(ingredient.ToString());
            }
        }

        public override string ToString() => $"Refrigerator: {Owner}/ Ingredients: {Ingredients.Count}";
    }
}
