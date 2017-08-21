using System;
using System.Collections.Generic;

namespace StackWarsConsole
{   
    //Singletone
    public class Engine
    {
        private static Engine _instance;

        private Engine()
        {
            Console.WriteLine("Game is launched...\n");
        }

        public static Engine GetInstance()
        {
            return _instance ?? (_instance = new Engine());
        }
        
        private readonly Strategy1 _s1 = new Strategy1();
        private readonly Strategy2 _s2 = new Strategy2();
        private readonly Strategy3 _s3 = new Strategy3();
        
        public void NextTurn(List<IUnit> hitArmy, List<IUnit> defArmy, int combat)
        {
            switch (combat)
            {
                case 0:
                    _s1.Combat(hitArmy, defArmy);
                    break;
                case 1:
                    _s2.Combat(hitArmy, defArmy);
                    break;
                case 2:
                    _s3.Combat(hitArmy, defArmy);
                    break;
            }
            
        }

        //Завершает игру, если в одной из армий нет юнитов
        public static void GameOver(List<IUnit> winnerList,string strWinner)
        {
            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Бой окончен. " + winnerList[0].GetType().Name + " и команда " + strWinner + "'a победили !\n" +
                              "Слава Королю !");
            Console.WriteLine(new string('_', 50));
            Console.WriteLine();
            new Army(strWinner).ArmyToString(winnerList);
            
            Console.WriteLine("Нажмите любую клавишу чтобы выйти из игры ...");

            if (Console.ReadKey(true).Key == ConsoleKey.E)
                Environment.Exit(0);
            else
                Environment.Exit(0);
        }
    }
}
