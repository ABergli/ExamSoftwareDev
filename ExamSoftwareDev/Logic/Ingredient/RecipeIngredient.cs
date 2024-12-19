

namespace ExamSoftwareDev.Logic.Ingredient {
    public class RecipeIngredient : Ingredient {
        public string Extra { get; private set; }

        public RecipeIngredient(double amount, string unit, string name, string extra = null)
            : base(0, name, "Other", amount, unit)
        {
            Extra = extra;
        }
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

