using ExamSoftwareDev.Storage;


namespace ExamSoftwareDev.Logic.Ingredient {
    public class IngredientService {

        private readonly IngredientRepository _repository;

        public IngredientService(IngredientRepository repository) {
            _repository = repository;
        }

        // Add a new ingredient
        public void AddNewIngredient(string name, string category, double amount, string unit) {

            // Check if the ingredient already exists in the repository
            // Checking currently only Name and Category is the same.
            var existingIngredient = _repository.GetAllIngredients()
                                                 .FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                                                  && i.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase));

            // If the ingredient already exists, don't add it
            if (existingIngredient != null) {
                Console.WriteLine($"Ingredient '{name}' in category '{category}' already exists.");
                return; // Exit the method early to prevent duplication
            }

            // Try to parse the category string into the IngredientCategory enum
            if (!Enum.TryParse<IngredientCategory>(category, true, out var parsedCategory)) {
                parsedCategory = IngredientCategory.Other; // Default to "Other" if parsing fails
            }

            // Create the Ingredient object
            var ingredient = new Ingredient {
                Name = name,
                Category = parsedCategory,
                Amount = amount,
                Unit = unit
            };

            // Add the ingredient to the repository
            _repository.AddIngredient(ingredient);
        }

        // Retrieve all ingredients
        public IEnumerable<Ingredient> GetAllIngredients() {
            return _repository.GetAllIngredients();
        }

        // Search for ingredients by name
        public IEnumerable<Ingredient> GetIngredientsByName(string name) {
            // Rely on the repository to handle case-insensitive "contains" matching
            return _repository.GetIngredientsByName(name);
        }


        // Search for ingredients by category
        public IEnumerable<Ingredient> GetIngredientsByCategory(IngredientCategory category) {
            return _repository.GetAllIngredients()
                              .Where(i => i.Category == category);
        }

        // Update an existing ingredient
        public bool UpdateIngredient(int id, string name, string category, double amount, string unit) {
            var ingredient = _repository.GetAllIngredients()
                                        .FirstOrDefault(i => i.Id == id);

            if (ingredient == null) {// Ingredient not found
                return false; 
            }

            if (!string.IsNullOrEmpty(name)) ingredient.Name = name;
            if (!string.IsNullOrEmpty(category) && Enum.TryParse<IngredientCategory>(category, true, out var parsedCategory)) {
                ingredient.Category = parsedCategory;
            }
            ingredient.Amount = amount > 0 ? amount : ingredient.Amount;
            if (!string.IsNullOrEmpty(unit)) ingredient.Unit = unit;

            _repository.UpdateIngredient(ingredient);
            return true;
        }

        // Delete an ingredient by ID
        public bool DeleteIngredient(int id) {
            var ingredient = _repository.GetAllIngredients()
                                        .FirstOrDefault(i => i.Id == id);

            if (ingredient == null) {
                return false; // Ingredient not found
            }

            _repository.DeleteIngredient(ingredient);
            return true;
        }

        // Delete ingredients by name
        public int DeleteIngredientsByName(string name) {
            var ingredients = _repository.GetAllIngredients()
                                          .Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                          .ToList();

            foreach (var ingredient in ingredients) {
                _repository.DeleteIngredient(ingredient);
            }

            return ingredients.Count; // Return number of deleted ingredients
        }
    }
}
