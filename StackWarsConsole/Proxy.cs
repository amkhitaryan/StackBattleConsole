using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StackWarsConsole
{
    /// <summary>
    /// Subject для прокси
    /// </summary>
    [Serializable]
    public abstract class Subject : IUnit, ISpecialAbility, ICanBeHealed
    {
        public abstract int MaxHP { get; set; }
        public abstract int Health { get; set; }
        public abstract int Strength { get; set; }
        public abstract int Armor { get; set; }
        public abstract int Cost { get; set; }
        public abstract int GetHit(int value);

        public abstract bool IsAlive();

        public abstract void DoSpecialAction(IUnit unit);

        public abstract void DoSpecialAction(ref List<IUnit> unitList, IUnit unit);


    }
    [Serializable]
    class ProxyMage : Subject
    {
        //private static string _path = "D:\\Desktop\\ProxyLog.txt";
        private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ProxyLog.txt";

        private Mage _realSubject;

        public ProxyMage(Mage realSubject)
        {
            if (realSubject != null)
                _realSubject = realSubject;
        }

        public override int MaxHP
        {
            get { return _realSubject.MaxHP; }
            set { _realSubject.MaxHP = value; }
        }
        public override int Health
        {
            get { return _realSubject.Health; }
            set { _realSubject.Health = value; }
        }

        public override int Strength
        {
            get { return _realSubject.Strength; }
            set { _realSubject.Strength = value; }
        }

        public override int Armor
        {
            get { return _realSubject.Armor; }
            set { _realSubject.Armor = value; }
        }

        public override int Cost
        {
            get { return _realSubject.Cost; }
            set { _realSubject.Cost = value; }
        }

        private static async Task WriteTxt(string path, string text, bool newLine)
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                Task writeTask = newLine
                    ? sw.WriteLineAsync(text.ToCharArray(), 0, text.Length)
                    : sw.WriteAsync(text.ToCharArray(), 0, text.Length);
                await writeTask;
            }
        }

        private async void Logging(string path, string s, bool newline)
        {
            await WriteTxt(_path, s, newline);
        }

        public override  int GetHit(int value)
        {
            Logging(_path, _realSubject.GetType().Name + "("+_realSubject.Health + ") увлекся колдовством и получил " + value + " урона",false);
            var a = _realSubject.GetHit(value);
            Logging(_path, "(" + _realSubject.Health + ").", true);
            return a;
        }

        public override bool IsAlive()
        {
            if (_realSubject.Health > 0) return true;
            Logging(_path, DateTime.Now + " " +
                           _realSubject.GetType().Name + " погиб в сражении, сражаясь за своего короля!", true);
            return false;
        }

        public override void DoSpecialAction(IUnit unit)
        {
            throw new NotImplementedException();
        }

        public override void DoSpecialAction(ref List<IUnit> unitList, IUnit unit)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            if ((rand.Next(0, 7) != 4)) return; // 1/7 шанс
            Logging(_path,
                _realSubject.GetType().Name + "(" + _realSubject.Health + ") взмахнул посохом и клонировал " + unit.GetType().Name +
                "'a(" + unit.Health + ").", true);
            _realSubject.DoSpecialAction(ref unitList, unit);
            //unitList.Add(((ICloneable)unit).Clone() as IUnit);
        }
    }
}
