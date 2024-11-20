using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class Armor
{
    private int _intelligence;
    private int _strength;
    private int _perception;
    private int _endurance;
    private int _charisma;
    private int _agility;
    private int _luck;


     public int Intelligence
    {
        get => _intelligence;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Intelligence), "Intelligence cannot be negative.");
            _intelligence = value;
        }
    }

    public int Strength
    {
        get => _strength;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Strength), "Strength cannot be negative.");
            _strength = value;
        }
    }

    public int Perception
    {
        get => _perception;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Perception), "Perception cannot be negative.");
            _perception = value;
        }
    }

    public int Endurance
    {
        get => _endurance;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Endurance), "Endurance cannot be negative.");
            _endurance = value;
        }
    }

    public int Charisma
    {
        get => _charisma;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Charisma), "Charisma cannot be negative.");
            _charisma = value;
        }
    }

    public int Agility
    {
        get => _agility;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Agility), "Agility cannot be negative.");
            _agility = value;
        }
    }

    public int Luck
    {
        get => _luck;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Luck), "Luck cannot be negative.");
            _luck = value;
        }
    }

    public Armor() { }

    public Armor(int intelligence, int strength, int perception, int endurance, int charisma, int agility, int luck)
    {
        Intelligence = intelligence;
        Strength = strength;
        Perception = perception;
        Endurance = endurance;
        Charisma = charisma;
        Agility = agility;
        Luck = luck;

        AddToExtent(this);
    }

    private static List<Armor> _extent = new List<Armor>();

    private static void AddToExtent(Armor armor)
    {
        if (armor == null)
            throw new ArgumentException("Armor cannot be null");

        _extent.Add(armor);
    }

    public static IReadOnlyList<Armor> GetExtent()
    {
        return _extent.AsReadOnly();
    }

    public static void SaveExtent(string filePath = "armor_extent.xml")
    {
        try
        {
            using StreamWriter file = File.CreateText(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Armor>));
            using XmlTextWriter writer = new XmlTextWriter(file);
            serializer.Serialize(writer, _extent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving extent: {ex.Message}");
        }
    }

    public static void LoadExtent(string filePath = "armor_extent.xml")
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _extent.Clear();
                return;
            }

            using StreamReader file = File.OpenText(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Armor>));
            using XmlTextReader reader = new XmlTextReader(file);
            _extent = (List<Armor>)serializer.Deserialize(reader) ?? new List<Armor>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading extent: {ex.Message}");
            _extent.Clear();
        }
    }
}
