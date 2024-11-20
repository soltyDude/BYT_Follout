using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Junk
    {
        private string? _name;
        private string? _rareness;
        private int _value;

        public string? Name
        {
            get => _name;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty or whitespace.");
                _name = value;
            }
        }

        public string? Rareness
        {
            get => _rareness;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Rareness cannot be empty or whitespace.");
                _rareness = value;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Value), "Value cannot be negative.");
                _value = value;
            }
        }


        public Junk() { }

        public Junk(string name, string rareness, int value)
        {
            Name = name;
            Rareness = rareness;
            Value = value;

            AddToExtent(this);
        }

        
        private static List<Junk> _extent = new List<Junk>();

        private static void AddToExtent(Junk junk)
        {
            if (junk == null)
                throw new ArgumentException("Junk cannot be null");

            _extent.Add(junk);
        }

        public static IReadOnlyList<Junk> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "junk_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Junk>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "junk_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Junk>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Junk>)serializer.Deserialize(reader) ?? new List<Junk>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
