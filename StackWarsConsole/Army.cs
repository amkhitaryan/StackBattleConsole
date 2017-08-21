using System;
using System.Collections.Generic;

namespace StackWarsConsole
{
    class Army
    {
        public Army(string armyName)
        {
            Name = armyName;
        }
        public string Name { get; }

        public List<IUnit> CompleteArmyList = new List<IUnit>();

        /// <summary>
        /// Для сохранения состояния списка юнитов
        /// </summary>
        /// <param name="stateToChange">Сохраняемый список</param>
        /// <returns>Независимую копию списка</returns>
        //public List<IUnit> ChangeState(List<IUnit> stateToChange)
        //{
        //    return DeepClone.DoDeepClone(stateToChange);
        //}
        
        public void CreateArmy()
        {

            string[] unitTypeStrings = {"Armored", "Infantry", "Mage", "Archer", "Cleric", "GulyayGorod"};
            int      totalValue = 0;         //Значение, в котором храним текущую ценность армии
            Random   rnd = new Random(DateTime.Now.Millisecond);
            while    (totalValue < 1000)
            {
                // Случайным образом выбираем класс юнита
                string randomUnit = unitTypeStrings[rnd.Next(0, unitTypeStrings.Length)];

                ICaptain CaptainMorgan;          //Фабрика : Капитан Морган умеет давать жизнь войнам(GiveLife)
                switch (randomUnit)
                {
                    case "Armored": 
                        CaptainMorgan = new ArmoredCaptain();           //Создали капитана нового формата   
                        IUnit armoredUnit = CaptainMorgan.GiveLife();   // Фабричный метод GiveLife()
                        CompleteArmyList.Add(armoredUnit);
                        totalValue += armoredUnit.Cost;
                        break;
                    case "Infantry":
                        CaptainMorgan = new InfantryCaptain();
                        IUnit infantryUnit = CaptainMorgan.GiveLife();
                        CompleteArmyList.Add(infantryUnit);
                        totalValue += infantryUnit.Cost;
                        break;
                    case "Archer":
                        CaptainMorgan = new ArcherCaptain();
                        IUnit archerUnit = CaptainMorgan.GiveLife();
                        CompleteArmyList.Add(archerUnit);
                        totalValue += archerUnit.Cost;
                        break;
                    case "Cleric":
                        CaptainMorgan = new ClericCaptain();
                        IUnit clericUnit = CaptainMorgan.GiveLife();
                        CompleteArmyList.Add(clericUnit);
                        totalValue += clericUnit.Cost;
                        break;
                    case "Mage":
                        CaptainMorgan = new MageCaptain();
                        IUnit mageUnit = CaptainMorgan.GiveLife();
                        CompleteArmyList.Add(mageUnit);
                        totalValue += mageUnit.Cost;
                        break;
                    case "GulyayGorod":
                        CaptainMorgan =new GulyayGorodCaptain();
                        IUnit gulyayGorod = CaptainMorgan.GiveLife();
                        CompleteArmyList.Add((gulyayGorod));
                        totalValue += gulyayGorod.Cost;
                        break;
                    default:
                        Console.WriteLine("No Unit Selected");
                        break;
                }
            }
            Console.WriteLine("Армия " + "\"" + Name + "'a\"" + " готова к бою !\n" );
        }
        
        //Представить армию в виде строки
        public void ArmyToString(List<IUnit> list)
        {
            Console.WriteLine("Армия \"" + Name + "'a\"" + " (" + list.Count + ")" + ":" +
                              '\n' + new string('_',22) + '\n');
            var i = 0;
            foreach (IUnit unit in list)
            {
                Console.Write(" " +i+ "." + unit.GetType().Name);
                int index = 18 - (unit.GetType().Name.Length + i.ToString().Length); 
                while (index != 0) {Console.Write(' '); index--;}
                Console.Write("HP: (" + unit.Health + ")");
                int index2 = 10 - unit.Health.ToString().Length;
                while (index2 !=0 ) { Console.Write(' '); index2--;}
                Console.Write("STR: (" + unit.Strength + ")");
                int index3 = 10 - unit.Strength.ToString().Length;
                while (index3 != 0) { Console.Write(' '); index3--; }
                Console.Write("AR: (" + unit.Armor + ")");
                int index4 = 10 - unit.Armor.ToString().Length;
                while (index4 != 0) { Console.Write(' '); index4--; }
                Console.WriteLine("COST: (" + unit.Cost + ")");
                i++;

            }
            Console.WriteLine(new string('_', 22));
            Console.WriteLine();
        }
    }
}
