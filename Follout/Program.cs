using System;
using System.Collections.Generic;

namespace Fallout
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var villager1 = new Villager
            {
                Name = "Alice",
                Intelligence = 5,
                Strength = 6,
                Perception = 7,
                Endurance = 8,
                Charisma = 4,
                Agility = 5,
                Luck = 3
            };

            var villager2 = new Villager
            {
                Name = "Bob",
                Intelligence = 6,
                Strength = 7,
                Perception = 5,
                Endurance = 6,
                Charisma = 8,
                Agility = 4,
                Luck = 5
            };

            var villager3 = new Villager
            {
                Name = "Charlie",
                Intelligence = 8,
                Strength = 6,
                Perception = 6,
                Endurance = 7,
                Charisma = 5,
                Agility = 6,
                Luck = 4
            };

            
            var villagers = new List<Villager> { villager1, villager2, villager3 };

            
            var room = new ExampleRoom(1, "Intelligence", 100, villagers);

            
            string[] skills = { "Intelligence", "Strength", "Perception", "Endurance", "Charisma", "Agility", "Luck" };

            foreach (var skill in skills)
            {
                int totalSkillPoints = room.UpdateSP();
                Console.WriteLine($"Total {skill} of all villagers: {totalSkillPoints}");
            }
        }
    }

    
    public class ExampleRoom : Room
    {
        public ExampleRoom(int lvl, string neededSkillPoints, int costToBuild, List<Villager> villagers)
            : base(lvl, neededSkillPoints, costToBuild, villagers) { }
    }
}
