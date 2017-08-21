using System;

namespace StackWarsConsole
{
    /// <summary>
    /// Интерфейс-компонент для декоратора
    /// </summary>
    public interface IHeavyUnit : IUnit
    {
        bool HasSword { get; set; }
        bool HasShield { get; set; }
        bool HasHelmet { get; set; }
        bool HasHorse { get; set; }

        void Output(bool buffOrDebuff);
    }
    /// <summary>
    /// Общий интерфейс определяет интерфейс объектов и декоратора
    /// </summary>
    [Serializable]
    public abstract class ArmoredDecoratorBase : IHeavyUnit, ICloneable
    {
        /// <summary>
        /// Конкретный компонент функциональность которого необходимо модифицировать
        /// </summary>
        public  IHeavyUnit _unit;

        protected ArmoredDecoratorBase(IHeavyUnit unit)
        {
            if (unit != null)
            {
                _unit = unit;
            }
        }

        public virtual bool HasSword
        {
            get { return _unit.HasSword; }
            set { _unit.HasSword = value; } 
        }

        public virtual bool HasShield
        {
            get { return _unit.HasShield; }
            set { _unit.HasShield = value; }
        }

        public virtual bool HasHelmet
        {
            get { return _unit.HasHelmet; }
            set { _unit.HasHelmet = value; }
        }

        public virtual bool HasHorse
        {
            get { return _unit.HasHorse; }
            set { _unit.HasHorse = value; }
        }
        
        public virtual int Health
        {
            get { return this._unit.Health; }
            set { this._unit.Health = value; } 
        }

        public virtual int Strength
        {
            get { return this._unit.Strength; }
            set { this._unit.Strength = value; }
        }

        public virtual int Armor
        {
            get { return this._unit.Armor; }
            set { this._unit.Armor = value; }
        }

        public virtual int Cost
        {
            get { return this._unit.Cost; }
            set { this._unit.Cost = value; }
        }

        public int GetHit(int value)
        {
            return this._unit.GetHit(value);
        }

        public bool IsAlive()
        {
            return this._unit.IsAlive();
        }

        public virtual void Output(bool buffOrDebuff) { }
        public object Clone()
        {
            IHeavyUnit a = null;
            if (HasHorse)
            {
                a = new Horseman(new Armored(1000, this.Health, this.Strength - 30, this.Armor - 15, this.Cost));
            }
            else if (HasHelmet)
            {
                a = new Knight(new Armored(1000, this.Health, this.Strength, this.Armor - 15, this.Cost));
            }
            else if (HasShield)
            {
                a = new Shieldman(new Armored(1000, this.Health, this.Strength, this.Armor - 20, this.Cost));
            }
            else if (HasSword)
            {
                a = new Swordman(new Armored(1000, this.Health, this.Strength - 75, this.Armor, this.Cost));
            }

            if (a == null)
                return new Armored(1000, this.Health, this.Strength, this.Armor, this.Cost)
                {
                    HasHorse = this.HasHorse,
                    HasHelmet = this.HasHelmet,
                    HasShield = this.HasShield,
                    HasSword = this.HasSword
                };
            a.HasHorse = this.HasHorse;
            a.HasHelmet = this.HasHelmet;
            a.HasShield = this.HasShield;
            a.HasSword = this.HasSword;
            return a;
        }

    }
    [Serializable]
    public class Swordman : ArmoredDecoratorBase
    {
        public Swordman(IHeavyUnit unit) : base(unit)
        {
            base.HasSword = true;
        }

        public override int Strength
        {
            get
            {
                if(base.HasSword)
                    return _unit.Strength + 75;
                return _unit.Strength;
            }
            set { }
        }
        

        public override void Output(bool buffOrDebuff)
        {
            if (buffOrDebuff)
                Console.WriteLine("Armored(" + this.Health +
                                  ") получил Меч Короля Артура, это увеличит силу его атаки(" + this.Strength + ")...");
            else
            {
                Console.WriteLine("Swordman(" + this.Health +
                                  ") выронил Меч Короля Артура, отныне его удар не так силен(" +
                                  (this.Strength - 75) + ")...");
            }
        }
    }
    [Serializable]
    public class Knight : ArmoredDecoratorBase
    {
        public Knight(IHeavyUnit unit) : base(unit)
        {
            base.HasHelmet = true;
        }

        public override bool HasSword => _unit.HasSword;

        public override bool HasShield => _unit.HasShield;

        public override int Armor
        {
            get
            {
                if(base.HasHelmet)
                    return _unit.Armor + 15;
                return _unit.Armor;
            }
        }

        public override void Output(bool buffOrDebuff)
        {
            if (buffOrDebuff)
                Console.WriteLine("Shieldman(" + this.Health + ") надел шлем, очки защиты увеличены(" +
                                  this.Armor + ")...");
            else
            {
                Console.WriteLine("Knight'у(" + this.Health + ") сбили с головы стальной шлем, уровень защиты снижен(" +
                                  (this.Armor-15) + ")...");
            }
        }
    }
    [Serializable]
    public class Shieldman : ArmoredDecoratorBase
    {
        public Shieldman(IHeavyUnit unit) : base(unit)
        {
            
            base.HasShield = true;
        }

        public override bool HasSword => _unit.HasSword;

        public override int Armor
        {
            get
            {
                if(base.HasShield)
                    return _unit.Armor + 20;
                return _unit.Armor;
            }
        }

        public override void Output(bool buffOrDebuff)
        {
            if (buffOrDebuff)
                Console.WriteLine("Swordman(" + this.Health + ") подобрал щит, очки защиты увеличены(" +
                                  this.Armor + ")...");
            else
            {
                Console.WriteLine("Shieldman(" + this.Health +
                                  ") устал и решил продолжать бой без щита, несмотря на то, что защищен хуже чем прежде(" +
                                  (this.Armor - 20) + ")...");
            }
        }
    }
    [Serializable]
    public class Horseman : ArmoredDecoratorBase
    {
        public Horseman(IHeavyUnit unit) : base(unit)
        {
            base.HasHorse = true;
        }

        public override bool HasHelmet => _unit.HasHelmet;

        public override bool HasSword => _unit.HasSword;

        public override bool HasShield => _unit.HasShield;

        public override int Strength
        {
            get
            {
                if (base.HasHorse)
                    return _unit.Strength + 30;
                return _unit.Strength;
            }
        }
        public override int Armor
        {
            get
            {
                if (base.HasHorse)
                    return _unit.Armor + 15;
                return _unit.Armor;
            }
        }

        public override void Output(bool buffOrDebuff)
        {
            if (buffOrDebuff)
                Console.WriteLine("Knight(" + this.Health + ") скачет верхом, наносимый урон повышен(" +
                                  this.Strength + "), очки защиты увеличены(" + this.Armor + ")...");
            else
            {
                Console.WriteLine("Horseman(" + this.Health +
                                  ") выпустил из-под ног верного коня, его удар ослаб(" +
                                  (this.Strength - 30) + "), а защита(" + (this.Armor - 15) + ") не так надежна как ранее......");
            }
        }
    }
}
