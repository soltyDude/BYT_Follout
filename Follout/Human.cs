namespace Fallout;

public abstract class Human : Creature
{
    private string? _weapon;
        private string? _cloth;

        public string? Weapon
        {
            get => _weapon;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Weapon cannot be empty or whitespace.");
                _weapon = value;
            }
        }

        public string? Cloth
        {
            get => _cloth;
            set
            {
                if (value != null && string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cloth cannot be empty or whitespace.");
                _cloth = value;
            }
        }
        
    protected Human()
    {
        
    }

    protected Human(int hp, int lvl, string weapon, string cloth)
        : base(hp, lvl)
    {
        Weapon = weapon;
        Cloth = cloth;
    }
}
