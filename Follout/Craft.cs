namespace Fallout;

public abstract class Craft : Room
{
    protected Craft()
    {
    }

    protected Craft(int lvl, String neededSkillPoints, int costToBuild, List<Villager> villagers)
        : base(lvl, neededSkillPoints, costToBuild, villagers)
    {
    }
}