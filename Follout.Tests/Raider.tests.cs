namespace Fallout.Tests
{
    [TestFixture]
    public class RaiderTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Raider)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Raider>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            int hp = 120;
            int lvl = 4;
            int xp = 40;
            int damage = 15;
            string[] loot = { "Gun", "Ammo" };

            // Act
            var raider = new Raider(hp, lvl, xp, damage, loot);

            // Assert
            Assert.AreEqual(hp, raider.Hp);
            Assert.AreEqual(lvl, raider.LVL);
            Assert.AreEqual(xp, raider.XP);
            Assert.AreEqual(damage, raider.Damage);
            Assert.AreEqual(loot, raider.Loot);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var raider = new Raider(120, 4, 40, 15, new string[] { "Gun" });

            // Act
            var extent = Raider.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(raider), "Extent should contain the created Raider.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void IsCapturable_ReturnsBoolean()
        {
            // Act
            bool result = Raider.IsCapturable();

            // Assert
            Assert.IsInstanceOf<bool>(result);
        }

        [Test]
        public void IsCapturable_ReturnsTrueWithProbability()
        {
            // Arrange
            int trials = 1000;
            int captureCount = 0;

            // Act
            for (int i = 0; i < trials; i++)
            {
                if (Raider.IsCapturable())
                {
                    captureCount++;
                }
            }

            // Assert
            double captureRate = captureCount / (double)trials;
            Assert.IsTrue(captureRate >= 0.25 && captureRate <= 0.35, "Capture rate should be around the specified probability (0.3).");
        }

        [Test]
        public void AddToExtent_NullRaider_ThrowsArgumentException()
        {
            // Arrange
            Raider raider = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(Raider)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { raider });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("Raider cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = Raider.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<Raider>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_raider_extent.xml";
            var raider = new Raider(120, 4, 40, 15, new string[] { "Gun" });
            Raider.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_raider_extent.xml";
            var raider = new Raider(120, 4, 40, 15, new string[] { "Gun" });
            Raider.SaveExtent(filePath);
            
            typeof(Raider)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Raider>());

            // Act
            Raider.LoadExtent(filePath);
            var extent = Raider.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(120, extent[0].Hp);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var raider = new Raider(120, 4, 40, 15, new string[] { "Gun" });

            // Act
            Raider.LoadExtent(filePath);
            var extent = Raider.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_raider_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var raider = new Raider(120, 4, 40, 15, new string[] { "Gun" });

            // Act
            Raider.LoadExtent(filePath);
            var extent = Raider.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
