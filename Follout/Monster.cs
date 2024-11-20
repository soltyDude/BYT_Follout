namespace Fallout;

public abstract class Monster : Creature
{
    private int _xp;
    private int _damage;
    private string[] _loot;

        public int XP
        {
            get => _xp;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(XP), "XP cannot be negative.");
                _xp = value;
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Damage), "Damage cannot be negative.");
                _damage = value;
            }
        }

        public string[] Loot
        {
            get => _loot;
            set
            {
                if (value == null || value.Length == 0)
                    throw new ArgumentException("Loot cannot be null or empty.");
                foreach (var item in value)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        throw new ArgumentException("Loot items cannot be null, empty, or whitespace.");
                }
                _loot = value;
            }
        }
    
    protected Monster(int hp, int lvl, int xp, int damage, string[] loot)
        : base(hp, lvl)
    {
        XP = xp;
        Damage = damage;
        Loot = loot;
    }

    protected Monster()
    {
    }
}
