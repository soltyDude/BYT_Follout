using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Villager : Human
    {
        private string? _name;
        private string? _weapon;
        private string? _cloth;
        public string? Name
        {
            get => _name;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cant be empty");
                _name = value;
            }
        }
        public Gender Gender { get; set; }
        public int LVL_Happines { get; set; }
        public int RadiationPoisonLVL { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public DateTime BirthDate { get; set; }

        
        public Villager() { }

        
        public Villager(string name, Gender gender, int lvlHappines, int radiationPoisonLvl, int intelligence,
                        int strength, int perception, int endurance, int charisma, int agility, int luck, int hp, int lvl, string weapon, string cloth)
        : base(hp, lvl, weapon, cloth)
        {
            Name = name;
            Gender = gender;
            LVL_Happines = lvlHappines >= 0 ? lvlHappines : throw new ArgumentOutOfRangeException(nameof(lvlHappines), "Happiness level cant be negative.");
            RadiationPoisonLVL = radiationPoisonLvl >= 0 ? radiationPoisonLvl : throw new ArgumentOutOfRangeException(nameof(radiationPoisonLvl), "Radiation level cant be negative.");
            Intelligence = intelligence >= 0 ? intelligence : throw new ArgumentOutOfRangeException(nameof(intelligence), "Intelligence cant be negative.");
            Strength = strength >= 0 ? strength : throw new ArgumentOutOfRangeException(nameof(strength), "Strength cant be negative.");
            Perception = perception >= 0 ? perception : throw new ArgumentOutOfRangeException(nameof(perception), "Perception cant be negative.");
            Endurance = endurance >= 0 ? endurance : throw new ArgumentOutOfRangeException(nameof(endurance), "Endurance cant be negative.");
            Charisma = charisma >= 0 ? charisma : throw new ArgumentOutOfRangeException(nameof(charisma), "Charisma cant be negative.");
            Agility = agility >= 0 ? agility : throw new ArgumentOutOfRangeException(nameof(agility), "Agility cant be negative.");
            Luck = luck >= 0 ? luck : throw new ArgumentOutOfRangeException(nameof(luck), "Luck cant be negative.");
            BirthDate = DateTime.Now;

            AddToExtent(this);
        }


        private static List<Villager> _extent = new List<Villager>();

        
        private static void AddToExtent(Villager villager)
        {
            if (villager == null)
                throw new ArgumentException("Villager cant be null");

            _extent.Add(villager);
        }

        
        public static IReadOnlyList<Villager> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "villager_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Villager>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "villager_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Villager>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Villager>)serializer.Deserialize(reader) ?? new List<Villager>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
