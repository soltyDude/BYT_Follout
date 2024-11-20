using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Fallout;

namespace Fallout.Tests
{
    [TestFixture]
    public class VillagerTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Villager)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Villager>());
        }

        [Test]
        public void Constructor_WithValidParameters_SetsProperties()
        {
            // Arrange
            string name = "John";
            Gender gender = Gender.M;
            int lvlHappines = 80;
            int radiationPoisonLvl = 10;
            int intelligence = 8;
            int strength = 10;
            int perception = 6;
            int endurance = 7;
            int charisma = 5;
            int agility = 9;
            int luck = 4;
            int hp = 100;
            int lvl = 2;
            string weapon = "Rifle";
            string cloth = "Armor";

            // Act
            var villager = new Villager(name, gender, lvlHappines, radiationPoisonLvl, intelligence,
                                        strength, perception, endurance, charisma, agility, luck, hp, lvl, weapon, cloth);

            // Assert
            Assert.AreEqual(name, villager.Name);
            Assert.AreEqual(gender, villager.Gender);
            Assert.AreEqual(lvlHappines, villager.LVL_Happines);
            Assert.AreEqual(radiationPoisonLvl, villager.RadiationPoisonLVL);
            Assert.AreEqual(intelligence, villager.Intelligence);
            Assert.AreEqual(strength, villager.Strength);
            Assert.AreEqual(perception, villager.Perception);
            Assert.AreEqual(endurance, villager.Endurance);
            Assert.AreEqual(charisma, villager.Charisma);
            Assert.AreEqual(agility, villager.Agility);
            Assert.AreEqual(luck, villager.Luck);
            Assert.AreEqual(hp, villager.Hp);
            Assert.AreEqual(lvl, villager.LVL);
            Assert.AreEqual(weapon, villager.Weapon);
            Assert.AreEqual(cloth, villager.Cloth);
            Assert.AreEqual(DateTime.Now.Date, villager.BirthDate.Date);
        }

        [Test]
        public void Constructor_AddsToExtent()
        {
            // Arrange
            var villager = new Villager("John", Gender.M, 80, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor");

            // Act
            var extent = Villager.GetExtent();

            // Assert
            Assert.IsTrue(extent.Contains(villager), "Extent should contain the created Villager.");
            Assert.AreEqual(1, extent.Count);
        }

        [Test]
        public void Name_SetEmptyOrWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var villager = new Villager();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => villager.Name = " ");
            Assert.Throws<ArgumentException>(() => villager.Name = "\t");
            Assert.Throws<ArgumentException>(() => villager.Name = "");
        }

        [Test]
        public void Property_SetNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Villager("John", Gender.M, -1, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Villager("John", Gender.M, 80, -1, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Villager("John", Gender.M, 80, 10, -1, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Villager("John", Gender.M, 80, 10, 8, -1, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Villager("John", Gender.M, 80, 10, 8, 10, -1, 7, 5, 9, 4, 100, 2, "Rifle", "Armor"));
        }

        [Test]
        public void AddToExtent_NullVillager_ThrowsArgumentException()
        {
            // Arrange
            Villager villager = null;

            // Act & Assert
            var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            {
                typeof(Villager)
                    .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { villager });
            });
            
            Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
            Assert.AreEqual("Villager cant be null", exception.InnerException.Message);
        }

        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = Villager.GetExtent();

            // Assert
            Assert.IsInstanceOf<IReadOnlyList<Villager>>(extent);
        }

        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_villager_extent.xml";
            var villager = new Villager("John", Gender.M, 80, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor");
            Villager.SaveExtent(filePath);

            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_villager_extent.xml";
            var villager = new Villager("John", Gender.M, 80, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor");
            Villager.SaveExtent(filePath);
            
            typeof(Villager)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<Villager>());

            // Act
            Villager.LoadExtent(filePath);
            var extent = Villager.GetExtent();

            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual("John", extent[0].Name);

            // Cleanup
            File.Delete(filePath);
        }

        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var villager = new Villager("John", Gender.M, 80, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor");

            // Act
            Villager.LoadExtent(filePath);
            var extent = Villager.GetExtent();

            // Assert
            Assert.IsEmpty(extent);
        }

        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_villager_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");

            var villager = new Villager("John", Gender.M, 80, 10, 8, 10, 6, 7, 5, 9, 4, 100, 2, "Rifle", "Armor");

            // Act
            Villager.LoadExtent(filePath);
            var extent = Villager.GetExtent();

            // Assert
            Assert.IsEmpty(extent);

            // Cleanup
            File.Delete(filePath);
        }
    }
}
