using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class RoomCreationCloath : Craft
    {
        private int _timeToCreate;
        private int _cost;

        public int TimeToCreate
        {
            get => _timeToCreate;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TimeToCreate), "TimeToCreate cannot be negative.");
                _timeToCreate = value;
            }
        }

        public int Cost
        {
            get => _cost;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Cost), "Cost cannot be negative.");
                _cost = value;
            }
        }

        public RoomCreationCloath() { }

        public RoomCreationCloath(int timeToCreate, int cost, int lvl, String neededSkillPoints, int costToBuild, List<Villager> villagers)
            : base(lvl, neededSkillPoints, costToBuild, villagers)
        {
            TimeToCreate = timeToCreate;
            Cost = cost;
            AddToExtent(this);
        }

        
        private static List<RoomCreationCloath> _extent = new List<RoomCreationCloath>();

        
        private static void AddToExtent(RoomCreationCloath roomCreationCloath)
        {
            if (roomCreationCloath == null)
                throw new ArgumentException("RoomCreationCloath cannot be null");

            _extent.Add(roomCreationCloath);
        }

       
        public static IReadOnlyList<RoomCreationCloath> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "roomcreationcloath_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<RoomCreationCloath>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "roomcreationcloath_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<RoomCreationCloath>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<RoomCreationCloath>)serializer.Deserialize(reader) ?? new List<RoomCreationCloath>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
