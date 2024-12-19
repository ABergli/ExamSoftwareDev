

namespace ExamSoftwareDev.Logic.Ingredient {
    public interface IRefrigerator {
        void AddIngredient(string name, string category, double amount, string unit);
        void RemoveIngredient(int id);
        bool UpdateIngredient(int id, string name, string category, double amount, string unit);
        IEnumerable<Ingredient> GetIngredientsByName(string name);
        IEnumerable<Ingredient> GetIngredientsByCategory(IngredientCategory category);
        void PrintIngredients();
    }
}
