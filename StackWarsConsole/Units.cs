using System;
using System.Collections.Generic;

namespace StackWarsConsole
{
    [Serializable]
    class Armored : IHeavyUnit, ICanBeHealed, ICloneable
    {
        public Armored(int maxHp=1000, int health=1000, int strength=200, int armor=50, int cost=200)
        {
            MaxHP   = maxHp;
            Health      = health;
            Strength    = strength;
            Armor       = armor;
            Cost        = cost;
        }

        public int MaxHP        { get; set; }
        public int Health       { get; set; }
        public int Strength     { get; set; }
        public int Armor        { get; set; }
        public int Cost         { get; set; }

        public int GetHit(int value)
        {
            return Health -= value;
        }
        //true если юнит жив
        public bool IsAlive()
        {
            return Health > 0;
        }

        public object Clone()
        {
            return new Armored(this.MaxHP, this.Health, this.Strength, this.Armor, this.Cost);
        }

        public void Output(bool buffOrDebuff){}

        public bool HasSword { get; set; }
        public bool HasShield { get; set; }
        public bool HasHelmet { get; set; }
        public bool HasHorse { get; set; }
    }
    [Serializable]
    class Infantry : IUnit, ICanBeHealed, ICloneable
    {
        public Infantry(int maxHp = 500, int health = 500, int strength = 150, int armor = 25, int cost = 100)
        {
            MaxHP = maxHp;
            Health = health;
            Strength = strength;
            Armor = armor;
            Cost = cost;
        }

        public int MaxHP        { get; set; }
        public int Health       { get; set; }
        public int Strength     { get; set; }
        public int Armor        { get; set; }
        public int Cost         { get; set; }

        public int GetHit(int value)
        {
            return Health -= value;
        }
        //true если юнит жив
        public bool IsAlive()
        {
            return Health > 0;
        }

        public void GiveBuff(ref IHeavyUnit unit)
        {
            {
                //Декорируем(обертываем) объект с помощью конкретных декораторов
                var tUnit = unit as ArmoredDecoratorBase;
                if (tUnit!=null)
                {
                    if (!tUnit.HasSword)
                    {
                        unit = new Swordman(unit);
                        unit.Output(true);
                    }
                    else if (!tUnit.HasShield)
                    {
                        unit = new Shieldman(unit);
                        unit.Output(true);
                    }
                    else if (!tUnit.HasHelmet)
                    {
                        unit = new Knight(unit);
                        unit.Output(true);
                    }

                    else if (!tUnit.HasHorse)
                    {
                        unit = new Horseman(unit);
                        unit.Output(true);
                    }
                }
                else
                {
                    unit = new Swordman(unit);
                    unit.Output(true);
                }
            }
        }

        public void DeBuff(ref IHeavyUnit unit)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            if(rnd.Next(5,7) != 5) return; //50% шанс
            // Снимаем слой обертки, если он есть
            var tUnit = unit as ArmoredDecoratorBase;
            if (tUnit != null)
            {
                if (tUnit.HasHorse)
                {
                    unit.Output(false);
                    unit = tUnit._unit;
                }
                else if (tUnit.HasHelmet)
                {
                    unit.Output(false);
                    unit = tUnit._unit;
                }
                else if (tUnit.HasShield)
                {
                    unit.Output(false);
                    unit = tUnit._unit;
                }
                else if (tUnit.HasSword)
                {
                    unit.Output(false);
                    unit = new Armored(1000, unit.Health, tUnit._unit.Strength, unit.Armor, unit.Cost);
                }
            }
        }

        public object Clone()
        {
            return new Infantry(this.MaxHP, this.Health, this.Strength, this.Armor, this.Cost);
        }
    }
    [Serializable]
    class Archer : IUnit, ISpecialAbility, ICanBeHealed, ICloneable
    {
        public Archer(int maxHp = 300, int health = 300, int strength = 200, int armor = 15, int cost = 75)
        {
            MaxHP = maxHp;
            Health = health;
            Strength = strength;
            Armor = armor;
            Cost = cost;
        }

        readonly Random _randomValue = new Random();
        public int MaxHP        { get; set; }
        public int Health       { get; set; }
        public int Strength     { get; set; }
        public int Armor        { get; set; }
        public int Cost         { get; set; }

        public int GetHit(int value)
        {
            return Health -= value;
        }

        //true если юнит жив
        public bool IsAlive()
        {
            return Health > 0;
        }

        public void DoSpecialAction(IUnit unit)
        {
            unit.GetHit(Strength - unit.Armor + _randomValue.Next(-20, 20));
        }
        public void DoSpecialAction(ref List<IUnit> unitList, IUnit unit)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return new Archer(this.MaxHP, this.Health, this.Strength, this.Armor, this.Cost);
        }
    }
    [Serializable]
    class Cleric : IUnit, ICanBeHealed, ISpecialAbility, ICloneable, IObservable
    {
        public Cleric(int maxHp = 250, int health = 250, int strength = 150, int armor = 10, int cost = 50)
        {
            MaxHP = maxHp;
            Health = health;
            Strength = strength;
            Armor = armor;
            Cost = cost;
        }

        private List<IObserver> _observers = new List<IObserver>();

        public int MaxHP        { get; set; }
        public int Health       { get; set; }
        public int Strength     { get; set; }
        public int Armor        { get; set; }
        public int Cost         { get; set; }

        public int GetHit(int value)
        {
            return Health -= value;
        }
        //true если юнит жив
        public bool IsAlive()
        {
            if (Health > 0) return true;
            NotifyObservers();
            return false;
        }
        // Heal
        public void DoSpecialAction(IUnit unit)
        {
            unit.Health += Convert.ToInt32((this.Health - this.Health * 0.9) + (unit.Health - unit.Health * 0.7));
        }

        public void DoSpecialAction(ref List<IUnit> unitList, IUnit unit)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            return new Cleric(this.MaxHP, this.Health, this.Strength, this.Armor, this.Cost);
        }

        public void AddObserver(IObserver obs)
        {
            _observers.Add(obs);
        }

        public void RemoveObserver(IObserver obs)
        {
            _observers.RemoveAt(_observers.IndexOf(obs));
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
    [Serializable]
    class Mage : IUnit, ICanBeHealed, ISpecialAbility
    {
        public Mage(int maxHp=300, int health=300, int strength=125, int armor=15, int cost=75)
        {
            MaxHP       = maxHp;
            Health      = health;
            Strength    = strength;
            Armor       = armor;
            Cost        = cost;
        }

        public int MaxHP        { get; set; }
        public int Health       { get; set; }
        public int Strength     { get; set; }
        public int Armor        { get; set; }
        public int Cost         { get; set; }

        public int GetHit(int value)
        {
            return Health -= value;
        }
        //true если юнит жив
        public bool IsAlive()
        {
            return Health > 0;
        }

        public void DoSpecialAction(ref List<IUnit> unitList, IUnit unit)
        {
            {
                unitList.Add(((ICloneable)unit).Clone() as IUnit);
            }
        }
        public void DoSpecialAction(IUnit unit)
        {
            throw new NotImplementedException();
        }
    }
}
