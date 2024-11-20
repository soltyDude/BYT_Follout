namespace Fallout.Tests
{
    [TestFixture]
    public class DeathClawTests
    {
        [SetUp]
        public void Setup()
        {
            
            typeof(DeathClaw)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<DeathClaw>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            int size = 3;
            int hp = 100;
            int lvl = 5;
            int xp = 50;
            int damage = 25;
            string[] loot = { "Claw", "Fur" };

            // Act
            var deathClaw = new DeathClaw(size, hp, lvl, xp, damage, loot);

            // Assert
            Assert.AreEqual(size, deathClaw.Size);
            Assert.AreEqual(hp, deathClaw.Hp);
            Assert.AreEqual(lvl, deathClaw.LVL);
            Assert.AreEqual(xp, deathClaw.XP);
            Assert.AreEqual(damage, deathClaw.Damage);
            Assert.AreEqual(loot, deathClaw.Loot);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            int size = 3;
            var deathClaw = new DeathClaw(size, 100, 5, 50, 25, new string[] { "Claw" });

            // Act
            var extent = DeathClaw.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(deathClaw), "Extent should contain the created DeathClaw.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void Size_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var deathClaw = new DeathClaw();

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => deathClaw.Size = -1);
            Assert.AreEqual("Size cannot be negative. (Parameter 'Size')", ex.Message);
        }

        [Test]
        public void Size_SetValidValue_SetsSize()
        {
            // Arrange
            var deathClaw = new DeathClaw();
            int size = 5;

            // Act
            deathClaw.Size = size;

            // Assert
            Assert.AreEqual(size, deathClaw.Size);
        }

        [Test]
        public void AddToExtent_NullDeathClaw_ThrowsArgumentException()
        {
            // Arrange
            DeathClaw deathClaw = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(DeathClaw)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { deathClaw });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("DeathClaw cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = DeathClaw.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<DeathClaw>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_deathclaw_extent.xml";
            var deathClaw = new DeathClaw(3, 100, 5, 50, 25, new string[] { "Claw" });
            DeathClaw.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_deathclaw_extent.xml";
            var deathClaw = new DeathClaw(3, 100, 5, 50, 25, new string[] { "Claw" });
            DeathClaw.SaveExtent(filePath);

            // Clear extent to simulate fresh load
            typeof(DeathClaw)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<DeathClaw>());

            // Act
            DeathClaw.LoadExtent(filePath);
            var extent = DeathClaw.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(3, extent[0].Size);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var deathClaw = new DeathClaw(3, 100, 5, 50, 25, new string[] { "Claw" });

            // Act
            DeathClaw.LoadExtent(filePath);
            var extent = DeathClaw.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_deathclaw_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var deathClaw = new DeathClaw(3, 100, 5, 50, 25, new string[] { "Claw" });

            // Act
            DeathClaw.LoadExtent(filePath);
            var extent = DeathClaw.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
