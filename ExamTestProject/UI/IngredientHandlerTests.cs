
using ExamSoftwareDesign.UI.Ingredients;
using ExamSoftwareDesign.UI;
using ExamSoftwareDesign.Logic.IngredientLogic;
using NUnit.Framework;


namespace ExamTestProject.UI {
    public class IngredientHandlerTests {
        private StringWriter _stringWriter;
        private StringReader _stringReader;
        private Refrigerator _fridge;
        private ExPrint _exPrint;
        private IngredientHandler _ingredientHandler;

        [SetUp]
        public void SetUp() {
            _exPrint = new ExPrint();
            _fridge = new Refrigerator();

            // Redirect Console output and input for testing
            _stringWriter = new StringWriter();
            _stringReader = new StringReader("Test input");
            Console.SetOut(_stringWriter);
            Console.SetIn(_stringReader);

            // Initialize IngredientHandler
            _ingredientHandler = new IngredientHandler(_fridge, _exPrint);
        }

        [Test]
        public void GetInput_ReturnsCorrectInput() {
            // Act
            string result = _ingredientHandler.GetInput("Please enter something: ");

            // Assert
            Assert.That(result, Is.EqualTo("Test input"));
            Assert.That(_stringWriter.ToString(), Does.Contain("Please enter something: "));
        }

        [TearDown]
        public void TearDown() {
            // Reset Console input and output streams after each test
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));

            // Dispose the StringWriter and StringReader
            _stringWriter.Dispose();
            _stringReader.Dispose();
        }
    }
}
