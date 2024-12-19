
namespace ExamSoftwareDev.Logic.Recipe {
    public interface ICookbook {

        ICookbook AddRecipe(Recipe recipe);
        ICookbook RemoveRecipe(Recipe recipe);
        ICookbook Clear();
        List<string> GetRecipesAsStrings();

    }
}
