using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fallout
{
    [Serializable]
    public class ExplorerTeam
    {
        public static int MaxCapacity { get; set; } = 3;
        private int _endPoint;
        private int _timeToReach;

         public int EndPoint
        {
            get => _endPoint;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(EndPoint), "EndPoint cannot be negative.");
                _endPoint = value;
            }
        }

        public int TimeToReach
        {
            get => _timeToReach;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TimeToReach), "TimeToReach cannot be negative.");
                _timeToReach = value;
            }
        }


        public ExplorerTeam() { }

        public ExplorerTeam(int endPoint, int timeToReach)
        {
            EndPoint = endPoint;
            TimeToReach = timeToReach;

            AddToExtent(this);
        }

        private static List<ExplorerTeam> _extent = new List<ExplorerTeam>();

        private static void AddToExtent(ExplorerTeam team)
        {
            if (team == null)
                throw new ArgumentException("ExplorerTeam cannot be null");

            _extent.Add(team);
        }

        public static IReadOnlyList<ExplorerTeam> GetExtent()
        {
            return _extent.AsReadOnly();
        }

        public static void SaveExtent(string filePath = "explorerteam_extent.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<ExplorerTeam>));
                using XmlTextWriter writer = new XmlTextWriter(file);
                serializer.Serialize(writer, _extent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        public static void LoadExtent(string filePath = "explorerteam_extent.xml")
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _extent.Clear();
                    return;
                }

                using StreamReader file = File.OpenText(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<ExplorerTeam>));
                using XmlTextReader reader = new XmlTextReader(file);
                _extent = (List<ExplorerTeam>)serializer.Deserialize(reader) ?? new List<ExplorerTeam>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent: {ex.Message}");
                _extent.Clear();
            }
        }
    }
}
