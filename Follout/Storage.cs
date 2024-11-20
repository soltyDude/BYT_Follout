using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Storage : Room
    {
       private int _capacity;

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Capacity), "Capacity cannot be negative.");
                _capacity = value;
            }
        }

        
        public Storage() { }

        
        public Storage(int capacity, int lvl, String neededSkillPoints, int costToBuild, List<Villager> villagers)
            : base(lvl, neededSkillPoints, costToBuild, villagers)
        {
            Capacity = capacity;
            AddToExtent(this);
        }

       
        private static List<Storage> _extent = new List<Storage>();

        
        private static void AddToExtent(Storage storage)
        {
            if (storage == null)
                throw new ArgumentException("Storage cannot be null");

            _extent.Add(storage);
        }

        
        public static IReadOnlyList<Storage> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "storage_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Storage>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "storage_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Storage>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Storage>)serializer.Deserialize(reader) ?? new List<Storage>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
