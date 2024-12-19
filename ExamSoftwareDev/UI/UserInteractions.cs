using ExamSoftwareDesign.Logic.IngredientLogic;
using ExamSoftwareDesign.Logic.RecipeLogic;
using ExamSoftwareDesign.Logic.RecipeServices;
using ExamSoftwareDesign.Storage;
using ExamSoftwareDesign.UI;
using ExamSoftwareDesign.UI.Ingredients;
using ExamSoftwareDesign.UI.RecipeUI;
using System;


namespace ExamSoftwareDesign.UI {
    public class UserInteractions {
        ExPrint p = new();
        string choice = "";

        private IngredientHandler ingredientHandler;
        private CreateRecipeService createRecipeService;
        private RecipeService recipeService;
        private RecipeUIHandler recipeUIHandler;
        private Refrigerator fridge;
        Cookbook book;

        public UserInteractions(Refrigerator fridge, Cookbook book)
        {
            this.fridge = fridge;
            this.book = book;

            ingredientHandler = new IngredientHandler(fridge, p);

            createRecipeService = new CreateRecipeService(book);
            recipeService = new RecipeService(book, fridge);

            // temporary to avoid null references
            recipeUIHandler = new RecipeUIHandler(recipeService, p, createRecipeService, fridge, book);
        }

        public void Run()
        {
            while (true)
            {
                p.print("\nMenu:\n");
                p.print("11 - Ingredient: Add New\n", "green");
                p.print("12 - Ingredient: Look-up\n", "green");
                p.print("13 - Ingredient: Delete\n", "green");
                p.print("14 - Ingredient: View all\n", "green");
                p.print("15 - Ingredient: Update\n", "green");
                p.print("21 - Recipe: Add New\n", "yellow");
                p.print("22 - Recipe: Search\n", "yellow");
                p.print("23 - Recipe: View all\n", "yellow");
                p.print(" 0 - Exit\n", "red");
                p.print("Choose an option: ");

#pragma warning disable CS8601 // Possible null reference assignment.
                choice = Console.ReadLine();
#pragma warning restore CS8601

                switch (choice)
                {
                    case "11": // add ingredient
                        ingredientHandler.AddIngredient();
                        break;

                    case "12": // look up ingredient
                        ingredientHandler.LookupIngredient();
                        break;

                    case "13": // delete ingredient
                        ingredientHandler.DeleteIngredient();
                        break;

                    case "14": //view all ingredients
                        ingredientHandler.ViewAllIngredients();
                        break;

                    case "15": // update ingredient
                        ingredientHandler.UpdateIngredient();
                        break;

                    case "21": // Add Recipe
                        recipeUIHandler.CreateRecipe();
                        break;

                    case "22": // Search Recipes
                        recipeUIHandler.Search();
                        break;

                    case "23": // View All Recipes
                        recipeUIHandler.ViewAllRecipes();
                        break;

                    case "0":
                        return;

                    default:
                        p.print($"Invalid option. Please try again.", "white");
                        break;
                }
            }
        }//run

    }
}
