using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class MoleRat : Monster
    {
       private string? _color;

        public string? Color
        {
            get => _color;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Color cannot be empty or whitespace.");
                _color = value;
            }
        }

        
        public MoleRat()
        { }

        public MoleRat(string color, int hp, int lvl, int xp, int damage, string[] loot)
            : base( hp,  lvl,  xp, damage, loot)
        {
            Color = color;
            AddToExtent(this);
        }

        
        private static List<MoleRat> _extent = new List<MoleRat>();

        private static void AddToExtent(MoleRat moleRat)
        {
            if (moleRat == null)
                throw new ArgumentException("MoleRat cannot be null");

            _extent.Add(moleRat);
        }

        public static IReadOnlyList<MoleRat> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "molerat_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<MoleRat>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "molerat_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<MoleRat>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<MoleRat>)serializer.Deserialize(reader) ?? new List<MoleRat>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
