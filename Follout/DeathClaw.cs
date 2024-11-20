using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class DeathClaw : Monster
    {
        private int _size;

        public int Size
        {
            get => _size;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Size), "Size cannot be negative.");
                _size = value;
            }
        }
        public DeathClaw() { }

        public DeathClaw(int size, int hp, int lvl, int xp, int damage, string[] loot)
            : base( hp,  lvl,  xp, damage, loot)
        {
            Size = size;
            AddToExtent(this);
        }

        private static List<DeathClaw> _extent = new List<DeathClaw>();

        private static void AddToExtent(DeathClaw deathClaw)
        {
            if (deathClaw == null)
                throw new ArgumentException("DeathClaw cannot be null");

            _extent.Add(deathClaw);
        }

        public static IReadOnlyList<DeathClaw> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "deathclaw_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<DeathClaw>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "deathclaw_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<DeathClaw>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<DeathClaw>)serializer.Deserialize(reader) ?? new List<DeathClaw>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}