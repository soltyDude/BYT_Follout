namespace Fallout;

public abstract class Room
{
    private int NumberOfSP { get; set; }
    private static int originaltimetoCreate = 100;

    private int _lvl;

    private string _neededSkillPoints;

    public int LVL
        {
            get => _lvl;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(LVL), "LVL must be positive.");
                _lvl = value;
            }
        }
    public int Size { get; set; }
    public int MaxNumVillagers { get; set; }
    public int TimeToCreate { get; set; }
    public string NeededSkillPoints
        {
            get => _neededSkillPoints;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("NeededSkillPoints cannot be null or whitespace.");
                _neededSkillPoints = value;
            }
        }    public int CostToBuild { get; set; }
    public int CostToUpgrade { get; set; }
    public List<Villager> Villagers { get; set; }
    
    protected Room(int lvl, string neededSkillPoints, int costToBuild, List<Villager> villagers)
    {
        LVL = lvl;
        NeededSkillPoints = neededSkillPoints;
        CostToBuild = costToBuild;
        MaxNumVillagers = LVL * 2;
        for (int i = 0; i < MaxNumVillagers; i++)
        {
            Villagers[i] = villagers[i];
            Console.WriteLine(Villagers[i].Name + " was aded");
        }
        Size = LVL * 2;
        TimeToCreate = originaltimetoCreate -(originaltimetoCreate * UpdateSP() / 10);
        CostToUpgrade = 100 * LVL;
    }

    protected Room()
    {
    }

    public int UpdateSP()
    {
        int totalSkillPoints = 0;

        foreach (var villager in Villagers)
        {
            try
            {
                switch (NeededSkillPoints)
                {
                    case "Intelligence":
                        totalSkillPoints += villager.Intelligence;
                        break;
                    case "Strength":
                        totalSkillPoints += villager.Strength;
                        break;
                    case "Perception":
                        totalSkillPoints += villager.Perception;
                        break;
                    case "Endurance":
                        totalSkillPoints += villager.Endurance;
                        break;
                    case "Charisma":
                        totalSkillPoints += villager.Charisma;
                        break;
                    case "Agility":
                        totalSkillPoints += villager.Agility;
                        break;
                    case "Luck":
                        totalSkillPoints += villager.Luck;
                        break;
                    default:
                        throw new ArgumentException($"Unknown skill: {NeededSkillPoints}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        NumberOfSP = totalSkillPoints;
        
        return totalSkillPoints;
    }

}
