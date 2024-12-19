using ExamSoftwareDesign.Logic.IngredientLogic;
using ExamSoftwareDesign.Logic.RecipeServices;
using ExamSoftwareDesign.UI;
using ExamSoftwareDesign.Storage;
using Microsoft.EntityFrameworkCore;
using ExamSoftwareDesign.UI.Ingredients;
using NUnit.Framework;



namespace ExamTestProject.UI.Tests {
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class UserInteractionTests {
        private IngredientContext _contextTest;
        private IngredientRepository _ingredientRepository;
        private IngredientService _ingredientService;
        private Refrigerator _fridge;
        private UserInteractions _userInteractions;
        private IngredientHandler _ingredientHandler;


        [SetUp]
        public void SetUp() {
            var options = new DbContextOptionsBuilder<IngredientContext>()
                          .UseSqlite("Data Source=:memory:")
                          .Options;

            _contextTest = new IngredientContext(options);
            _contextTest.Database.OpenConnection();
            _contextTest.Database.EnsureDeleted();
            _contextTest.Database.EnsureCreated();

            _ingredientRepository = new IngredientRepository(_contextTest);
            _ingredientService = new IngredientService(_ingredientRepository);
            _fridge = new Refrigerator("User1", _ingredientService);
            _userInteractions = new UserInteractions(_fridge, new Cookbook());
            _ingredientHandler = new IngredientHandler(_fridge, new ExPrint());

            var ingredientCount = _contextTest.Ingredients.Count();
            Console.WriteLine($"\n\nNumber of ingredients to test on: {ingredientCount}");
        }

        [TearDown]
        public void TearDown() {
            // dispose resources
            _contextTest.Dispose();
        }

        // Test AddIngredient
        //[Test]
        //public void AddIngredient_ShouldAddIngredientSuccessfully() {
        //    // Arrange
        //    string ingredientName = "Tomato";
        //    string category = "Produce";
        //    double amount = 2;
        //    string unit = "kg";

        //    // Act
        //    _ingredientHandler.AddIngredient();

        //    // Assert
        //    var addedIngredient = _fridge.GetIngredientsByName(ingredientName).FirstOrDefault();
        //    Assert.That(addedIngredient, Is.Not.Null);
        //    Assert.That(addedIngredient.Name, Is.EqualTo(ingredientName));
        //    Assert.That(addedIngredient.Category, Is.EqualTo(category));
        //    Assert.That(addedIngredient.Amount, Is.EqualTo(amount));
        //    Assert.That(addedIngredient.Unit, Is.EqualTo(unit));
        //}





        [Test]
        public void Run_ShouldExit_WhenUserSelectsExitOption() {
            // Arrange
            var userInput = "0";  // Simulate the user input for exiting the menu
            var stringWriter = new System.IO.StringWriter();
            Console.SetOut(stringWriter);
            Console.SetIn(new System.IO.StringReader(userInput));

            var cancellationTokenSource = new System.Threading.CancellationTokenSource();
            var timeoutTask = System.Threading.Tasks.Task.Delay(5000, cancellationTokenSource.Token);

            var task = System.Threading.Tasks.Task.Run(() => _userInteractions.Run());

            // Act
            if (!task.Wait(1000)) {  // Timeout after 5 seconds
                cancellationTokenSource.Cancel();
                Assert.Fail("Test timed out before Run() completed.");
            }

            // Assert
            Assert.That(stringWriter.ToString(), Does.Contain("Exit"));
        }



    }
}
