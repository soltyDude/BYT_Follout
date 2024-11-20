using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class Pet
    {
         private string? _buff;

        public string? Buff
        {
            get => _buff;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Buff cannot be empty or whitespace.");
                _buff = value;
            }
        }


        
        public Pet() { }

        public Pet(string buff)
        {
            Buff = buff;
            AddToExtent(this);
        }

        
        private static List<Pet> _extent = new List<Pet>();

        private static void AddToExtent(Pet pet)
        {
            if (pet == null)
                throw new ArgumentException("Pet cannot be null");

            _extent.Add(pet);
        }

        public static IReadOnlyList<Pet> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "pet_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pet>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "pet_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pet>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<Pet>)serializer.Deserialize(reader) ?? new List<Pet>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
