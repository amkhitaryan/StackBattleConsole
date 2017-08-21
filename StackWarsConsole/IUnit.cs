using System.Collections.Generic;

namespace StackWarsConsole
{
    public interface IUnit
    {
        int Health      { get; set; }
        int Strength    { get; set; }
        int Armor       { get; set; } 
        int Cost        { get; set; }

        int     GetHit(int value);
        bool    IsAlive();
        string  ToString();
    }    
    
    interface ISpecialAbility
    {   
        void DoSpecialAction(IUnit unit);
        void DoSpecialAction(ref List<IUnit> unitList, IUnit unit);
    }

    interface ICanBeHealed
    {
        int MaxHP       { get; set; }
    }
}
