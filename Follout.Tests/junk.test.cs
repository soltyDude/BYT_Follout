namespace Fallout.Tests
{
    [TestFixture]
    public class JunkTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Junk)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Junk>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            string name = "Metal Scrap";
            string rareness = "Common";
            int value = 50;

            // Act
            var junk = new Junk(name, rareness, value);

            // Assert
            Assert.AreEqual(name, junk.Name);
            Assert.AreEqual(rareness, junk.Rareness);
            Assert.AreEqual(value, junk.Value);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var junk = new Junk("Metal Scrap", "Common", 50);

            // Act
            var extent = Junk.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(junk), "Extent should contain the created Junk.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void Name_SetEmptyOrWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var junk = new Junk();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => junk.Name = " ");
            Assert.Throws<ArgumentException>(() => junk.Name = "\t");
            Assert.Throws<ArgumentException>(() => junk.Name = "");
        }

        [Test]
        public void Name_SetValidValue_SetsName()
        {
            // Arrange
            var junk = new Junk();
            string name = "Wooden Plank";

            // Act
            junk.Name = name;

            // Assert
            Assert.AreEqual(name, junk.Name);
        }

        [Test]
        public void Rareness_SetEmptyOrWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var junk = new Junk();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => junk.Rareness = " ");
            Assert.Throws<ArgumentException>(() => junk.Rareness = "\t");
            Assert.Throws<ArgumentException>(() => junk.Rareness = "");
        }

        [Test]
        public void Rareness_SetValidValue_SetsRareness()
        {
            // Arrange
            var junk = new Junk();
            string rareness = "Rare";

            // Act
            junk.Rareness = rareness;

            // Assert
            Assert.AreEqual(rareness, junk.Rareness);
        }

        [Test]
        public void Value_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var junk = new Junk();

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => junk.Value = -10);
            Assert.AreEqual("Value cannot be negative. (Parameter 'Value')", ex.Message);
        }

        [Test]
        public void Value_SetValidValue_SetsValue()
        {
            // Arrange
            var junk = new Junk();
            int value = 100;

            // Act
            junk.Value = value;

            // Assert
            Assert.AreEqual(value, junk.Value);
        }

        [Test]
        public void AddToExtent_NullJunk_ThrowsArgumentException()
        {
            // Arrange
            Junk junk = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(Junk)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { junk });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("Junk cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = Junk.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<Junk>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_junk_extent.xml";
            var junk = new Junk("Metal Scrap", "Common", 50);
            Junk.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_junk_extent.xml";
            var junk = new Junk("Metal Scrap", "Common", 50);
            Junk.SaveExtent(filePath);
            
            typeof(Junk)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Junk>());

            // Act
            Junk.LoadExtent(filePath);
            var extent = Junk.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual("Metal Scrap", extent[0].Name);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var junk = new Junk("Metal Scrap", "Common", 50);

            // Act
            Junk.LoadExtent(filePath);
            var extent = Junk.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_junk_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var junk = new Junk("Metal Scrap", "Common", 50);

            // Act
            Junk.LoadExtent(filePath);
            var extent = Junk.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
