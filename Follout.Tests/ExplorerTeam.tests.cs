namespace Fallout.Tests
{
    [TestFixture]
    public class ExplorerTeamTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(ExplorerTeam)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<ExplorerTeam>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            int endPoint = 100;
            int timeToReach = 60;

            // Act
            var team = new ExplorerTeam(endPoint, timeToReach);

            // Assert
            Assert.AreEqual(endPoint, team.EndPoint);
            Assert.AreEqual(timeToReach, team.TimeToReach);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var team = new ExplorerTeam(100, 60);

            // Act
            var extent = ExplorerTeam.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(team), "Extent should contain the created ExplorerTeam.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void EndPoint_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var team = new ExplorerTeam();

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => team.EndPoint = -1);
            Assert.AreEqual("EndPoint cannot be negative. (Parameter 'EndPoint')", ex.Message);
        }

        [Test]
        public void EndPoint_SetValidValue_SetsEndPoint()
        {
            // Arrange
            var team = new ExplorerTeam();
            int endPoint = 50;

            // Act
            team.EndPoint = endPoint;

            // Assert
            Assert.AreEqual(endPoint, team.EndPoint);
        }

        [Test]
        public void TimeToReach_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var team = new ExplorerTeam();

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => team.TimeToReach = -1);
            Assert.AreEqual("TimeToReach cannot be negative. (Parameter 'TimeToReach')", ex.Message);
        }

        [Test]
        public void TimeToReach_SetValidValue_SetsTimeToReach()
        {
            // Arrange
            var team = new ExplorerTeam();
            int timeToReach = 30;

            // Act
            team.TimeToReach = timeToReach;

            // Assert
            Assert.AreEqual(timeToReach, team.TimeToReach);
        }

        [Test]
        public void MaxCapacity_SetValidValue_SetsMaxCapacity()
        {
            // Arrange
            int maxCapacity = 5;

            // Act
            ExplorerTeam.MaxCapacity = maxCapacity;

            // Assert
            Assert.AreEqual(maxCapacity, ExplorerTeam.MaxCapacity);
        }

        [Test]
        public void AddToExtent_NullTeam_ThrowsArgumentException()
        {
            // Arrange
            ExplorerTeam team = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(ExplorerTeam)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { team });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("ExplorerTeam cannot be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = ExplorerTeam.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<ExplorerTeam>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_explorerteam_extent.xml";
            var team = new ExplorerTeam(100, 60);
            ExplorerTeam.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_explorerteam_extent.xml";
            var team = new ExplorerTeam(100, 60);
            ExplorerTeam.SaveExtent(filePath);

            // Clear extent to simulate fresh load
            typeof(ExplorerTeam)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<ExplorerTeam>());

            // Act
            ExplorerTeam.LoadExtent(filePath);
            var extent = ExplorerTeam.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(100, extent[0].EndPoint);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var team = new ExplorerTeam(100, 60);

            // Act
            ExplorerTeam.LoadExtent(filePath);
            var extent = ExplorerTeam.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_explorerteam_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var team = new ExplorerTeam(100, 60);

            // Act
            ExplorerTeam.LoadExtent(filePath);
            var extent = ExplorerTeam.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
