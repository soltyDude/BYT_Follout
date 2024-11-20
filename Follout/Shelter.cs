using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Shelter
    {
        public int Number { get; set; }
        public int NuKaColaKaps { get; set; }
        public int NuKaCola { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    
        public Shelter() { }

        
        public Shelter(int number, int nuKaColaKaps, int nuKaCola)
        {
            Number = number >= 0 ? number : throw new ArgumentOutOfRangeException(nameof(number), "Number cannot be negative.");
            NuKaColaKaps = nuKaColaKaps >= 0 ? nuKaColaKaps : throw new ArgumentOutOfRangeException(nameof(nuKaColaKaps), "NuKaColaKaps cannot be negative.");
            NuKaCola = nuKaCola >= 0 ? nuKaCola : throw new ArgumentOutOfRangeException(nameof(nuKaCola), "NuKaCola cannot be negative.");
            AddToExtent(this);
        }

        
        private static List<Shelter> _extent = new List<Shelter>();

        
        private static void AddToExtent(Shelter shelter)
        {
            if (shelter == null)
                throw new ArgumentException("Shelter cannot be null");

            _extent.Add(shelter);
        }

        
        public static IReadOnlyList<Shelter> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        
        public static void SaveExtent(string filePath = "shelter_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Shelter>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        
        public static void LoadExtent(string filePath = "shelter_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Shelter>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Shelter>)serializer.Deserialize(reader) ?? new List<Shelter>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
