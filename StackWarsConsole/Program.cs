using System;


namespace StackWarsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            UI userInterface = new UI();
            userInterface.Menu();

            Engine Engine = Engine.GetInstance();

            Army Alpha = new Army("Captain Morgan");
            Alpha.CreateArmy();
            Alpha.ArmyToString(Alpha.CompleteArmyList);

            Army Beta = new Army("Jack Daniel");
            Beta.CreateArmy();
            Beta.ArmyToString(Beta.CompleteArmyList);

            //Engine.War(Alpha.CompleteArmyList, Beta.CompleteArmyList);

            Console.ReadKey();
        }
    }
}
