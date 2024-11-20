using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Training : Room
    {
        private int _skillPoint;

        public int SkillPoint
        {
            get => _skillPoint;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(SkillPoint), "SkillPoint cannot be negative.");
                _skillPoint = value;
            }
        }
        
        public Training() { }

        
        public Training(int skillPoint, int lvl, String neededSkillPoints, int costToBuild, List<Villager> villagers)
            : base(lvl, neededSkillPoints, costToBuild, villagers)
        {
            SkillPoint = skillPoint;
            AddToExtent(this);
        }

        
        private static List<Training> _extent = new List<Training>();

       
        private static void AddToExtent(Training training)
        {
            if (training == null)
                throw new ArgumentException("Training cannot be null");

            _extent.Add(training);
        }

       
        public static IReadOnlyList<Training> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "training_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Training>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "training_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Training>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Training>)serializer.Deserialize(reader) ?? new List<Training>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
