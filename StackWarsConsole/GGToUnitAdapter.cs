using System;
using SpecialUnits;

namespace StackWarsConsole
{
    [Serializable]
    class GGAdapted : IUnit
    {
        private GulyayGorod _gorod;

        public GGAdapted(GulyayGorod g)
        {
            _gorod = g;
        }
        
        public int Health
        {
            get { return _gorod.GetCurrentHealth(); }
            set { }
        }
        public int Strength { get; set; }

        public int Armor
        {
            get { return _gorod.GetDefence(); }
            set { }
        }

        public int Cost
        {
            get { return _gorod.GetCost(); }
            set { }
        }

        public int GetHit(int value)
        {
            _gorod.TakeDamage(value);
            return _gorod.GetHealth();
        }

        public bool IsAlive()
        {
            return !_gorod.IsDead;
        }
    }

    
}
