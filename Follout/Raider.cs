using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Raider : Monster
    {
        private static Random random = new Random();
        
        private static double PercentageOfCapturable = 0.3;
        private bool IsItCapturable; 

        public Raider() { }

        
        public Raider(int hp, int lvl, int xp, int damage, string[] loot)
            : base( hp,  lvl,  xp, damage, loot)
        {
            IsItCapturable = IsCapturable();
            AddToExtent(this);
        }

        private static List<Raider> _extent = new List<Raider>();

        private static void AddToExtent(Raider raider)
        {
            if (raider == null)
                throw new ArgumentException("Raider cannot be null");

            _extent.Add(raider);
        }

        public static IReadOnlyList<Raider> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "raider_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Raider>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "raider_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Raider>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Raider>)serializer.Deserialize(reader) ?? new List<Raider>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }

        public static bool IsCapturable()
        {
            double randomValue = random.NextDouble();
            return randomValue < PercentageOfCapturable;
        }
    }
}
