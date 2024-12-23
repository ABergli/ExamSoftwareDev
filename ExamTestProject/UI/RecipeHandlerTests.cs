using ExamSoftwareDev.Logic.Ingredient;
using ExamSoftwareDev.Logic.Recipe;
using ExamSoftwareDev.UI.Ingredients;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExamTestProject.UI
{
    public class RecipeHandlerTests
    {
        private StringWriter _stringWriter;
        private StringReader _stringReader;
        private Refrigerator _fridge;
        private Recipe _recipe;

        [SetUp]
        public void SetUp()
        {
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);

            // Mock Refrigerator
            _fridge = new Refrigerator();
            _fridge.Ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Ingredient1", Unit = "grams", Amount = 100, Category = IngredientCategory.Dairy },
                new Ingredient { Id = 2, Name = "Ingredient2", Unit = "cups", Amount = 50, Category = IngredientCategory.Spices }
            };

            // Mock Recipe
            _recipe = new Recipe
            {
                Title = "Recipe1",
                Ingredients = new List<RecipeIngredient>
                {
                    new RecipeIngredient(50, "grams", "Ingredient1", ""),
                    new RecipeIngredient(25, "cups", "Ingredient2", "")
                },
                Steps = new List<string> { "Step1", "Step2" }
            };
        }

        [Test]
        public void DeductIngredients_Test()
        {
            _stringReader = new StringReader("50\n25\n");
            Console.SetIn(_stringReader);

            // Act
            foreach (var recipeIngredient in _recipe.Ingredients)
            {
                Console.WriteLine($"\nIngredient: {recipeIngredient.Name}");
                Console.WriteLine($"\nRequired Amount: {recipeIngredient.Amount} {recipeIngredient.Unit}");
                Console.WriteLine("\nEnter amount to use (leave blank to use the default amount, or enter 0 to skip):");

                string input = Console.ReadLine()?.Trim();
                double amountToSpend = string.IsNullOrWhiteSpace(input) ? recipeIngredient.Amount : double.Parse(input);

                var fridgeIngredient = _fridge.Ingredients.Find(i => i.Name == recipeIngredient.Name);
                if (fridgeIngredient != null && fridgeIngredient.Amount >= amountToSpend)
                {
                    fridgeIngredient.Amount -= amountToSpend;
                    Console.WriteLine($"\nUsed {amountToSpend} {recipeIngredient.Unit} of '{recipeIngredient.Name}'.");
                }
                else
                {
                    Console.WriteLine($"\nInsufficient '{recipeIngredient.Name}' ({recipeIngredient.Unit}) in fridge.");
                }
            }

            // Assert Console output
            string output = _stringWriter.ToString();
            Assert.That(output, Does.Contain("Used 50 grams of 'Ingredient1'"));
            Assert.That(output, Does.Contain("Used 25 cups of 'Ingredient2'"));

            // Assert Refrigerator state
            Assert.That(_fridge.Ingredients[0].Amount, Is.EqualTo(50));
            Assert.That(_fridge.Ingredients[1].Amount, Is.EqualTo(25));
        }

        [TearDown]
        public void TearDown()
        {
            // Reset Console input and output streams after each test
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));

            // Dispose the StringWriter and StringReader
            _stringWriter.Dispose();
            _stringReader.Dispose();
        }
    }
}
