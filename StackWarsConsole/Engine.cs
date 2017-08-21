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

        //Для генерации случайного урона
        Random _randomValue = new Random();
        private readonly Strategy1 _s1 = new Strategy1();
        private readonly Strategy2 _s2 = new Strategy2();
        private readonly Strategy3 _s3 = new Strategy3();

        //public void Hit(List<IUnit> doHitArmyList, List<IUnit> takeHitArmyList, string hitter)
        //{
        //    _randomValue = new Random((int)DateTime.Now.Ticks);

        //    string attackArmyName = "", defenceArmyName = "";
            
        //    if (hitter == "Captain Morgan")
        //    {
        //        attackArmyName = hitter;
        //        defenceArmyName = "Jack Daniel";
        //    }
        //    else
        //    {
        //        attackArmyName = "Jack Daniel";
        //        defenceArmyName = "Captain Morgan";
        //    }
            
        //    if (doHitArmyList.Count == 0) GameOver(takeHitArmyList, defenceArmyName);
        //    if (!doHitArmyList[0].IsAlive())
        //    {
        //        Console.WriteLine("С поля боя унесли дряблое тело " + doHitArmyList[0].GetType().Name + "'а с позиции["+ 0 +"].");
        //        doHitArmyList.RemoveAt(0);
        //        if (doHitArmyList.Count == 0) GameOver(takeHitArmyList, defenceArmyName);
        //        else Console.WriteLine("Приветствуем вместо него дерзкого " + doHitArmyList[0].GetType().Name + "'а.\n");
        //    }
        //    if (takeHitArmyList.Count == 0) GameOver(doHitArmyList, defenceArmyName);
        //    if (!takeHitArmyList[0].IsAlive())
        //    {
        //        Console.WriteLine("С поля боя унесли дряблое тело " + takeHitArmyList[0].GetType().Name + "'а.");
        //        takeHitArmyList.RemoveAt(0);
        //        if (takeHitArmyList.Count == 0) GameOver(doHitArmyList, attackArmyName);
        //        else Console.WriteLine("Приветствуем вместо него дерзкого " + takeHitArmyList[0].GetType().Name + "'а.\n");
        //    }
            
            
        //    if (doHitArmyList.Count != 0 && takeHitArmyList.Count != 0 && 
        //        doHitArmyList[0].IsAlive() && takeHitArmyList[0].IsAlive())
        //    {
        //        int _randomValue = this._randomValue.Next(-20, 20);
        //        if(doHitArmyList[0].Strength <= 0) return; // Гуляй город не бьет
        //        takeHitArmyList[0].GetHit(doHitArmyList[0].Strength - takeHitArmyList[0].Armor + _randomValue);
        //        if (takeHitArmyList[0].Health < 0) takeHitArmyList[0].Health = 0;
        //        Console.WriteLine("Воин " + doHitArmyList[0].GetType().Name + "(" + doHitArmyList[0].Health + ")" +
        //                   " из армии " + attackArmyName + "'a, нанёс " + (doHitArmyList[0].Strength -
        //                   takeHitArmyList[0].Armor + _randomValue) + " урона войну " + takeHitArmyList[0].GetType().Name +
        //                   "(" + takeHitArmyList[0].Health + ") из армии " + defenceArmyName + "'a");
        //        var unit = takeHitArmyList[0] as IHeavyUnit;
        //        if (unit == null) return; //дебафф, если возможно
        //        var tmp = unit;
        //        new Infantry().DeBuff(ref tmp);
        //        takeHitArmyList[0] = tmp;
        //    }
        //}

        // Один размен ударами, если невозможно играть вернёт что-то

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

            #region Old

            ////1 удар
            //Hit(hitArmy, defArmy, "Captain Morgan");
            ////Обратка
            //Hit(defArmy, hitArmy, "Jack Daniel");

            ////Оруженосцы doHitArmyList дают бафф
            //if (hitArmy.Count > 1 && defArmy.Count != 0)
            //{
            //    for (int i = 1; i < hitArmy.Count; i++)
            //    {
            //        var infantry = hitArmy[i] as Infantry;
            //        var armored = hitArmy[i - 1] as IHeavyUnit;
            //        if (infantry != null && infantry.IsAlive())
            //        {
            //            //if(armored is Armored || armored is ArmoredDecoratorBase && armored.IsAlive())
            //            if (armored != null && armored.IsAlive())
            //            {
            //                infantry.GiveBuff(ref armored);
            //                hitArmy[i - 1] = armored;
            //            }

            //        }
            //    }
            //}
            ////Оруженосцы takeHitArmyList дают бафф
            //if (defArmy.Count > 1 && hitArmy.Count != 0)
            //{
            //    for (int i = 1; i < defArmy.Count; i++)
            //    {
            //        var infantry = defArmy[i] as Infantry;
            //        var armored = defArmy[i - 1] as IHeavyUnit;
            //        if (infantry != null && infantry.IsAlive())
            //        {
            //            if (armored != null && armored.IsAlive())
            //            //if (armored is Armored || armored is ArmoredDecoratorBase && armored.IsAlive())
            //            {
            //                //takeHitArmyList[i - 1] = 
            //                infantry.GiveBuff( ref armored);
            //                defArmy[i - 1] = armored;
            //            }
            //        }
            //    }
            //}


            //Debuff Armored'ов из doHitArmyList
            //if (doHitArmyList.Count != 0 && takeHitArmyList.Count != 0)
            //{
            //    for (var i = 0; i < doHitArmyList.Count; i++)
            //    {
            //        var unit1 = doHitArmyList[i];
            //        var unit = unit1 as IHeavyUnit;
            //        if (unit == null || !unit.IsAlive()) continue;
            //        new Infantry().DeBuff(ref unit);
            //        doHitArmyList[i] = unit;
            //        //infantry.DeBuff(unit);
            //        //Infantry infantry = new Infantry();
            //        //infantry.DeBuff(unit);
            //    }
            //}
            //Debuff Armored'ов из takeHitArmyList
            //if (takeHitArmyList.Count != 0 && doHitArmyList.Count != 0)
            //{
            //    for (var i = 0; i < takeHitArmyList.Count; i++)
            //    {
            //        var unit1 = takeHitArmyList[i];
            //        var unit = unit1 as IHeavyUnit;
            //        if (unit == null || !unit.IsAlive()) continue;
            //        new Infantry().DeBuff(ref unit);
            //        takeHitArmyList[i] = unit;
            //    }
            //}

            //            #region Archer attack
            //            //Атака лучников из doHitArmyList
            //            if (hitArmy.Count > 1 && defArmy.Count != 0)
            //            {
            //                for (int i = 1; i < hitArmy.Count; i++)
            //                {
            //                    if (hitArmy[i].IsAlive())
            //                    {
            //                        var specialAbility = hitArmy[i] as ISpecialAbility;
            //                        if (hitArmy[i].GetType().Name == "Archer")
            //                        {
            //                            int concreteUnit = _randomValue.Next(0, defArmy.Count);
            //                            int randomDamage = _randomValue.Next(-20, 20);
            //                            if (defArmy[concreteUnit].IsAlive())
            //                            {
            //                                if (specialAbility != null)
            //                                    specialAbility.DoSpecialAction(defArmy[concreteUnit]); //Использование метода RangeAttack интерфейса ISpecialAbility
            //                                if (defArmy[concreteUnit].Health < 0) defArmy[concreteUnit].Health = 0;
            //                                Console.WriteLine(new string('_', 50));
            //                                Console.WriteLine(
            //                                    "\nСтрелы лучников из армии Captain'a Morgan'a направлены точно в глаз дерзким войнам армии Jack'a...\n");
            //                                Console.WriteLine("Captain Morgan : \"Лучники, к бою ! \"");
            //                                Console.WriteLine("Воин " + hitArmy[i].GetType().Name + "(" +
            //                                                  hitArmy[i].Health + ")" +
            //                                                  " из армии Captain Morgan нанёс " +
            //                                                  (hitArmy[i].Strength - defArmy[concreteUnit].Armor +
            //                                                   randomDamage) +
            //                                                  " урона войну " + defArmy[concreteUnit].GetType().Name +
            //                                                  "(" + defArmy[concreteUnit].Health +
            //                                                  ") из армии Jack Daniel.");
            //                                Console.WriteLine("В яблочко! Лучники из первой армии отстрелялись ...\n");
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine("С поля боя унесли дряблое тело " +
            //                                                  defArmy[concreteUnit].GetType().Name + "'а.");
            //                                defArmy.RemoveAt(concreteUnit);
            //                                if (defArmy.Count == 0) GameOver(hitArmy);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name + "'а.");
            //                        hitArmy.RemoveAt(i);
            //                        if (hitArmy.Count == 0) GameOver(defArmy);
            //                    }
            //                }
            //            }

            //            //Обратка лучников
            //            if (defArmy.Count > 1 && hitArmy.Count != 0)
            //            {
            //                for (int i = 1; i < defArmy.Count; i++)
            //                {
            //                    if (defArmy[i].IsAlive()) 
            //                    {
            //                        var specialAbility = defArmy[i] as ISpecialAbility;
            //                        if (defArmy[i].GetType().Name == "Archer")
            //                        {
            //                            int concreteUnit = _randomValue.Next(0, hitArmy.Count);
            //                            int randomDamage = _randomValue.Next(-20, 20);
            //                            if (hitArmy[concreteUnit].IsAlive())
            //                            {
            //                                if (specialAbility != null)
            //                                    specialAbility
            //                                        .DoSpecialAction(
            //                                            hitArmy[
            //                                                concreteUnit]); //Использование метода RangeAttack интерфейса ISpecialAbility
            //                                if (hitArmy[concreteUnit].Health < 0) hitArmy[concreteUnit].Health = 0;
            //                                Console.WriteLine(
            //                                    "\nЛучники из армии Jack'a Daniel'a уже натянули тетиву и ждут команды своего доблестного капитана чтобы обрушить град из стрел на врага !\n");
            //                                Console.WriteLine("Jack Daniel : \"Лучники, к бою ! \"");
            //                                Console.WriteLine("Воин " + defArmy[i].GetType().Name + "(" +
            //                                                  defArmy[i].Health + ")" +
            //                                                  " из армии Jack Daniel нанёс " +
            //                                                  (defArmy[i].Strength - hitArmy[concreteUnit].Armor +
            //                                                   randomDamage) +
            //                                                  " урона войну " + hitArmy[concreteUnit].GetType().Name +
            //                                                  "(" + hitArmy[concreteUnit].Health +
            //                                                  ") из армии Captain Morgan.");
            //                                Console.WriteLine("В яблочко! Лучники из второй армии отстрелялись ...\n");
            //                                Console.WriteLine(new string('_', 50));
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine("С поля боя унесли дряблое тело " +
            //                                                  hitArmy[concreteUnit].GetType().Name + "'а.");
            //                                hitArmy.RemoveAt(concreteUnit);
            //                                if (hitArmy.Count == 0) GameOver(defArmy);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name + "'а.");
            //                        defArmy.RemoveAt(i);
            //                        if (defArmy.Count == 0) GameOver(hitArmy);
            //                    }
            //                }
            //            }
            //            #endregion

            //            #region Cleric attack
            //            //Выход клериков из doHitArmyList
            //            if (hitArmy.Count > 1 && defArmy.Count != 0)
            //            {
            //                for (int i = 1; i < hitArmy.Count; i++)
            //                {
            //                    if (hitArmy[i].IsAlive())
            //                    {
            //                        var healer = hitArmy[i] as ISpecialAbility;
            //                        int healMe;
            //                        // Если есть SpecialAbility, если Cleric - лечим
            //                        if (healer != null && healer.GetType().Name == "Cleric")
            //                        {
            //                            if (i != hitArmy.Count - 1) // Если не на последней позиции
            //                            {
            //                                healMe = _randomValue.Next(i - 1, i + 2);
            //                                var youCanHealMe = hitArmy[healMe] as ICanBeHealed;
            //                                if (hitArmy[healMe].IsAlive())
            //                                {
            //                                    if (youCanHealMe != null &&
            //                                        hitArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
            //                                    {
            //                                        int prevHPvalue = hitArmy[healMe].Health;
            //                                        healer.DoSpecialAction(hitArmy[healMe]); // Подлечили юнита
            //                                        if (hitArmy[healMe].Health > youCanHealMe.MaxHP)
            //                                            hitArmy[healMe].Health = youCanHealMe.MaxHP;
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "\nЦелители из армии Captain'a Morgan'a готовы залатать раны своих союзников...\n");
            //                                        Console.WriteLine("Captain Morgan : \"Целители, оказать помощь союзникам ! \"");
            //                                        Console.WriteLine("Воин " + hitArmy[i].GetType().Name + "(" +
            //                                                          hitArmy[i].Health + ")" +
            //                                                          " из армии Captain Morgan исцелил " +
            //                                                          hitArmy[healMe].GetType().Name +
            //                                                          "'a на  " + (hitArmy[healMe].Health - prevHPvalue) +
            //                                                          "HP");
            //                                        Console.WriteLine("Подлеченные войны вновь готовы сражаться !\n");
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[healMe].GetType().Name + "'а.");
            //                                    hitArmy.RemoveAt(healMe);
            //                                    if (hitArmy.Count == 0) GameOver(defArmy);
            //                                }
            //                            }
            //                            if (i == hitArmy.Count - 1) // Если находится в конце армии
            //                            {
            //                                healMe = _randomValue.Next(i - 1, i + 1);
            //                                var youCanHealMe = hitArmy[healMe] as ICanBeHealed;
            //                                if (hitArmy[healMe].IsAlive())
            //                                {
            //                                    if (youCanHealMe != null &&
            //                                        hitArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
            //                                    {
            //                                        int prevHPvalue = hitArmy[healMe].Health;
            //                                        healer.DoSpecialAction(hitArmy[healMe]); // Подлечили юнита
            //                                        if (hitArmy[healMe].Health > youCanHealMe.MaxHP)
            //                                            hitArmy[healMe].Health = youCanHealMe.MaxHP;
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "\nЦелители из армии Captain'a Morgan'a готовы залатать раны своих союзников...\n");
            //                                        Console.WriteLine("Captain Morgan : \"Целители, оказать помощь союзникам ! \"");
            //                                        Console.WriteLine("Воин " + hitArmy[i].GetType().Name + "(" +
            //                                                          hitArmy[i].Health + ")" +
            //                                                          " из армии Captain Morgan исцелил " +
            //                                                          hitArmy[healMe].GetType().Name +
            //                                                          "'a на  " + (hitArmy[healMe].Health - prevHPvalue) +
            //                                                          "HP");
            //                                        Console.WriteLine("Подлеченные войны вновь готовы сражаться !\n");
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine("С поля боя унесли дряблое тело " +
            //                                                      hitArmy[healMe].GetType().Name + "'а.");
            //                                    hitArmy.RemoveAt(healMe);
            //                                    if (hitArmy.Count == 0) GameOver(defArmy);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name + "'а.");
            //                        hitArmy.RemoveAt(i);
            //                        if (hitArmy.Count == 0) GameOver(defArmy);
            //                    }
            //                }
            //            }

            //            // Выход клериков из takeHitArmyList
            //            if (defArmy.Count > 1 && hitArmy.Count != 0)
            //            {
            //                for (int i = 1; i < defArmy.Count; i++)
            //                {
            //                    if (defArmy[i].IsAlive())
            //                    {
            //                        var healer = defArmy[i] as ISpecialAbility;
            //                        int healMe;
            //                        // Если есть SpecialAbility, если Cleric - лечим
            //                        if (healer != null && healer.GetType().Name == "Cleric")
            //                        {
            //                            if (i != defArmy.Count - 1) // Если не на последней позиции
            //                            {
            //                                healMe = _randomValue.Next(i - 1, i + 2);
            //                                var youCanHealMe = defArmy[healMe] as ICanBeHealed;
            //                                if (defArmy[healMe].IsAlive())
            //                                {
            //                                    if (youCanHealMe != null &&
            //                                        defArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
            //                                    {
            //                                        int prevHPvalue = defArmy[healMe].Health;
            //                                        healer.DoSpecialAction(defArmy[healMe]); // Подлечили юнита
            //                                        if (defArmy[healMe].Health > youCanHealMe.MaxHP)
            //                                            defArmy[healMe].Health = youCanHealMe.MaxHP;
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "\nЛекари из армии Jack'a Daniel'a готовы оказать первую помощь своим союзникам...\n");
            //                                        Console.WriteLine("Jack Daniel : \"Лекари, враг отвлёкся, оказать помощь войнам ! \"");
            //                                        Console.WriteLine("Воин " + defArmy[i].GetType().Name + "(" +
            //                                                          defArmy[i].Health + ")" +
            //                                                          " из армии Captain Morgan исцелил " +
            //                                                          defArmy[healMe].GetType().Name +
            //                                                          "'a на  " + (defArmy[healMe].Health - prevHPvalue) +
            //                                                          "HP");
            //                                        Console.WriteLine("Фух, успели, а теперь снова в бой !\n");
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[healMe].GetType().Name + "'а.");
            //                                    defArmy.RemoveAt(healMe);
            //                                    if (defArmy.Count == 0) GameOver(hitArmy);
            //                                }
            //                            }
            //                            if (i == defArmy.Count - 1) // Если находится в конце армии
            //                            {
            //                                healMe = _randomValue.Next(i - 1, i + 1);
            //                                var youCanHealMe = defArmy[healMe] as ICanBeHealed;
            //                                if (defArmy[healMe].IsAlive())
            //                                {
            //                                    if (youCanHealMe != null &&
            //                                        defArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
            //                                    {
            //                                        int prevHPvalue = defArmy[healMe].Health;
            //                                        healer.DoSpecialAction(defArmy[healMe]); // Подлечили юнита
            //                                        if (defArmy[healMe].Health > youCanHealMe.MaxHP)
            //                                            defArmy[healMe].Health = youCanHealMe.MaxHP;
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "\nЛекари из армии Jack'a Daniel'a готовы оказать первую помощь своим союзникам...\n");
            //                                        Console.WriteLine("Jack Daniel : \"Лекари, враг отвлёкся, оказать помощь войнам ! \"");
            //                                        Console.WriteLine("Воин " + defArmy[i].GetType().Name + "(" +
            //                                                          defArmy[i].Health + ")" +
            //                                                          " из армии Captain Morgan исцелил " +
            //                                                          defArmy[healMe].GetType().Name +
            //                                                          "'a на  " + (defArmy[healMe].Health - prevHPvalue) +
            //                                                          "HP");
            //                                        Console.WriteLine("Фух, успели, а теперь снова в бой !\n");
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine("С поля боя унесли дряблое тело " +
            //                                                      defArmy[healMe].GetType().Name + "'а.");
            //                                    defArmy.RemoveAt(healMe);
            //                                    if (defArmy.Count == 0) GameOver(hitArmy);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name + "'а.");
            //                        defArmy.RemoveAt(i);
            //                        if (defArmy.Count == 0) GameOver(hitArmy);
            //                    }
            //                }
            //            }
            //            #endregion

            //            #region Mage attack
            //            // SpecialAbility магов doHitArmyList
            //            if (hitArmy.Count > 1 && defArmy.Count != 0)
            //            {
            //                for (int i = 1; i < hitArmy.Count; i++)
            //                {
            //                    if (hitArmy[i].IsAlive())
            //                    {
            //                        var cloner = hitArmy[i] as ISpecialAbility;
            //                        int cloneMe;
            //                        // Если есть абилка и юнит Маг - пытаемся клонировать
            //                        if (cloner != null && hitArmy[i].GetType().Name == "Mage")
            //                        {
            //                            if (i != hitArmy.Count - 1) // Если не на последней позиции
            //                            {
            //                                if (_randomValue.Next(0, 2)  == 0)
            //                                    cloneMe = i - 1;
            //                                else cloneMe = i + 1;
            //                                if (hitArmy[cloneMe].IsAlive() && hitArmy[cloneMe] is ICloneable)
            //                                {
            //                                    int listCountBeforeSpecialAction = hitArmy.Count;
            //                                    cloner.DoSpecialAction(hitArmy, hitArmy[cloneMe]);
            //                                    if (hitArmy.Count == listCountBeforeSpecialAction)
            //                                    {
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "Маг из армии Captain Morgan не сумел применить заклинание клонирования...(");
            //                                        Console.WriteLine(new string('_', 50));
            //                                    }
            //                                    else
            //                                    {
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(hitArmy[i].GetType().Name + " клонировал " +
            //                                                          hitArmy[cloneMe].GetType().Name + "'a" +
            //                                                          "(" + hitArmy[cloneMe].Health + "). " +
            //                                                          "В строю армии Captan'a Morgan'a новый союзник...");
            //                                        Console.WriteLine(new string('_', 50));
            //                                    }
            //                                }
            //                                //else
            //                                //{
            //                                //    Console.WriteLine("С поля боя унесли дряблое тело " + doHitArmyList[cloneMe].GetType().Name + "'а.");
            //                                //    doHitArmyList.RemoveAt(cloneMe);
            //                                //    if (doHitArmyList.Count == 0) GameOver(takeHitArmyList);
            //                                //}
            //                            }
            //                            if (i == hitArmy.Count - 1) // Если маг в конце, клонируем того,кто перед ним
            //                            {
            //                                cloneMe = i - 1;
            //                                if (hitArmy[cloneMe].IsAlive() && hitArmy[cloneMe] is ICloneable)
            //                                {
            //                                    int listCountBeforeSpecialAction = hitArmy.Count;
            //                                    cloner.DoSpecialAction(hitArmy, hitArmy[cloneMe]);
            //                                    if (hitArmy.Count == listCountBeforeSpecialAction)
            //                                    {
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "Маг из армии Captain Morgan не сумел применить заклинание клонирования...(");
            //                                        Console.WriteLine(new string('_', 50));
            //                                    }
            //                                    else
            //                                    {
            //                                        Console.WriteLine(hitArmy[i].GetType().Name + " клонировал " +
            //                                                          hitArmy[cloneMe].GetType().Name + "'a" +
            //                                                          "(" + hitArmy[cloneMe].Health + "). " +
            //                                                          "В строю армии Captan'a Morgan'a новый союзник...");
            //                                    }
            //                                }
            //                                //else
            //                                //{
            //                                //    Console.WriteLine("С поля боя унесли дряблое тело " + doHitArmyList[cloneMe].GetType().Name + "'а.");
            //                                //    doHitArmyList.RemoveAt(cloneMe);
            //                                //    if (doHitArmyList.Count == 0) GameOver(takeHitArmyList);
            //                                //}
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name + "'а.");
            //                        hitArmy.RemoveAt(i);
            //                        if (hitArmy.Count == 0) GameOver(defArmy);
            //                    }
            //                }
            //            }
            //            // SpecialAbility магов takeHitArmyList
            //            if (defArmy.Count > 1 && hitArmy.Count != 0)
            //            {
            //                for (int i = 1; i < defArmy.Count; i++)
            //                {
            //                    if (defArmy[i].IsAlive())
            //                    {
            //                        var cloner = defArmy[i] as ISpecialAbility;
            //                        int cloneMe;
            //                        // Если есть абилка и юнит Маг - пытаемся клонировать
            //                        if (cloner != null && defArmy[i].GetType().Name == "Mage")
            //                        {
            //                            if (i != defArmy.Count - 1) // Если не на последней позиции
            //                            {
            //                                if (_randomValue.Next(0, 2) == 0)
            //                                    cloneMe = i - 1;
            //                                else cloneMe = i + 1;
            //                                if (defArmy[cloneMe].IsAlive() && defArmy[cloneMe] is ICloneable)
            //                                {
            //                                    int listCountBeforeSpecialAction = defArmy.Count;
            //                                    cloner.DoSpecialAction(defArmy, defArmy[cloneMe]);
            //                                    if (defArmy.Count == listCountBeforeSpecialAction)
            //                                    {
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "Маг из армии Jack Daniel не сумел применить заклинание клонирования...(");
            //                                        Console.WriteLine(new string('_', 50));
            //                                    }
            //                                    else
            //                                    {
            //                                        Console.WriteLine(defArmy[i].GetType().Name + " клонировал " +
            //                                                          defArmy[cloneMe].GetType().Name + "'a" +
            //                                                          "(" + defArmy[cloneMe].Health + "). " +
            //                                                          "Армия Jack'a Daniel'a приветствует нового союзника...");
            //                                    }
            //                                }
            //                                //else
            //                                //{
            //                                //    Console.WriteLine("С поля боя унесли дряблое тело " + takeHitArmyList[cloneMe].GetType().Name + "'а.");
            //                                //    takeHitArmyList.RemoveAt(cloneMe);
            //                                //    if (takeHitArmyList.Count == 0) GameOver(doHitArmyList);
            //                                //}
            //                            }
            //                            if (i == defArmy.Count - 1) // Если маг в конце, клонируем того,кто перед ним
            //                            {
            //                                cloneMe = i - 1;
            //                                if (defArmy[cloneMe].IsAlive() && defArmy[cloneMe] is ICloneable)
            //                                {
            //                                    int listCountBeforeSpecialAction = defArmy.Count;
            //                                    cloner.DoSpecialAction(defArmy, defArmy[cloneMe]);
            //                                    if (defArmy.Count == listCountBeforeSpecialAction)
            //                                    {
            //                                        Console.WriteLine(new string('_', 50));
            //                                        Console.WriteLine(
            //                                            "Маг из армии Jack Daniel не сумел применить заклинание клонирования...(");
            //                                        Console.WriteLine(new string('_', 50));
            //                                    }
            //                                    else
            //                                    {
            //                                        Console.WriteLine(defArmy[i].GetType().Name + " клонировал " +
            //                                                          defArmy[cloneMe].GetType().Name + "'a" +
            //                                                          "(" + defArmy[cloneMe].Health + "). " +
            //                                                          "Армия Jack'a Daniel'a приветствует нового союзника...");
            //                                    }
            //                                }
            //                                //else
            //                                //{
            //                                //    Console.WriteLine("С поля боя унесли дряблое тело " + takeHitArmyList[cloneMe].GetType().Name + "'а.");
            //                                //    takeHitArmyList.RemoveAt(cloneMe);
            //                                //    if (takeHitArmyList.Count == 0) GameOver(doHitArmyList);
            //                                //}
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name + "'а.");
            //                        hitArmy.RemoveAt(i);
            //                        if (hitArmy.Count == 0) GameOver(defArmy);
            //                    }
            //                }
            //            }
            //#endregion

            #endregion
        }

        // Битва до конца
        //public void War(List<IUnit> doHitArmyList, List<IUnit> takeHitArmyList)
        //{
        //    while (doHitArmyList.Count != 0 || takeHitArmyList.Count != 0)
        //    {
        //        NextTurn(doHitArmyList,takeHitArmyList);
        //    }
        //}

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
