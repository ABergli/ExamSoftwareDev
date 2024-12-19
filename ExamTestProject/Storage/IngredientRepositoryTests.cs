using ExamSoftwareDev.Logic.Ingredient;
using ExamSoftwareDev.Storage;
using Microsoft.EntityFrameworkCore;


// comprehensive testing of the IngredientRepository class
// and storage operations for the Ingredient entity
// ! the initial database is seeded with 20 ingredients from the IngredientContext
namespace ExamTestProject.Storage.Tests {
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class IngredientRepositoryTests {
        private IngredientContext _contextTest;
        private IngredientRepository _ingredientRepository;


        [SetUp]
        public void SetUp() {
            // Create a new in-memory SQLite database for each test
            var options = new DbContextOptionsBuilder<IngredientContext>()
                          .UseSqlite("Data Source=:memory:") // Using in-memory SQLite database for isolation
                          .Options;

            // Create a new context instance
            _contextTest = new IngredientContext(options);

            // Ensure the database schema is created (delete if it exists)
            Console.WriteLine("Creating in-memory database...");
            _contextTest.Database.OpenConnection(); // Ensure the connection is opened and remains open
            _contextTest.Database.EnsureDeleted();  // Clean up any existing state
            _contextTest.Database.EnsureCreated();  // Create fresh schema

            var ingredientCount = _contextTest.Ingredients.Count();
            Console.WriteLine($"Number of ingredients to test on: {ingredientCount}");

            // Initialize IngredientRepository here
            _ingredientRepository = new IngredientRepository(_contextTest);
        }



        [TearDown]
        public void TearDown() {
            // Log the number of ingredients in the database (before cleanup)
            var allIngredients = _contextTest.Ingredients.ToList();
            Console.WriteLine($"Number of ingredients in database before cleanup: {allIngredients.Count}");

            // Only remove the ingredients if there are any
            if (allIngredients.Any()) {
                _contextTest.Ingredients.RemoveRange(allIngredients);
                _contextTest.SaveChanges();
            }

            // Ensure all resources are disposed after database operations
            _contextTest.Dispose();

            // Add a slight delay to allow the file to be released
            System.Threading.Thread.Sleep(500);
        }



        [Test]
        public void AddIngredient_ShouldAddIngredientToDatabase() {
            // Arrange
            var ingredient = new Ingredient {
                Name = "Cucumber",
                Category = IngredientCategory.Produce,
                Amount = 1.0,
                Unit = "kg"
            };

            // Act
            _ingredientRepository.AddIngredient(ingredient);

            // Assert
            var addedIngredient = _contextTest.Ingredients.FirstOrDefault(i => i.Name == "Cucumber");
            Assert.That(addedIngredient, Is.Not.Null);
            Assert.That(addedIngredient.Name, Is.EqualTo("Cucumber"));
        }

        [Test]
        public void GetAllIngredients_ShouldReturnAllIngredients() {
            // Check the count before retrieving ingredients
            var preFetchCount = _contextTest.Ingredients.Count();
            Console.WriteLine($"Number of ingredients before fetching: {preFetchCount}");

            // Act
            var result = _ingredientRepository.GetAllIngredients();

            // Check the count after fetching ingredients
            var postFetchCount = result.Count();
            Console.WriteLine($"Number of ingredients after fetching: {postFetchCount}");

            // Assert
            Assert.That(postFetchCount, Is.EqualTo(20));
        }


        [Test]
        public void GetIngredientById_ShouldReturnCorrectIngredient() {
            // Act
            var result = _ingredientRepository.GetIngredientById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("New potatoes"));
        }

        [Test]
        public void GetIngredientsByName_ShouldReturnMatchingIngredients() {
            // Act
            var result = _ingredientRepository.GetIngredientsByName("Oil").ToList();

            // Debug output
            Console.WriteLine($"Number of ingredients matching 'Oil': {result.Count}");
            foreach (var ingredient in result) {
                Console.WriteLine($"Found ingredient: {ingredient.Name}");
            }

            // Assert
            Assert.That(result.Count, Is.EqualTo(2), "Expected 2 ingredients matching 'Oil' (case-insensitive).");
            Assert.That(result.Any(i => i.Name == "Oil"), "Expected 'Oil' in the results.");
            Assert.That(result.Any(i => i.Name == "Rapeseed oil"), "Expected 'Rapeseed oil' in the results.");
        }



        [Test]
        public void GetIngredientsByCategory_ShouldReturnIngredientsByCategory() {
            // Act
            var result = _ingredientRepository.GetIngredientsByCategory(IngredientCategory.Produce);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(5));
            //"New potatoes", "Leeks", "Red onions", "Kale", "Spinach"

            // Additional check to ensure category comparison is case-insensitive
            foreach (var ingredient in result) {
                Assert.That(ingredient.Category.ToString().ToLower(), Is.EqualTo(IngredientCategory.Produce.ToString().ToLower()));
            }
        }


        [Test]
        public void UpdateIngredient_ShouldUpdateIngredientInDatabase() {
            // Arrange
            var ingredient = _contextTest.Ingredients.First();
            ingredient.Name = "Updated New potatoes";

            // Act
            _ingredientRepository.UpdateIngredient(ingredient);

            // Assert
            var updatedIngredient = _contextTest.Ingredients.Find(ingredient.Id);
            Assert.That(updatedIngredient.Name, Is.EqualTo("Updated New potatoes"));
        }

        [Test]
        public void DeleteIngredient_ShouldRemoveIngredientFromDatabase() {
            // Arrange
            var ingredient = _contextTest.Ingredients.First();

            // Act
            _ingredientRepository.DeleteIngredient(ingredient);

            // Assert
            var deletedIngredient = _contextTest.Ingredients.Find(ingredient.Id);
            Assert.That(deletedIngredient, Is.Null); // Ingredient should be deleted
        }

        [Test]
        public void DeleteIngredientsByName_ShouldRemoveIngredientsByName() {
            // Arrange
            var ingredientName = "Oil";

            // Act
            _ingredientRepository.DeleteIngredientsByName(ingredientName);

            // Assert
            var deletedIngredients = _contextTest.Ingredients.Where(i => i.Name.ToLower() == ingredientName.ToLower()).ToList();
            Assert.That(deletedIngredients.Count, Is.EqualTo(0)); // No "Oil" ingredients should remain
        }
    }
}
