using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
 
namespace Fallout.Tests
{
    [TestFixture]
    public class RoomCreationCloathTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(RoomCreationCloath)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<RoomCreationCloath>());
        }
 
        [Test]
        public void RoomCreationCloathConstructor_WithValidValues_SetsProperties()
        {
            // Arrange
            int timeToCreate = 100;
            int cost = 200;
            int lvl = 2;
            string neededSkillPoints = "Intelligence";
            int costToBuild = 500;
            var villagers = new List<Villager>
            {
                new Villager { Name = "Alice", Intelligence = 5 },
                new Villager { Name = "Bob", Intelligence = 6 }
            };
 
            // Act
            var room = new RoomCreationCloath(timeToCreate, cost, lvl, neededSkillPoints, costToBuild, villagers);
 
            // Assert
            Assert.AreEqual(timeToCreate, room.TimeToCreate);
            Assert.AreEqual(cost, room.Cost);
            Assert.AreEqual(lvl, room.LVL);
            Assert.AreEqual(neededSkillPoints, room.NeededSkillPoints);
            Assert.AreEqual(costToBuild, room.CostToBuild);
            Assert.AreEqual(villagers, room.Villagers);
        }
 
        [Test]
        public void RoomCreationCloathConstructor_AddsToExtent()
        {
            // Arrange
            int timeToCreate = 100;
            int cost = 200;
 
            // Act
            var room = new RoomCreationCloath(timeToCreate, cost, 1, "Intelligence", 500, new List<Villager>());
            var extent = RoomCreationCloath.GetExtent();
 
            // Assert
            Assert.IsTrue(extent.Contains(room), "Extent should contain the created RoomCreationCloath.");
            Assert.AreEqual(1, extent.Count);
        }
 
        [Test]
        public void PropertySetter_WithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var room = new RoomCreationCloath();
 
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => room.TimeToCreate = -1);
            Assert.Throws<ArgumentOutOfRangeException>(() => room.Cost = -1);
        }
 
        [Test]
        public void GetExtent_ReturnsReadOnlyList()
        {
            // Act
            var extent = RoomCreationCloath.GetExtent();
 
            // Assert
            Assert.IsInstanceOf<IReadOnlyList<RoomCreationCloath>>(extent);
        }
 
        [Test]
        public void SaveExtent_SavesToFile()
        {
            // Arrange
            string filePath = "test_room_creation_cloath_extent.xml";
            var room = new RoomCreationCloath(100, 200, 1, "Strength", 500, new List<Villager>());
            RoomCreationCloath.SaveExtent(filePath);
 
            // Act & Assert
            Assert.IsTrue(File.Exists(filePath));
 
            // Cleanup
            File.Delete(filePath);
        }
 
        [Test]
        public void LoadExtent_LoadsFromFile()
        {
            // Arrange
            string filePath = "test_room_creation_cloath_extent.xml";
            var room = new RoomCreationCloath(100, 200, 1, "Charisma", 500, new List<Villager>());
            RoomCreationCloath.SaveExtent(filePath);
 
            typeof(RoomCreationCloath)
                .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new List<RoomCreationCloath>());
 
            // Act
            RoomCreationCloath.LoadExtent(filePath);
            var extent = RoomCreationCloath.GetExtent();
 
            // Assert
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(100, extent[0].TimeToCreate);
 
            // Cleanup
            File.Delete(filePath);
        }
 
        [Test]
        public void LoadExtent_WithNonExistingFile_ClearsExtent()
        {
            // Arrange
            string filePath = "non_existing_file.xml";
            var room = new RoomCreationCloath(100, 200, 1, "Luck", 500, new List<Villager>());
 
            // Act
            RoomCreationCloath.LoadExtent(filePath);
            var extent = RoomCreationCloath.GetExtent();
 
            // Assert
            Assert.IsEmpty(extent);
        }
 
        [Test]
        public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
        {
            // Arrange
            string filePath = "invalid_room_creation_cloath_extent.xml";
            File.WriteAllText(filePath, "Invalid XML Content");
 
            // Act
            RoomCreationCloath.LoadExtent(filePath);
            var extent = RoomCreationCloath.GetExtent();
 
            // Assert
            Assert.IsEmpty(extent);
 
            // Cleanup
            File.Delete(filePath);
        }
    }
}