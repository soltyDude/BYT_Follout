[TestFixture]
public class ArmorTests
{
    [SetUp]
    public void Setup()
    {
        typeof(Armor)
            .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, new List<Armor>());
    }

    [Test]
    public void ArmorConstructor_WithValidValues_SetsProperties()
    {
        // Arrange
        int intelligence = 5;
        int strength = 10;
        int perception = 8;
        int endurance = 7;
        int charisma = 6;
        int agility = 9;
        int luck = 4;

        // Act
        var armor = new Armor(intelligence, strength, perception, endurance, charisma, agility, luck);

        // Assert
        Assert.AreEqual(intelligence, armor.Intelligence);
        Assert.AreEqual(strength, armor.Strength);
        Assert.AreEqual(perception, armor.Perception);
        Assert.AreEqual(endurance, armor.Endurance);
        Assert.AreEqual(charisma, armor.Charisma);
        Assert.AreEqual(agility, armor.Agility);
        Assert.AreEqual(luck, armor.Luck);
    }

    [Test]
    public void ArmorConstructor_AddsToExtent()
    {
        // Arrange
        int intelligence = 5;

        // Act
        var armor = new Armor(intelligence, 0, 0, 0, 0, 0, 0);

        // Assert
        var extent = Armor.GetExtent();
        Assert.IsTrue(extent.Contains(armor), "Extent should contain the created armor.");
        Assert.AreEqual(1, extent.Count);
    }

    [Test]
    public void PropertySetter_WithNegativeValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var armor = new Armor();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Intelligence = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Strength = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Perception = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Endurance = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Charisma = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Agility = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => armor.Luck = -1);
    }

    [Test]
    public void PropertySetter_WithValidValues_SetsPropertiesCorrectly()
    {
        // Arrange
        var armor = new Armor();
        int value = 10;

        // Act
        armor.Intelligence = value;
        armor.Strength = value;
        armor.Perception = value;
        armor.Endurance = value;
        armor.Charisma = value;
        armor.Agility = value;
        armor.Luck = value;

        // Assert
        Assert.AreEqual(value, armor.Intelligence);
        Assert.AreEqual(value, armor.Strength);
        Assert.AreEqual(value, armor.Perception);
        Assert.AreEqual(value, armor.Endurance);
        Assert.AreEqual(value, armor.Charisma);
        Assert.AreEqual(value, armor.Agility);
        Assert.AreEqual(value, armor.Luck);
    }

    [Test]
    public void AddToExtent_NullArmor_ThrowsArgumentException()
    {
        // Arrange
        Armor armor = null;

        // Act & Assert
        var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
        {
            typeof(Armor)
                .GetMethod("AddToExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { armor });
        });

        // Verify that the inner exception is the expected ArgumentException
        Assert.IsInstanceOf<ArgumentException>(exception.InnerException);
        Assert.AreEqual("Armor cannot be null", exception.InnerException.Message);
    }

    [Test]
    public void GetExtent_ReturnsReadOnlyList()
    {
        // Act
        var extent = Armor.GetExtent();

        // Assert
        Assert.IsInstanceOf<IReadOnlyList<Armor>>(extent);
    }

    [Test]
    public void SaveExtent_SavesToFile()
    {
        // Arrange
        string filePath = "test_armor_extent.xml";
        var armor = new Armor(1, 2, 3, 4, 5, 6, 7);
        Armor.SaveExtent(filePath);

        // Act & Assert
        Assert.IsTrue(File.Exists(filePath));

        // Cleanup
        File.Delete(filePath);
    }

    [Test]
    public void LoadExtent_LoadsFromFile()
    {
        // Arrange
        string filePath = "test_armor_extent.xml";
        var armor = new Armor(1, 2, 3, 4, 5, 6, 7);
        Armor.SaveExtent(filePath);
        
        typeof(Armor)
            .GetField("_extent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, new List<Armor>());

        // Act
        Armor.LoadExtent(filePath);
        var extent = Armor.GetExtent();

        // Assert
        Assert.AreEqual(1, extent.Count);
        Assert.AreEqual(1, extent[0].Intelligence);

        // Cleanup
        File.Delete(filePath);
    }

    [Test]
    public void LoadExtent_WithNonExistingFile_ClearsExtent()
    {
        // Arrange
        string filePath = "non_existing_file.xml";
        var armor = new Armor(1, 2, 3, 4, 5, 6, 7);

        // Act
        Armor.LoadExtent(filePath);
        var extent = Armor.GetExtent();

        // Assert
        Assert.IsEmpty(extent);
    }

    [Test]
    public void LoadExtent_WithInvalidXml_ThrowsExceptionAndClearsExtent()
    {
        // Arrange
        string filePath = "invalid_armor_extent.xml";
        File.WriteAllText(filePath, "Invalid XML Content");

        var armor = new Armor(1, 2, 3, 4, 5, 6, 7);

        // Act
        Armor.LoadExtent(filePath);
        var extent = Armor.GetExtent();

        // Assert
        Assert.IsEmpty(extent);

        // Cleanup
        File.Delete(filePath);
    }
}
