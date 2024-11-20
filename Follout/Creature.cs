namespace Fallout;

public abstract class Creature
{
     private int _hp;
     private int _lvl;

    public int Hp
        {
            get => _hp;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Hp), "Hp cannot be negative.");
                _hp = value;
            }
        }

    public int LVL
        {
            get => _lvl;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(LVL), "Level cannot be negative.");
                _lvl = value;
            }
        }
    
    protected Creature(int hp, int lvl)
    {
        Hp = hp;
        LVL = lvl;
    }
    
    protected Creature()
    {
    }
    
}
