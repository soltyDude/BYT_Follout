namespace Fallout.Tests
{
    [TestFixture]
    public class MoleRatTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(MoleRat)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<MoleRat>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            string color = "Brown";
            int hp = 80;
            int lvl = 3;
            int xp = 20;
            int damage = 10;
            string[] loot = { "Teeth", "Fur" };

            // Act
            var moleRat = new MoleRat(color, hp, lvl, xp, damage, loot);

            // Assert
            Assert.AreEqual(color, moleRat.Color);
            Assert.AreEqual(hp, moleRat.Hp);
            Assert.AreEqual(lvl, moleRat.LVL);
            Assert.AreEqual(xp, moleRat.XP);
            Assert.AreEqual(damage, moleRat.Damage);
            Assert.AreEqual(loot, moleRat.Loot);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var moleRat = new MoleRat("Brown", 80, 3, 20, 10, new string[] { "Teeth" });

            // Act
            var extent = MoleRat.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(moleRat), "Extent should contain the created MoleRat.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void Color_SetEmptyOrWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var moleRat = new MoleRat();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => moleRat.Color = " ");
            Assert.Throws<ArgumentException>(() => moleRat.Color = "\t");
            Assert.Throws<ArgumentException>(() => moleRat.Color = "");
        }

        [Test]
        public void Color_SetValidValue_SetsColor()
        {
            // Arrange
            var moleRat = new MoleRat();
            string color = "Gray";

            // Act
            moleRat.Color = color;

            // Assert
            Assert.AreEqual(color, moleRat.Color);
        }

        [Test]
        public void AddToExtent_NullMoleRat_ThrowsArgumentException()
        {
            // Arrange
            MoleRat moleRat = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(MoleRat)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { moleRat });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("MoleRat cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = MoleRat.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<MoleRat>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_molerat_extent.xml";
            var moleRat = new MoleRat("Brown", 80, 3, 20, 10, new string[] { "Teeth" });
            MoleRat.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_molerat_extent.xml";
            var moleRat = new MoleRat("Brown", 80, 3, 20, 10, new string[] { "Teeth" });
            MoleRat.SaveExtent(filePath);
            
            typeof(MoleRat)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<MoleRat>());

            // Act
            MoleRat.LoadExtent(filePath);
            var extent = MoleRat.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual("Brown", extent[0].Color);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var moleRat = new MoleRat("Brown", 80, 3, 20, 10, new string[] { "Teeth" });

            // Act
            MoleRat.LoadExtent(filePath);
            var extent = MoleRat.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_molerat_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var moleRat = new MoleRat("Brown", 80, 3, 20, 10, new string[] { "Teeth" });

            // Act
            MoleRat.LoadExtent(filePath);
            var extent = MoleRat.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
