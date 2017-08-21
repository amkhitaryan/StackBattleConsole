using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StackWarsConsole
{
    class UI
    {
        List<IObserver> _obs = new List<IObserver>()
            {
                new BeepObserver(),
                new BlinkObserver(),
                new DeathLogObserver()
            };

        private void Subscribe(List<IUnit> units)
        {
            foreach (var unit in units)
            {
                var clericUnit = unit as Cleric;
                if (clericUnit == null) continue;
                foreach (var observer in _obs)
                {
                    clericUnit.AddObserver(observer);
                }
            }
        }

        Invoker _invoker = new Invoker();

        private int _combatMode = 0;

        public void Menu()
        {
            Engine gameEngine = null;
            Army captainMorgan = null, jackDaniel = null;
            int key;

            Console.WriteLine("         Меню:       ");
            Console.WriteLine("1.  Запустить игру");
            Console.WriteLine("2.  Создать армию Captain Morgan");
            Console.WriteLine("3.  Показать состав армии Captain Morgan");
            Console.WriteLine("4.  Создать армию Jack Daniel");
            Console.WriteLine("5.  Показать состав армии Jack Daniel");
            Console.WriteLine("6.  Сделать ход");
            Console.WriteLine("7.  Быстрый бой");
            Console.WriteLine("8.  Undo");
            Console.WriteLine("9.  Redo");
            Console.WriteLine("10. Выбрать стратегию боя 1 на 1");
            Console.WriteLine("11. Выбрать стратегию боя 3 на 3");
            Console.WriteLine("12. Выбрать стратегию боя стенка на стенку");
            Console.WriteLine("13. Открыть Proxy лог файл");
            Console.WriteLine("14. Открыть Observer лог файл");
            Console.WriteLine("15. Показать меню");
            Console.WriteLine("16. Выход из игры\n");
            Console.Write("#");
            while (true)
            {
                try
                {
                    key = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    continue;
                }

                switch (key)
                {
                    case 1:

                        if (gameEngine == null)
                        {
                            gameEngine = Engine.GetInstance();
                        }
                        else
                            Console.WriteLine("Игра уже загружена...");
                        break;
                    case 2:
                        if (captainMorgan == null)
                        {
                            captainMorgan = new Army("Captain Morgan");
                            captainMorgan.CreateArmy();
                            Subscribe(captainMorgan.CompleteArmyList);
                        }
                        else
                        {
                            Console.WriteLine("Армия Captain Morgan была ранее создана...");
                        }
                        break;
                    case 3:
                        if (captainMorgan != null)
                        {
                            captainMorgan.ArmyToString(captainMorgan.CompleteArmyList);
                        }
                        else
                        {
                            Console.WriteLine("Необходимо создать армию Cpt. Morgran прежде чем отобразить ее");
                        }
                        break;
                    case 4:
                        if (jackDaniel == null)
                        {
                            jackDaniel = new Army("Jack Daniel");
                            jackDaniel.CreateArmy();
                            Subscribe(jackDaniel.CompleteArmyList);
                        }
                        else
                        {
                            Console.WriteLine("Армия Jack Daniel была ранее создана...");
                        }
                        break;
                    case 5:
                        if (jackDaniel != null)
                        {
                            jackDaniel.ArmyToString(jackDaniel.CompleteArmyList);
                        }
                        else
                        {
                            Console.WriteLine("Необходимо создать армию Jack Daniel прежде чем отобразить ее");
                        }
                        break;
                    case 6:
                        if (gameEngine != null && jackDaniel != null && captainMorgan != null)
                        {
                            //Добавляем команду
                            _invoker.AddCommand(
                                new NextTurnCommand(DeepClone.DoDeepClone(captainMorgan.CompleteArmyList),
                                    DeepClone.DoDeepClone(jackDaniel.CompleteArmyList)));
                            //Очищаем стек Redo
                            _invoker.ClearRedo();

                            gameEngine.NextTurn(captainMorgan.CompleteArmyList, jackDaniel.CompleteArmyList, _combatMode);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка. Убедитесь, что обе армии созданы и игра загружена.");
                        }
                        break;
                    case 7:
                        if (gameEngine != null && jackDaniel != null && captainMorgan != null)
                        {
                            while (captainMorgan.CompleteArmyList.Count != 0 || jackDaniel.CompleteArmyList.Count != 0)
                            {
                                gameEngine.NextTurn(captainMorgan.CompleteArmyList, jackDaniel.CompleteArmyList, _combatMode);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка. Убедитесь, что обе армии созданы и игра загружена.");
                        }
                        break;
                    case 8:
                        var a = _invoker.Undo(captainMorgan.CompleteArmyList, jackDaniel.CompleteArmyList);
                        if (a == null)
                        {
                            Console.WriteLine("Звезды не сошлись, попробуйте в другой раз..");
                            break;
                        }
                        Console.WriteLine("Звезды сошлись и время повернулось вспять..");
                        captainMorgan.CompleteArmyList = a.Item1;
                        jackDaniel.CompleteArmyList = a.Item2;
                        break;
                    case 9:
                        var b = _invoker.Redo(captainMorgan.CompleteArmyList, jackDaniel.CompleteArmyList);
                        if (b == null)
                        {
                            Console.WriteLine("Вселенная безгранична, но невозможно выйти за ее пределы..");
                            break;
                        }
                        Console.WriteLine("Дежавю? Кажется, мы здесь уже были.. ");
                        captainMorgan.CompleteArmyList = b.Item1;
                        jackDaniel.CompleteArmyList = b.Item2;
                        break;
                    case 10:
                        Console.WriteLine("Выбрана стратегия боя 1 на 1.");
                        break;
                    case 11:
                        Console.WriteLine("Выбрана стратегия боя 3 на 3.");
                        _combatMode = 1;
                        break;
                    case 12:
                        Console.WriteLine("Выбрана стратегия боя стенка на стенку.");
                        _combatMode = 2;
                        break;
                    case 13:
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ProxyLog.txt");
                        break;
                    case 14:
                        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DeathObserver.txt");
                        break;
                    case 15:
                        Console.WriteLine("         Меню:       ");
                        Console.WriteLine("1.  Запустить игру");
                        Console.WriteLine("2.  Создать армию Captain Morgan");
                        Console.WriteLine("3.  Показать состав армии Captain Morgan");
                        Console.WriteLine("4.  Создать армию Jack Daniel");
                        Console.WriteLine("5.  Показать состав армии Jack Daniel");
                        Console.WriteLine("6.  Сделать ход");
                        Console.WriteLine("7.  Битва до конца");
                        Console.WriteLine("8.  Undo");
                        Console.WriteLine("9.  Redo");
                        Console.WriteLine("10. Выбрать стратегию боя 1 на 1");
                        Console.WriteLine("11. Выбрать стратегию боя 3 на 3");
                        Console.WriteLine("12. Выбрать стратегию боя стенка на стенку");
                        Console.WriteLine("13. Открыть Proxy лог файл");
                        Console.WriteLine("14. Открыть Observer лог файл");
                        Console.WriteLine("15. Показать меню");
                        Console.WriteLine("16. Выход из игры");
                        break;
                    case 16:
                        Environment.Exit(0);
                        break;
                }
                Console.Write("#");
            }
        }
    }
}
