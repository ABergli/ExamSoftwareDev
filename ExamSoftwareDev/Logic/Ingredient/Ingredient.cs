namespace ExamSoftwareDev.Logic.Ingredient {
    public enum IngredientCategory {
        Other,
        Produce,
        Dairy,
        Meat,
        Liquids,
        Bakery,
        Spices
    }
    public class Ingredient {
        public int Id { get; set; }
        public string Name { get; set; }
        public IngredientCategory Category; // e.g. "Produce", "Dairy", "Meat", "Liquids", "Bakery", "Other"

        public double Amount { get; set; } // e.g. 100, 250, 1.5
        public string Unit { get; set; } // e.g. "g", "ml", "tbsp"


        public Ingredient() { }// Parameterless constructor required by EF
        public Ingredient(int id, string n, string c, double a, string u) {
            Id = id;
            Name = n;
            if (Enum.TryParse<IngredientCategory>(c, out var category)) {
                Category = category;

            } else {
                Category = IngredientCategory.Other;
            }
            //this.Category = c;
            Amount = a;
            Unit = u;
        }

        public override string ToString() {
            return $"Ingredient: {Id}/ {Name}/ {Category}/ {Amount}/ {Unit}\n";
        }
    }
}
