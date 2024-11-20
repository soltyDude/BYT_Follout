namespace Fallout.Tests
{
    [TestFixture]
    public class ShelterTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Shelter)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Shelter>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            int number = 1;
            int nuKaColaKaps = 100;
            int nuKaCola = 50;

            // Act
            var shelter = new Shelter(number, nuKaColaKaps, nuKaCola);

            // Assert
            Assert.AreEqual(number, shelter.Number);
            Assert.AreEqual(nuKaColaKaps, shelter.NuKaColaKaps);
            Assert.AreEqual(nuKaCola, shelter.NuKaCola);
            Assert.AreEqual(DateTime.Now.Date, shelter.CreationDate.Date);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var shelter = new Shelter(1, 100, 50);

            // Act
            var extent = Shelter.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(shelter), "Extent should contain the created Shelter.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void Number_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Shelter(-1, 100, 50));
            Assert.AreEqual("Number cannot be negative. (Parameter 'number')", ex.Message);
        }

        [Test]
        public void NuKaColaKaps_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Shelter(1, -100, 50));
            Assert.AreEqual("NuKaColaKaps cannot be negative. (Parameter 'nuKaColaKaps')", ex.Message);
        }

        [Test]
        public void NuKaCola_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Shelter(1, 100, -50));
            Assert.AreEqual("NuKaCola cannot be negative. (Parameter 'nuKaCola')", ex.Message);
        }

        [Test]
        public void AddToExtent_NullShelter_ThrowsArgumentException()
        {
            // Arrange
            Shelter shelter = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(Shelter)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { shelter });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("Shelter cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = Shelter.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<Shelter>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_shelter_extent.xml";
            var shelter = new Shelter(1, 100, 50);
            Shelter.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_shelter_extent.xml";
            var shelter = new Shelter(1, 100, 50);
            Shelter.SaveExtent(filePath);
            
            typeof(Shelter)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Shelter>());

            // Act
            Shelter.LoadExtent(filePath);
            var extent = Shelter.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(1, extent[0].Number);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var shelter = new Shelter(1, 100, 50);

            // Act
            Shelter.LoadExtent(filePath);
            var extent = Shelter.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_shelter_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var shelter = new Shelter(1, 100, 50);

            // Act
            Shelter.LoadExtent(filePath);
            var extent = Shelter.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
