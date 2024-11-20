using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class ResourceProduction : Room
    {
        private ResProdType _type;

        public ResProdType Type
        {
            get => _type;
            set
            {
                if (!Enum.IsDefined(typeof(ResProdType), value))
                    throw new ArgumentException("Invalid resource production type.");
                _type = value;
            }
        }

        
        public ResourceProduction() { }

    
        public ResourceProduction(ResProdType type, int lvl, string neededSkillPoints, int costToBuild,
            List<Villager> villagers)
            : base(lvl, neededSkillPoints, costToBuild, villagers)
        {
            Type = type;
            AddToExtent(this);
        }

        
        private static List<ResourceProduction> _extent = new List<ResourceProduction>();

        
        private static void AddToExtent(ResourceProduction resourceProduction)
        {
            if (resourceProduction == null)
                throw new ArgumentException("ResourceProduction cannot be null");

            _extent.Add(resourceProduction);
        }

        
        public static IReadOnlyList<ResourceProduction> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "resourceproduction_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<ResourceProduction>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "resourceproduction_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<ResourceProduction>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<ResourceProduction>)serializer.Deserialize(reader) ?? new List<ResourceProduction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
