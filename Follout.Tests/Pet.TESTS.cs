namespace Fallout.Tests
{
    [TestFixture]
    public class PetTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Pet)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Pet>());
        }

        [Test]
        public void PetConstructor_WithValidBuff_SetsBuff()
        {
            // Arrange
            string buff = "Increased Carry Weight";

            // Act
            var pet = new Pet(buff);

            // Assert
            Assert.AreEqual(buff, pet.Buff);
        }

        [Test]
        public void PetConstructor_AddsToExtent()
        {
            // Arrange
            string buff = "Increased Carry Weight";

            // Act
            var pet = new Pet(buff);

            // Assert
            var extent = Pet.GetExtent();
            Assert.IsTrue(extent.Contains(pet), "Extent should contain the created pet.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void BuffSetter_WithValidValue_SetsBuff()
        {
            // Arrange
            var pet = new Pet();
            string buff = "Increased Carry Weight";

            // Act
            pet.Buff = buff;

            // Assert
            Assert.AreEqual(buff, pet.Buff);
        }

        [Test]
        public void BuffSetter_WithEmptyValue_ThrowsArgumentException()
        {
            // Arrange
            var pet = new Pet();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => pet.Buff = " ");
        }

        [Test]
        public void BuffSetter_WithNullValue_SetsBuffToNull()
        {
            // Arrange
            var pet = new Pet("Initial Buff");

            // Act
            pet.Buff = null;

            // Assert
            Assert.IsNull(pet.Buff);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = Pet.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<Pet>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_pet_extent.xml";
            var pet = new Pet("Buff 1");
            Pet.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_pet_extent.xml";
            var pet1 = new Pet("Buff 1");
            Pet.SaveExtent(filePath);
            
            typeof(Pet)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Pet>());

            // Act
            Pet.LoadExtent(filePath);
            var extent = Pet.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual("Buff 1", extent[0].Buff);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var pet = new Pet("Buff 1");

            // Act
            Pet.LoadExtent(filePath);
            var extent = Pet.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_pet_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var pet = new Pet("Buff 1");

            // Act
            Pet.LoadExtent(filePath);
            var extent = Pet.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
