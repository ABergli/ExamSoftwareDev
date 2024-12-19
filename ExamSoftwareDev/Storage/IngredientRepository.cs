using ExamSoftwareDev.Logic.Ingredient;
using Microsoft.EntityFrameworkCore;


namespace ExamSoftwareDev.Storage {
    public class IngredientRepository {

        private readonly IngredientContext _context;

        public IngredientRepository(IngredientContext context) {
            _context = context;
        }

        // Add a new ingredient to the database
        public void AddIngredient(Ingredient ingredient) {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
        }

        // Retrieve all ingredients from the database
        public IEnumerable<Ingredient> GetAllIngredients() {
            return _context.Ingredients.ToList();
        }

        // Retrieve a single ingredient by ID
        public Ingredient GetIngredientById(int id) {
            return _context.Ingredients.FirstOrDefault(i => i.Id == id);
        }

        // Retrieve ingredients by name
        public IEnumerable<Ingredient> GetIngredientsByName(string name) {
            // Case-insensitive "contains" match using EF.Functions.Like and wildcards
            return _context.Ingredients
                .Where(i => EF.Functions.Like(i.Name, $"%{name}%"));
        }



        // Retrieve ingredients by category
        public IEnumerable<Ingredient> GetIngredientsByCategory(IngredientCategory category) {
            return _context.Ingredients.Where(i => i.Category == category).ToList();
        }

        // Update an existing ingredient
        public void UpdateIngredient(Ingredient ingredient) {
            var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (existingIngredient != null)
            {
                existingIngredient.Name = ingredient.Name;
                existingIngredient.Category = ingredient.Category;
                existingIngredient.Amount = ingredient.Amount;
                existingIngredient.Unit = ingredient.Unit;

                _context.SaveChanges();
            }
        }

        // Delete an ingredient
        public void DeleteIngredient(Ingredient ingredient) {
            // Ensure the ingredient is tracked by the context
            if (_context.Entry(ingredient).State == EntityState.Detached) {
                _context.Ingredients.Attach(ingredient); // Attach the ingredient if it's not being tracked
            }

            // Remove the ingredient from the database
            _context.Ingredients.Remove(ingredient);
            _context.SaveChanges();
        }

        // Delete ingredients by name

        public void DeleteIngredientsByName(string name) {
            var ingredientsToDelete = _context.Ingredients
                .Where(i => i.Name.ToLower() == name.ToLower())  // Case-insensitive comparison
                .ToList();

            _context.Ingredients.RemoveRange(ingredientsToDelete);  // Remove matching ingredients
            _context.SaveChanges();  // Commit the changes to the database
        }


    }
}
