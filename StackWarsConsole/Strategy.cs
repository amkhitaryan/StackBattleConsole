using System;
using System.Collections.Generic;

namespace StackWarsConsole
{
    interface IStrategy
    {
        void Combat(List<IUnit> hitArmy, List<IUnit> defArmy);
    }

    /// <summary>
    /// Стратегия боя 1 на 1
    /// </summary>
    class Strategy1 : IStrategy
    {
        public void Combat(List<IUnit> hitArmy, List<IUnit> defArmy)
        {
            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Strategy 1x1");
            Random _randomValue = new Random((int) DateTime.Now.Ticks);

            //1 удар
            Hit(hitArmy, defArmy, "Captain Morgan");
            //Обратка
            Hit(defArmy, hitArmy, "Jack Daniel");

            //Далее, начиная со второй позиции, юниты совершают специальные действия
            //Способности 1-й армии
            Console.WriteLine("*Captain Morgan's army Special Abilities*");
            for (int j = 1; j < hitArmy.Count; j++)
            {
                if (j < hitArmy.Count && hitArmy[j].IsAlive())
                {
                    #region Infantry

                    //Если это оруженосец, одевает Armored, если есть рядом
                    var infantry = hitArmy[j] as Infantry;
                    if (infantry != null)
                    {
                        var armored = hitArmy[j - 1] as IHeavyUnit;
                        if (armored != null && armored.IsAlive() && !(armored is Horseman))
                        {
                            _randomValue = new Random((int)DateTime.Now.Ticks);
                            if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                            Console.WriteLine(hitArmy[j].GetType().Name + " с позиции [" + j +
                                              "] из армии Captain Morgan'a экипировал " +
                                              hitArmy[j - 1].GetType().Name + " на позиции [" + (j - 1) + "]");
                            infantry.GiveBuff(ref armored);
                            hitArmy[j - 1] = armored;
                        }
                        else if(!(armored is Horseman))
                        {
                            if (j == hitArmy.Count - 1) break; //Оруженосец в конце строя, за ним никого нет - бездействует
                            armored = hitArmy[j + 1] as IHeavyUnit;
                            if (armored != null && armored.IsAlive())
                            {
                                _randomValue = new Random((int)DateTime.Now.Ticks);
                                if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                                Console.WriteLine(hitArmy[j].GetType().Name + " с позиции [" + j +
                                              "] из армии Captain Morga'a экипировал " +
                                              hitArmy[j + 1].GetType().Name + " на позиции [" + (j + 1) + "]");
                                infantry.GiveBuff(ref armored);
                                hitArmy[j + 1] = armored;
                            }
                        }
                    }
                    #endregion

                    #region Archer

                    //Если это лучник, стреляет во врагов
                    else if (hitArmy[j] is Archer)
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        var archer = (Archer) hitArmy[j];
                        if (archer.IsAlive())
                        {
                            int concreteUnit = _randomValue.Next(0, defArmy.Count);
                            if (defArmy[concreteUnit].IsAlive())
                            {
                                var hp = defArmy[concreteUnit].Health;
                                archer.DoSpecialAction(defArmy[concreteUnit]);
                                //Использование метода RangeAttack интерфейса ISpecialAbility
                                if (defArmy[concreteUnit].Health < 0) defArmy[concreteUnit].Health = 0;
                                Console.WriteLine(hitArmy[j].GetType().Name + "(" +
                                                  hitArmy[j].Health + ")" +
                                                  " c позиции[" + j + "]  из армии Captain Morgan нанёс " +
                                                  (hp - defArmy[concreteUnit].Health) +
                                                  " урона воину " + defArmy[concreteUnit].GetType().Name +
                                                  "(" + defArmy[concreteUnit].Health +
                                                  ") на позиции[" + concreteUnit + "] из армии Jack Daniel'a.");
                                if(!defArmy[concreteUnit].IsAlive())
                                {
                                    Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[concreteUnit].GetType().Name +
                                                      "'а с позиции[" + concreteUnit + "].");
                                    defArmy.RemoveAt(concreteUnit);
                                    if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                                }
                            }
                            else
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " +
                                                  defArmy[concreteUnit].GetType().Name + "'а с позиции[" + concreteUnit +
                                                  "].");
                                defArmy.RemoveAt(concreteUnit);
                                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                            }
                        }
                    }
                    #endregion

                    #region Cleric

                    //Если это лекарь
                    else if (hitArmy[j] is Cleric)
                    {
                        if (hitArmy[j].IsAlive())
                        {
                            if (j != hitArmy.Count - 1) // Если не на последней позиции
                            {
                                var healer = (Cleric) hitArmy[j];
                                var healMe = _randomValue.Next(j - 1, j + 2);
                                var youCanHealMe = hitArmy[healMe] as ICanBeHealed;
                                if (hitArmy[healMe].IsAlive())
                                {
                                    if (youCanHealMe != null &&
                                        hitArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                    {
                                        int prevHPvalue = hitArmy[healMe].Health;
                                        healer.DoSpecialAction(hitArmy[healMe]); // Подлечили юнита
                                        if (hitArmy[healMe].Health > youCanHealMe.MaxHP)
                                            hitArmy[healMe].Health = youCanHealMe.MaxHP;
                                        Console.WriteLine(hitArmy[j].GetType().Name + "(" +
                                                          hitArmy[j].Health + ")" +
                                                          " с позиции[" + j + "] из армии Captain Morgan'a исцелил " +
                                                          hitArmy[healMe].GetType().Name +
                                                          "'a на позиции[" + healMe + "] на  " +
                                                          (hitArmy[healMe].Health - prevHPvalue) +
                                                          "HP.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("С поля боя унесли дряблое тело " +
                                                      hitArmy[healMe].GetType().Name + "'а с позиции[" + healMe + "].");
                                    hitArmy.RemoveAt(healMe);
                                    if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                                }
                            }
                            else if (j == hitArmy.Count - 1) // Если находится в конце армии
                            {
                                var healer = (Cleric)hitArmy[j];
                                var healMe = _randomValue.Next(j - 1, j + 1);
                                var youCanHealMe = hitArmy[healMe] as ICanBeHealed;
                                if (hitArmy[healMe].IsAlive())
                                {
                                    if (youCanHealMe != null &&
                                        hitArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                    {
                                        int prevHPvalue = hitArmy[healMe].Health;
                                        healer.DoSpecialAction(hitArmy[healMe]); // Подлечили юнита
                                        if (hitArmy[healMe].Health > youCanHealMe.MaxHP)
                                            hitArmy[healMe].Health = youCanHealMe.MaxHP;
                                        Console.WriteLine(hitArmy[j].GetType().Name + "(" +
                                                          hitArmy[j].Health + ")" +
                                                          " с позиции[" + j + "] из армии Captain Morgan'a исцелил " +
                                                          hitArmy[healMe].GetType().Name +
                                                          "'a на позиции[" + healMe + "] на " +
                                                          (hitArmy[healMe].Health - prevHPvalue) +
                                                          "HP");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("С поля боя унесли дряблое тело " +
                                                      hitArmy[healMe].GetType().Name + "'а с позиции[" + healMe + "].");
                                    hitArmy.RemoveAt(healMe);
                                    if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Captain Morgan");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[j].GetType().Name +
                                              "'а с позиции[" + j + "].");
                            hitArmy.RemoveAt(j);
                            if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                        }
                        
                    }
                    #endregion

                    #region Mage
                    else if (hitArmy[j] is ProxyMage)
                    {
                        if (hitArmy[j].IsAlive())
                        {
                            var cloner = (ProxyMage) hitArmy[j];
                            // Если есть абилка и юнит Маг - пытаемся клонировать

                            int cloneMe;
                            if (j != hitArmy.Count - 1) // Если не на последней позиции
                            {
                                if (_randomValue.Next(0, 2) == 0)
                                    cloneMe = j - 1;
                                else cloneMe = j + 1;
                                if (hitArmy[cloneMe].IsAlive() && hitArmy[cloneMe] is ICloneable)
                                {
                                    int listCountBeforeSpecialAction = hitArmy.Count;
                                    cloner.DoSpecialAction(ref hitArmy, hitArmy[cloneMe]);
                                    if (hitArmy.Count != listCountBeforeSpecialAction)
                                    {
                                        Console.WriteLine(hitArmy[j].GetType().Name + " с позиции [" + j +
                                                          "] клонировал " +
                                                          hitArmy[cloneMe].GetType().Name + "'a " +
                                                          "(" + hitArmy[cloneMe].Health + ") на позиции [" +
                                                          cloneMe + "]. " +
                                                          "Армия Captain Morgan'a приветствует нового союзника...");
                                    }

                                }
                            }
                            if (j == hitArmy.Count - 1) // Если маг в конце, клонируем того,кто перед ним
                            {
                                cloneMe = j - 1;
                                if (hitArmy[cloneMe].IsAlive() && hitArmy[cloneMe] is ICloneable)
                                {
                                    int listCountBeforeSpecialAction = hitArmy.Count;
                                    cloner.DoSpecialAction(ref hitArmy, hitArmy[cloneMe]);
                                    if (hitArmy.Count != listCountBeforeSpecialAction)
                                    {
                                        Console.WriteLine(hitArmy[j].GetType().Name + " с позиции [" + j +
                                                          "] клонировал " +
                                                          hitArmy[cloneMe].GetType().Name + "'a " +
                                                          "(" + hitArmy[cloneMe].Health + ") на позиции [" +
                                                          cloneMe + "]. " +
                                                          "Армия Captain Morgan'a приветствует нового союзника...");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[j].GetType().Name +
                                              "'а с позиции[" + j + "].");
                            hitArmy.RemoveAt(j);
                            if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                        }
                    }
                    #endregion
                }
            }

            _randomValue = new Random((int) DateTime.Now.Ticks);
            //Вторая армия
            Console.WriteLine("*Jack Daniel's army Special Abilities*");
            for (int k = 1; k < defArmy.Count; k++)
            {
                #region Infantry

                //Если это оруженосец, одевает Armored, если есть рядом
                var infantry = defArmy[k] as Infantry;
                if (infantry != null)
                {
                    var armored = defArmy[k - 1] as IHeavyUnit;
                    if (armored != null && armored.IsAlive() && !(armored is Horseman))
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                        Console.WriteLine(defArmy[k].GetType().Name + " с позиции [" + k +
                                              "] из армии Jack Daniel'a экипировал " +
                                              defArmy[k - 1].GetType().Name + " на позиции [" + (k - 1) + "]");
                        infantry.GiveBuff(ref armored);
                        defArmy[k - 1] = armored;
                    }
                    else if(!(armored is Horseman))
                    {
                        if (k == defArmy.Count - 1) break; //Оруженосец в конце строя, за ним никого нет
                        armored = defArmy[k + 1] as IHeavyUnit;
                        if (armored != null && armored.IsAlive())
                        {
                            _randomValue = new Random((int)DateTime.Now.Ticks);
                            if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                            Console.WriteLine(defArmy[k].GetType().Name + " с позиции [" + k +
                                              "] из армии Jack Daniel'a экипировал " +
                                              defArmy[k + 1].GetType().Name + " на позиции [" + (k + 1) + "]");
                            infantry.GiveBuff(ref armored);
                            defArmy[k + 1] = armored;
                        }
                    }
                }
                #endregion

                #region Archer

                else if (defArmy[k] is Archer)
                {
                    var archer = (Archer) defArmy[k];
                    if (archer.IsAlive())
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        int concreteUnit = _randomValue.Next(0, hitArmy.Count);
                        if (hitArmy[concreteUnit].IsAlive())
                        {
                            var hp = hitArmy[concreteUnit].Health;
                            archer.DoSpecialAction(hitArmy[concreteUnit]);
                            //Использование метода RangeAttack интерфейса ISpecialAbility
                            if (hitArmy[concreteUnit].Health < 0) hitArmy[concreteUnit].Health = 0;
                            Console.WriteLine(defArmy[k].GetType().Name + "(" +
                                              defArmy[k].Health + ")" +
                                              "с позиции[" + k + "] из армии Jack Daniel'a нанёс " +
                                              (hp - hitArmy[concreteUnit].Health) +
                                              " урона воину " + hitArmy[concreteUnit].GetType().Name +
                                              "(" + hitArmy[concreteUnit].Health +
                                              ") на позиции[" + concreteUnit + "] из армии Captain Morgan'a.");
                            if (!hitArmy[concreteUnit].IsAlive())
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[concreteUnit].GetType().Name +
                                                  "'а с позиции[" + concreteUnit + "].");
                                hitArmy.RemoveAt(concreteUnit);
                                if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " +
                                              hitArmy[concreteUnit].GetType().Name + "'а с позиции[" + concreteUnit +
                                              "].");
                            hitArmy.RemoveAt(concreteUnit);
                            if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                        }
                    }
                }
                #endregion

                #region Cleric

                //Если это лекарь
                else if (defArmy[k] is Cleric)
                {
                    var healer = (Cleric) defArmy[k];
                    if (defArmy[k].IsAlive())
                    {
                        if (k != defArmy.Count - 1) // Если не на последней позиции
                        {
                            var healMe = _randomValue.Next(k - 1, k + 2);
                            var youCanHealMe = defArmy[healMe] as ICanBeHealed;
                            if (defArmy[healMe].IsAlive())
                            {
                                if (youCanHealMe != null &&
                                    defArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                {
                                    int prevHPvalue = defArmy[healMe].Health;
                                    healer.DoSpecialAction(defArmy[healMe]); // Подлечили юнита
                                    if (defArmy[healMe].Health > youCanHealMe.MaxHP)
                                        defArmy[healMe].Health = youCanHealMe.MaxHP;
                                    Console.WriteLine(defArmy[k].GetType().Name + "(" +
                                                      defArmy[k].Health + ")" +
                                                      "с позиции[" + k + "] из армии Jack Daniel'a исцелил " +
                                                      defArmy[healMe].GetType().Name +
                                                      "'a на позиции[" + healMe + "] на " +
                                                      (defArmy[healMe].Health - prevHPvalue) +
                                                      "HP");
                                }
                            }
                            else
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[k].GetType().Name +
                                                  "'а с позиции[" + k + "].");
                                defArmy.RemoveAt(k);
                                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                            }

                        }
                        else if (k == defArmy.Count - 1) // Если находится в конце армии
                        {
                            var healMe = _randomValue.Next(k - 1, k + 1);
                            var youCanHealMe = defArmy[healMe] as ICanBeHealed;
                            if (defArmy[healMe].IsAlive())
                            {
                                if (youCanHealMe != null &&
                                    defArmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                {
                                    int prevHPvalue = defArmy[healMe].Health;
                                    healer.DoSpecialAction(defArmy[healMe]); // Подлечили юнита
                                    if (defArmy[healMe].Health > youCanHealMe.MaxHP)
                                        defArmy[healMe].Health = youCanHealMe.MaxHP;
                                    Console.WriteLine(defArmy[k].GetType().Name + "(" +
                                                      defArmy[k].Health + ")" +
                                                      "с позиции[" + k + "] из армии Jack Daniel'a исцелил " +
                                                      defArmy[healMe].GetType().Name +
                                                      "'a на позиции[" + healMe + "] на " +
                                                      (defArmy[healMe].Health - prevHPvalue) +
                                                      "HP");
                                }
                            }
                            else
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " +
                                                  defArmy[healMe].GetType().Name + "'а с позиции[" + healMe + "].");
                                defArmy.RemoveAt(healMe);
                                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[k].GetType().Name +
                                          "'а с позиции[" + k + "].");
                        defArmy.RemoveAt(k);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    }
                }
                #endregion

                #region Mage

                else if (defArmy[k] is ProxyMage)
                {
                    if (defArmy[k].IsAlive())
                    {
                        var cloner = (ProxyMage) defArmy[k];
                        // Если есть абилка и юнит Маг - пытаемся клонировать

                        int cloneMe;
                        if (k != defArmy.Count - 1) // Если не на последней позиции
                        {
                            if (_randomValue.Next(0, 2) == 0)
                                cloneMe = k - 1;
                            else cloneMe = k + 1;
                            if (defArmy[cloneMe].IsAlive() && defArmy[cloneMe] is ICloneable)
                            {
                                int listCountBeforeSpecialAction = defArmy.Count;
                                cloner.DoSpecialAction(ref defArmy, defArmy[cloneMe]);
                                if (defArmy.Count != listCountBeforeSpecialAction)
                                {
                                    Console.WriteLine(defArmy[k].GetType().Name + "с позиции [" + k +
                                                      "] клонировал " +
                                                      defArmy[cloneMe].GetType().Name + "'a " +
                                                      "(" + defArmy[cloneMe].Health + ") на позиции [" +
                                                      cloneMe + "]. " +
                                                      "В строю армии Jack Daniel'a новый союзник...");
                                }

                            }
                        }
                        if (k == defArmy.Count - 1) // Если маг в конце, клонируем того,кто перед ним
                        {
                            cloneMe = k - 1;
                            if (defArmy[cloneMe].IsAlive() && defArmy[cloneMe] is ICloneable)
                            {
                                int listCountBeforeSpecialAction = defArmy.Count;
                                cloner.DoSpecialAction(ref defArmy, defArmy[cloneMe]);
                                if (defArmy.Count != listCountBeforeSpecialAction)
                                {
                                    Console.WriteLine(defArmy[k].GetType().Name + "с позиции [" + k +
                                                      "] клонировал " +
                                                      defArmy[cloneMe].GetType().Name + "'a " +
                                                      "(" + defArmy[cloneMe].Health + ") на позиции [" +
                                                      cloneMe + "]. " +
                                                      "В строю армии Jack Daniel'a новый союзник...");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[k].GetType().Name +
                                          "'а с позиции[" + k + "].");
                        defArmy.RemoveAt(k);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    }
                }

                #endregion
            }
        }
        /// <summary>
        /// Первая фаза боя - ближний бой
        /// </summary>
        private void Hit(List<IUnit> hitArmy, List<IUnit> defArmy, string hitter)
        {
            Random _randomValue = new Random((int)DateTime.Now.Ticks);

            string attackArmyName = "", defenceArmyName = "";

            if (hitter == "Captain Morgan")
            {
                attackArmyName = hitter;
                defenceArmyName = "Jack Daniel";
            }
            else
            {
                attackArmyName = "Jack Daniel";
                defenceArmyName = "Captain Morgan";
            }

            if (hitArmy.Count == 0) Engine.GameOver(defArmy, defenceArmyName);
            if (!hitArmy[0].IsAlive())
            {
                Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[0].GetType().Name + "'а с позиции[" + 0 + "].");
                hitArmy.RemoveAt(0);
                if (hitArmy.Count == 0) Engine.GameOver(defArmy, defenceArmyName);
                else Console.WriteLine("Приветствуем вместо него дерзкого " + hitArmy[0].GetType().Name + "'а.");
            }
            if (defArmy.Count == 0) Engine.GameOver(hitArmy, attackArmyName);
            if (!defArmy[0].IsAlive())
            {
                Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[0].GetType().Name + "'а с позиции[" + 0 + "].");
                defArmy.RemoveAt(0);
                if (defArmy.Count == 0) Engine.GameOver(hitArmy, attackArmyName);
                else Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[0].GetType().Name + "'а.");
            }

            if (hitArmy.Count != 0 && defArmy.Count != 0 && hitArmy[0].Strength > 0 &&
                hitArmy[0].IsAlive() && defArmy[0].IsAlive())
            {
                int randomValue = _randomValue.Next(-20, 20);
                //if (doHitArmyList[0].Strength <= 0) return; // Гуляй город не бьет
                defArmy[0].GetHit(hitArmy[0].Strength - defArmy[0].Armor + randomValue);
                if (defArmy[0].Health < 0) defArmy[0].Health = 0;
                Console.WriteLine(hitArmy[0].GetType().Name + "(" + hitArmy[0].Health + ")" +
                                  " с позиции[" + 0 + "] из армии " + attackArmyName + "'a, нанёс " +
                                  (hitArmy[0].Strength -
                                   defArmy[0].Armor + randomValue) + " урона воину " +
                                  defArmy[0].GetType().Name +
                                  "(" + defArmy[0].Health + ") на позиции[" + 0 + "] из армии " +
                                  defenceArmyName + "'a");
                if (!defArmy[0].IsAlive())
                {
                    Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[0].GetType().Name +
                                  "'а с позиции[" + 0 + "].");
                    defArmy.RemoveAt(0);
                    if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    else if (0 < defArmy.Count)
                        Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[0].GetType().Name + "'а.");
                }
                else
                {
                    var unit = defArmy[0] as IHeavyUnit;
                    if (unit != null)  //дебафф, если возможно
                    {
                        var tmp = unit;
                        new Infantry().DeBuff(ref tmp);
                        defArmy[0] = tmp;
                    }
                }
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Стратегия боя 3 на 3
    /// </summary>
    class Strategy2 : IStrategy
    {
        public void Combat(List<IUnit> hitArmy, List<IUnit> defArmy)
        {
            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Strategy 3x3");
            Random _randomValue = new Random((int)DateTime.Now.Ticks);

            //1 шеренга наносит удар
            Hit(hitArmy, defArmy, "Captain Morgan");
            //Обратка
            Hit(defArmy, hitArmy, "Jack Daniel");

            //Далее, начиная со второй позиции, юниты совершают специальные действия
            //Способности 1-й армии
            Console.WriteLine("*Captain Morgan's army Special Abilities*");
            for (int j = 3; j < hitArmy.Count; j++)
            {
                if (j >= hitArmy.Count || !hitArmy[j].IsAlive()) continue;

                #region Infantry
                //Если это оруженосец, одевает Armored, если есть рядом
                var infantry = hitArmy[j] as Infantry;
                if (infantry != null)
                {
                    int[] indexes = SpecialAbilityHelper(j); //Получаем индексы возможных Armored
                    foreach (int targetIndex in indexes)
                    {
                        if (targetIndex < 0 || targetIndex >= hitArmy.Count) continue;
                        var tmp = hitArmy[targetIndex] as IHeavyUnit;
                        if (tmp != null && tmp.IsAlive())
                        {
                            if(tmp is Horseman) continue;
                            _randomValue = new Random((int)DateTime.Now.Ticks);
                            if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                            Console.WriteLine(hitArmy[j].GetType().Name + " с позиции[" + j +
                                              "] из армии Captain Morgan'a экипировал " +
                                              hitArmy[targetIndex].GetType().Name + " на позиции[" + targetIndex + "]");
                            infantry.GiveBuff(ref tmp);
                            hitArmy[targetIndex] = tmp;
                            break;
                        }
                    }
                }
                #endregion

                #region Archer

                //Если это лучник, стреляет во врагов
                else if (hitArmy[j] is Archer)
                {
                    var archer = (Archer)hitArmy[j];
                    if (archer.IsAlive())
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        int concreteUnit = _randomValue.Next(0, defArmy.Count);
                        if (defArmy[concreteUnit].IsAlive())
                        {
                            var hp = defArmy[concreteUnit].Health;
                            archer.DoSpecialAction(defArmy[concreteUnit]);
                            //Использование метода RangeAttack интерфейса ISpecialAbility
                            if (defArmy[concreteUnit].Health < 0) defArmy[concreteUnit].Health = 0;
                            Console.WriteLine(hitArmy[j].GetType().Name + "(" +
                                              hitArmy[j].Health + ")" +
                                              " c позиции[" + j + "]  из армии Captain Morgan'a нанёс " +
                                              (hp - defArmy[concreteUnit].Health) +
                                              " урона воину " + defArmy[concreteUnit].GetType().Name +
                                              "(" + defArmy[concreteUnit].Health +
                                              ") на позиции[" + concreteUnit + "] из армии Jack Daniel'a.");
                            if (!defArmy[concreteUnit].IsAlive())
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[concreteUnit].GetType().Name +
                                                  "'а с позиции[" + concreteUnit + "].");
                                defArmy.RemoveAt(concreteUnit);
                                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " +
                                              defArmy[concreteUnit].GetType().Name + "'а с позиции[" + concreteUnit +
                                              "].");
                            defArmy.RemoveAt(concreteUnit);
                            if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                        }
                    }
                }
                #endregion

                #region Cleric

                //Если это лекарь
                else if (hitArmy[j] is Cleric)
                {
                    if (hitArmy[j].IsAlive())
                    {
                        var healer = (Cleric) hitArmy[j];
                        int[] indexes = SpecialAbilityHelper(j); //Получаем индексы возможных ICanBeHealed
                        foreach (int targetIndex in indexes)
                        {
                            if (targetIndex < 0 || targetIndex >= hitArmy.Count) continue;
                            var tmp = hitArmy[targetIndex] as ICanBeHealed;
                            if (tmp != null && hitArmy[targetIndex].IsAlive() &&
                                hitArmy[targetIndex].Health != tmp.MaxHP)
                            {
                                int prevHp = hitArmy[targetIndex].Health;
                                healer.DoSpecialAction(hitArmy[targetIndex]);
                                if (hitArmy[targetIndex].Health > tmp.MaxHP) hitArmy[targetIndex].Health = tmp.MaxHP;
                                Console.WriteLine(hitArmy[j].GetType().Name + "(" +
                                                  hitArmy[j].Health + ")" +
                                                  " с позиции[" + j + "] из армии Captain Morgan'a исцелил " +
                                                  hitArmy[targetIndex].GetType().Name +
                                                  "'a на позиции[" + targetIndex + "] на  " +
                                                  (hitArmy[targetIndex].Health - prevHp) +
                                                  "HP.");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[j].GetType().Name +
                                          "'а с позиции[" + j + "].");
                        hitArmy.RemoveAt(j);
                        if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                    }
                }
                #endregion

                #region Mage
                else if (hitArmy[j] is ProxyMage)
                {
                    if (hitArmy[j].IsAlive())
                    {
                        var cloner = (ProxyMage) hitArmy[j];
                        int[] indexes = SpecialAbilityHelper(j); //Получаем индексы возможных ICloneable
                        foreach (int targetIndex in indexes)
                        {
                            if (targetIndex < 0 || targetIndex >= hitArmy.Count) continue;
                            var tmp = hitArmy[targetIndex] as ICloneable;
                            if (tmp != null && hitArmy[targetIndex].IsAlive())
                            {
                                int listCountBeforeSpecialAction = hitArmy.Count;
                                cloner.DoSpecialAction(ref hitArmy, hitArmy[targetIndex]);
                                if (hitArmy.Count != listCountBeforeSpecialAction)
                                {
                                    Console.WriteLine(hitArmy[j].GetType().Name + " с позиции[" + j +
                                                      "] клонировал " +
                                                      hitArmy[targetIndex].GetType().Name + "'a" +
                                                      "(" + hitArmy[targetIndex].Health + ") на позиции[" +
                                                      targetIndex + "]. " +
                                                      "Армия Captain Morgan'a приветствует нового союзника...");
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[j].GetType().Name +
                                          "'а с позиции[" + j + "].");
                        hitArmy.RemoveAt(j);
                        if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                    }
                    }

                    #endregion
            }

            _randomValue = new Random((int)DateTime.Now.Ticks);
            //Вторая армия
            Console.WriteLine("*Jack Daniel's army Special Abilities*");
            for (int k = 3; k < defArmy.Count; k++)
            {
                if (k >= defArmy.Count || !defArmy[k].IsAlive()) continue;

                #region Infantry

                //Если это оруженосец, одевает Armored, если есть рядом
                var infantry = defArmy[k] as Infantry;
                if (infantry != null)
                {
                    int[] indexes = SpecialAbilityHelper(k); //Получаем индексы возможных Armored
                    foreach (int targetIndex in indexes)
                    {
                        if (targetIndex < 0 || targetIndex >= defArmy.Count) continue;
                        var tmp = defArmy[targetIndex] as IHeavyUnit;
                        if (tmp != null && tmp.IsAlive())
                        {
                            if(tmp is Horseman) continue;
                            _randomValue=new Random((int)DateTime.Now.Ticks);
                            if (_randomValue.Next(0, 4) != 2) break; //25% шанс
                            Console.WriteLine(defArmy[k].GetType().Name + " с позиции[" + k +
                                              "] из армии Jack Daniel'a экипировал " +
                                              defArmy[targetIndex].GetType().Name + " на позиции[" + targetIndex + "]");
                            infantry.GiveBuff(ref tmp);
                            defArmy[targetIndex] = tmp;
                            break;
                        }
                    }
                }
                #endregion

                #region Archer

                else if (defArmy[k] is Archer)
                {
                    var archer = (Archer)defArmy[k];
                    if (archer.IsAlive())
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        int concreteUnit = _randomValue.Next(0, hitArmy.Count);
                        if (hitArmy[concreteUnit].IsAlive())
                        {
                            var hp = hitArmy[concreteUnit].Health;
                            archer.DoSpecialAction(hitArmy[concreteUnit]);
                            //Использование метода RangeAttack интерфейса ISpecialAbility
                            if (hitArmy[concreteUnit].Health < 0) hitArmy[concreteUnit].Health = 0;
                            Console.WriteLine(defArmy[k].GetType().Name + "(" +
                                              defArmy[k].Health + ")" +
                                              "с позиции[" + k + "] из армии Jack Daniel'a нанёс " +
                                              (hp - hitArmy[concreteUnit].Health) +
                                              " урона воину " + hitArmy[concreteUnit].GetType().Name +
                                              "(" + hitArmy[concreteUnit].Health +
                                              ") на позиции[" + concreteUnit + "] из армии Captain Morgan'a.");
                            if (!hitArmy[concreteUnit].IsAlive())
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[concreteUnit].GetType().Name +
                                                  "'а с позиции[" + concreteUnit + "].");
                                hitArmy.RemoveAt(concreteUnit);
                                if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " +
                                              hitArmy[concreteUnit].GetType().Name + "'а с позиции[" + concreteUnit +
                                              "].");
                            hitArmy.RemoveAt(concreteUnit);
                            if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                        }
                    }
                }
                #endregion

                #region Cleric

                //Если это лекарь
                else if (defArmy[k] is Cleric)
                {
                    if (defArmy[k].IsAlive())
                    {
                        var healer = (Cleric) defArmy[k];
                        int[] indexes = SpecialAbilityHelper(k); //Получаем индексы возможных ICanBeHealed
                        foreach (int targetIndex in indexes)
                        {
                            if (targetIndex < 0 || targetIndex >= defArmy.Count) continue;
                            var tmp = defArmy[targetIndex] as ICanBeHealed;
                            if (tmp != null && defArmy[targetIndex].IsAlive() &&
                                defArmy[targetIndex].Health != tmp.MaxHP)
                            {
                                int prevHp = defArmy[targetIndex].Health;
                                healer.DoSpecialAction(defArmy[targetIndex]);
                                if (defArmy[targetIndex].Health > tmp.MaxHP) defArmy[targetIndex].Health = tmp.MaxHP;
                                Console.WriteLine(defArmy[k].GetType().Name + "(" +
                                                  defArmy[k].Health + ")" +
                                                  " с позиции[" + k + "] из армии Jack Daniel'a исцелил " +
                                                  defArmy[targetIndex].GetType().Name +
                                                  "'a на позиции[" + targetIndex + "] на  " +
                                                  (defArmy[targetIndex].Health - prevHp) +
                                                  "HP.");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[k].GetType().Name +
                                          "'а с позиции[" + k + "].");
                        defArmy.RemoveAt(k);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    }
                }
                #endregion

                #region Mage

                else if (defArmy[k] is ProxyMage)
                {
                    if (defArmy[k].IsAlive())
                    {
                        var cloner = (ProxyMage)defArmy[k];
                        int[] indexes = SpecialAbilityHelper(k); //Получаем индексы возможных ICloneable
                        foreach (int targetIndex in indexes)
                        {
                            if (targetIndex < 0 || targetIndex >= defArmy.Count) continue;
                            var tmp = defArmy[targetIndex] as ICloneable;
                            if (tmp != null && defArmy[targetIndex].IsAlive())
                            {
                                int listCountBeforeSpecialAction = defArmy.Count;
                                cloner.DoSpecialAction(ref defArmy, defArmy[targetIndex]);
                                if (defArmy.Count != listCountBeforeSpecialAction)
                                {
                                    Console.WriteLine(defArmy[k].GetType().Name + " с позиции[" + k +
                                                      "] клонировал " +
                                                      defArmy[targetIndex].GetType().Name + "'a" +
                                                      "(" + defArmy[targetIndex].Health + ") на позиции[" +
                                                      targetIndex + "]. " +
                                                      "Армия Captain Morgan'a приветствует нового союзника...");
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[k].GetType().Name +
                                          "'а с позиции[" + k + "].");
                        defArmy.RemoveAt(k);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// Первая фаза боя - ближний бой
        /// </summary>
        private void Hit(List<IUnit> hitArmy, List<IUnit> defArmy, string hitter)
        {
            for (int i = 0; i < 3 && i <Math.Min(hitArmy.Count,defArmy.Count); i++)
            {
                Random _randomValue = new Random((int)DateTime.Now.Ticks);

                string attackArmyName = "", defenceArmyName = "";

                if (hitter == "Captain Morgan")
                {
                    attackArmyName = hitter;
                    defenceArmyName = "Jack Daniel";
                }
                else
                {
                    attackArmyName = "Jack Daniel";
                    defenceArmyName = "Captain Morgan";
                }

                if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                if (!hitArmy[i].IsAlive())
                {
                    Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name + "'а с позиции[" + i + "].");
                    hitArmy.RemoveAt(i);
                    if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                    else if (i < hitArmy.Count) Console.WriteLine("Приветствуем вместо него дерзкого " + hitArmy[i].GetType().Name + "'а.");
                }
                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                if (!defArmy[i].IsAlive())
                {
                    Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name + "'а с позиции[" + i + "].");
                    defArmy.RemoveAt(i);
                    if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    else if (i < defArmy.Count) Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[i].GetType().Name + "'а.");
                }

                if (hitArmy.Count != 0 && defArmy.Count != 0 && i < Math.Min(hitArmy.Count, defArmy.Count) && hitArmy[i].Strength > 0  &&
                    hitArmy[i].IsAlive() && defArmy[i].IsAlive())
                {
                    int randomValue = _randomValue.Next(-20, 20);
                    //if (hitArmy[i].Strength <= 0) continue; // Гуляй город не бьет
                    defArmy[i].GetHit(hitArmy[i].Strength - defArmy[i].Armor + randomValue);
                    if (defArmy[i].Health < 0) defArmy[i].Health = 0;
                    Console.WriteLine(hitArmy[i].GetType().Name + "(" + hitArmy[i].Health + ")" +
                                      " с позиции[" + i + "] из армии " + attackArmyName + "'a, нанёс " +
                                      (hitArmy[i].Strength -
                                       defArmy[i].Armor + randomValue) + " урона воину " +
                                      defArmy[i].GetType().Name +
                                      "(" + defArmy[i].Health + ") на позиции[" + i + "] из армии " +
                                      defenceArmyName + "'a");
                    if (!defArmy[i].IsAlive())
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name +
                                      "'а с позиции[" + i + "].");
                        defArmy.RemoveAt(i);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                        else if (i < defArmy.Count)
                            Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[i].GetType().Name + "'а.");
                    }
                    else
                    {
                        var unit = defArmy[i] as IHeavyUnit;
                        if (unit == null) continue; //дебафф, если возможно
                        var tmp = unit;
                        new Infantry().DeBuff(ref tmp);
                        defArmy[i] = tmp;
                    }
                }
            }
            Console.WriteLine();
            
        }

        /// <summary>
        /// Метод для расчета индексов для применения специального действия для Infantry,Mage,Cleric в стратегии 3 на 3
        /// </summary>
        /// <param name="position">Индекс действующего юнита</param>
        /// <returns>Массив индексов</returns>
        public int[] SpecialAbilityHelper(int position)
        {
            int[] indexes = new int[4];
            indexes[0] = position - 3;
            indexes[1] = position / 3 == (position - 1) / 3 ? position - 1 : -1;
            indexes[2] = position / 3 == (position + 1) / 3 ? position + 1 : -1;
            indexes[3] = position + 3;
            return indexes;
        }
    }

    /// <summary>
    /// Стратегия боя стенка на стенку
    /// </summary>
    class Strategy3 : IStrategy
    {
        public void Combat(List<IUnit> hitArmy, List<IUnit> defArmy)
        {
            Console.WriteLine(new string('_', 50));
            Console.WriteLine("Strategy Wall");
            Random _randomValue = new Random((int) DateTime.Now.Ticks);

            //1 шеренга наносит удар
            Hit(hitArmy, defArmy, "Captain Morgan");
            //Обратка
            Hit(defArmy, hitArmy, "Jack Daniel");


            //Далее, начиная со второй позиции, юниты совершают специальные действия
            //Это всегда будет только одна из армий
            List<IUnit> _hitarmy;
            List<IUnit> _defarmy;
            string _hitarmystr;
            string _defarmystr;
            if (hitArmy.Count == defArmy.Count) return; //нет подходящих юнитов
            if (hitArmy.Count > defArmy.Count)
            {
                _hitarmy = hitArmy;
                _defarmy = defArmy;
                _hitarmystr = "Captain Morgan'a";
                _defarmystr = "Jack Daniel'a";
                Console.WriteLine("*Captain Morgan's army Special Abilities*");
            }
            else
            {
                _hitarmy = defArmy;
                _defarmy = hitArmy;
                _hitarmystr = "Jack Daniel'a";
                _defarmystr = "Captain Morgan'a";
                Console.WriteLine("*Jack Daniel's army Special Abilities*");
            }
            //начиная с первого юнита, напротив которого нет цели и до конца этой армии
            for (int j = Math.Min(_hitarmy.Count, _defarmy.Count); j < Math.Max(_hitarmy.Count, _defarmy.Count); j++)
            {
                if (j < _hitarmy.Count && _hitarmy[j].IsAlive())
                {
                    #region Infantry

                    //Если это оруженосец, одевает Armored, если есть рядом
                    var infantry = _hitarmy[j] as Infantry;
                    if (infantry != null)
                    {
                        var armored = _hitarmy[j - 1] as IHeavyUnit;
                        if (armored != null && armored.IsAlive() && !(armored is Horseman))
                        {
                            Console.WriteLine(_hitarmy[j].GetType().Name + " с позиции[" + j +
                                              "] из армии " + _hitarmystr + " экипировал " +
                                              _hitarmy[j - 1].GetType().Name + " на позиции[" + (j - 1) + "]");
                            infantry.GiveBuff(ref armored);
                            _hitarmy[j - 1] = armored;
                        }
                        else if(!(armored is Horseman))
                        {
                            if (j == _hitarmy.Count - 1) break; //Оруженосец в конце строя, за ним никого нет
                            armored = _hitarmy[j + 1] as IHeavyUnit;
                            if (armored != null && armored.IsAlive())
                            {
                                Console.WriteLine(_hitarmy[j].GetType().Name + " с позиции[" + j +
                                                  "] из армии " + _hitarmystr + " экипировал " +
                                                  _hitarmy[j + 1].GetType().Name + " на позиции[" + (j + 1) + "]");
                                infantry.GiveBuff(ref armored);
                                _hitarmy[j + 1] = armored;
                            }
                        }
                    }
                    #endregion

                    #region Archer

                    //Если это лучник, стреляет во врагов
                    else if (_hitarmy[j] is Archer)
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        var archer = (Archer) _hitarmy[j];
                        if (archer.IsAlive())
                        {
                            int concreteUnit = _randomValue.Next(0, _defarmy.Count);
                            if (_defarmy[concreteUnit].IsAlive())
                            {
                                var hp = _defarmy[concreteUnit].Health;
                                archer.DoSpecialAction(_defarmy[concreteUnit]);
                                //Использование метода RangeAttack интерфейса ISpecialAbility
                                if (_defarmy[concreteUnit].Health < 0) _defarmy[concreteUnit].Health = 0;
                                Console.WriteLine(_hitarmy[j].GetType().Name + "(" +
                                                  _hitarmy[j].Health + ")" +
                                                  " c позиции[" + j + "]  из армии " + _hitarmystr + " нанёс " +
                                                  (hp - _defarmy[concreteUnit].Health) +
                                                  " урона воину " + _defarmy[concreteUnit].GetType().Name +
                                                  "(" + _defarmy[concreteUnit].Health +
                                                  ") на позиции[" + concreteUnit + "] из армии" + _defarmystr + ".");
                                if (!_defarmy[concreteUnit].IsAlive())
                                {
                                    Console.WriteLine("С поля боя унесли дряблое тело " + _defarmy[concreteUnit].GetType().Name +
                                                      "'а с позиции[" + concreteUnit + "].");
                                    _defarmy.RemoveAt(concreteUnit);
                                    if (_defarmy.Count == 0) Engine.GameOver(_hitarmy, "Captain Morgan");
                                }
                            }
                            else
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " +
                                                  _defarmy[concreteUnit].GetType().Name + "'а с позиции[" + concreteUnit +
                                                  "].");
                                _defarmy.RemoveAt(concreteUnit);
                                if (_defarmy.Count == 0) Engine.GameOver(_hitarmy, "Captain Morgan");
                            }
                        }
                    }
                    #endregion

                    #region Cleric

                    //Если это лекарь
                    else if (_hitarmy[j] is Cleric)
                    {
                        _randomValue = new Random((int)DateTime.Now.Ticks);
                        if (_hitarmy[j].IsAlive())
                        {
                            if (j != _hitarmy.Count - 1) // Если не на последней позиции
                            {
                                var healer = (Cleric) _hitarmy[j];
                                var healMe = _randomValue.Next(j - 1, j + 2);
                                var youCanHealMe = _hitarmy[healMe] as ICanBeHealed;
                                if (_hitarmy[healMe].IsAlive())
                                {
                                    if (youCanHealMe != null &&
                                        _hitarmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                    {
                                        int prevHPvalue = _hitarmy[healMe].Health;
                                        healer.DoSpecialAction(_hitarmy[healMe]); // Подлечили юнита
                                        if (_hitarmy[healMe].Health > youCanHealMe.MaxHP)
                                            _hitarmy[healMe].Health = youCanHealMe.MaxHP;
                                        Console.WriteLine(_hitarmy[j].GetType().Name + "(" +
                                                          _hitarmy[j].Health + ")" +
                                                          " с позиции[" + j + "] из армии" + _hitarmystr + " исцелил " +
                                                          _hitarmy[healMe].GetType().Name +
                                                          "'a на позиции[" + healMe + "] на  " +
                                                          (_hitarmy[healMe].Health - prevHPvalue) +
                                                          "HP.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("С поля боя унесли дряблое тело " +
                                                      _hitarmy[healMe].GetType().Name + "'а с позиции[" + healMe + "].");
                                    _hitarmy.RemoveAt(healMe);
                                    if (_hitarmy.Count == 0) Engine.GameOver(_defarmy, "Jack Daniel");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " + _hitarmy[j].GetType().Name +
                                              "'а с позиции[" + j + "].");
                            _hitarmy.RemoveAt(j);
                            if (_hitarmy.Count == 0) Engine.GameOver(_defarmy, "Jack Daniel");
                        }
                        if (j == _hitarmy.Count - 1) // Если находится в конце армии
                        {
                            var healer = (Cleric) _hitarmy[j];
                            var healMe = _randomValue.Next(j - 1, j + 1);
                            var youCanHealMe = _hitarmy[healMe] as ICanBeHealed;
                            if (_hitarmy[healMe].IsAlive())
                            {
                                if (youCanHealMe != null &&
                                    _hitarmy[healMe].Health != youCanHealMe.MaxHP) // Не лечим если полное HP
                                {
                                    int prevHPvalue = _hitarmy[healMe].Health;
                                    healer.DoSpecialAction(_hitarmy[healMe]); // Подлечили юнита
                                    if (_hitarmy[healMe].Health > youCanHealMe.MaxHP)
                                        _hitarmy[healMe].Health = youCanHealMe.MaxHP;
                                    Console.WriteLine(_hitarmy[j].GetType().Name + "(" +
                                                      _hitarmy[j].Health + ")" +
                                                      " с позиции[" + j + "] из армии " + _hitarmystr + " исцелил " +
                                                      _hitarmy[healMe].GetType().Name +
                                                      "'a на позиции[" + healMe + "] на " +
                                                      (_hitarmy[healMe].Health - prevHPvalue) +
                                                      "HP");
                                }
                            }
                            else
                            {
                                Console.WriteLine("С поля боя унесли дряблое тело " +
                                                  _hitarmy[healMe].GetType().Name + "'а с позиции[" + healMe + "].");
                                _hitarmy.RemoveAt(healMe);
                                if (_hitarmy.Count == 0) Engine.GameOver(_defarmy, "Jack Daniel");
                            }
                        }
                    }
                    #endregion

                    #region Mage

                    else if (_hitarmy[j] is ProxyMage)
                    {
                        if (_hitarmy[j].IsAlive())
                        {
                            var cloner = (ProxyMage) _hitarmy[j];
                            // Если есть абилка и юнит Маг - пытаемся клонировать

                            int cloneMe;
                            if (j != _hitarmy.Count - 1) // Если не на последней позиции
                            {
                                if (_randomValue.Next(0, 2) == 0)
                                    cloneMe = j - 1;
                                else cloneMe = j + 1;
                                if (_hitarmy[cloneMe].IsAlive() && _hitarmy[cloneMe] is ICloneable)
                                {
                                    int listCountBeforeSpecialAction = _hitarmy.Count;
                                    cloner.DoSpecialAction(ref _hitarmy, _hitarmy[cloneMe]);
                                    if (_hitarmy.Count != listCountBeforeSpecialAction)
                                    {
                                        Console.WriteLine(_hitarmy[j].GetType().Name + " с позиции[" + j +
                                                          "] клонировал " +
                                                          _hitarmy[cloneMe].GetType().Name + "'a" +
                                                          "(" + _hitarmy[cloneMe].Health + ") на позиции[" +
                                                          cloneMe + "]. " +
                                                          "Армия " + _hitarmystr + " приветствует нового союзника...");
                                    }
                                }
                            }
                            if (j == _hitarmy.Count - 1) // Если маг в конце, клонируем того,кто перед ним
                            {
                                cloneMe = j - 1;
                                if (_hitarmy[cloneMe].IsAlive() && _hitarmy[cloneMe] is ICloneable)
                                {
                                    int listCountBeforeSpecialAction = _hitarmy.Count;
                                    cloner.DoSpecialAction(ref _hitarmy, _hitarmy[cloneMe]);
                                    if (_hitarmy.Count != listCountBeforeSpecialAction)
                                    {
                                        Console.WriteLine(_hitarmy[j].GetType().Name + " с позиции[" + j +
                                                          "] клонировал " +
                                                          _hitarmy[cloneMe].GetType().Name + "'a" +
                                                          "(" + _hitarmy[cloneMe].Health + ") на позиции[" +
                                                          cloneMe + "]. " +
                                                          "Армия " + _hitarmystr + " приветствует нового союзника...");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("С поля боя унесли дряблое тело " + _hitarmy[j].GetType().Name +
                                              "'а с позиции[" + j + "].");
                            _hitarmy.RemoveAt(j);
                            if (_hitarmy.Count == 0) Engine.GameOver(_defarmy, "Jack Daniel");
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Первая фаза боя - ближний бой
        /// </summary>
        private void Hit(List<IUnit> hitArmy, List<IUnit> defArmy, string hitter)
        {
            //В этом режиме воин бьет, если перед ним стоит вражеский воин
            for (int i = 0; i < Math.Min(hitArmy.Count, defArmy.Count); i++)
            {
                Random _randomValue = new Random((int) DateTime.Now.Ticks);

                string attackArmyName = "", defenceArmyName = "";

                if (hitter == "Captain Morgan")
                {
                    attackArmyName = hitter;
                    defenceArmyName = "Jack Daniel";
                }
                else
                {
                    attackArmyName = "Jack Daniel";
                    defenceArmyName = "Captain Morgan";
                }

                if (hitArmy.Count == 0) Engine.GameOver(defArmy, "jack Daniel");
                if (!hitArmy[i].IsAlive())
                {
                    Console.WriteLine("С поля боя унесли дряблое тело " + hitArmy[i].GetType().Name +
                                      "'а с позиции[" + i + "].");
                    hitArmy.RemoveAt(i);
                    if (hitArmy.Count == 0) Engine.GameOver(defArmy, "Jack Daniel");
                    else if (i < hitArmy.Count)
                        Console.WriteLine("Приветствуем вместо него дерзкого " + hitArmy[i].GetType().Name + "'а.");
                }
                if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                if (!defArmy[i].IsAlive())
                {
                    Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name +
                                      "'а с позиции[" + i + "].");
                    defArmy.RemoveAt(i);
                    if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                    else if (i < defArmy.Count)
                        Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[i].GetType().Name + "'а.");
                }

                if (hitArmy.Count != 0 && defArmy.Count != 0 && i < Math.Min(hitArmy.Count, defArmy.Count) &&
                    hitArmy[i].IsAlive() && defArmy[i].IsAlive())
                {
                    int randomValue = _randomValue.Next(-20, 20);
                    if (hitArmy[i].Strength <= 0) continue; // Гуляй город не бьет
                    defArmy[i].GetHit(hitArmy[i].Strength - defArmy[i].Armor + randomValue);
                    if (defArmy[i].Health < 0) defArmy[i].Health = 0;
                    Console.WriteLine(hitArmy[i].GetType().Name + "(" + hitArmy[i].Health + ")" +
                                      " с позиции[" + i + "] из армии " + attackArmyName + "'a, нанёс " +
                                      (hitArmy[i].Strength -
                                       defArmy[i].Armor + randomValue) + " урона воину " +
                                      defArmy[i].GetType().Name +
                                      "(" + defArmy[i].Health + ") на позиции[" + i + "] из армии " +
                                      defenceArmyName + "'a");
                    if (!defArmy[i].IsAlive())
                    {
                        Console.WriteLine("С поля боя унесли дряблое тело " + defArmy[i].GetType().Name +
                                      "'а с позиции[" + i + "].");
                        defArmy.RemoveAt(i);
                        if (defArmy.Count == 0) Engine.GameOver(hitArmy, "Captain Morgan");
                        else if (i < defArmy.Count)
                            Console.WriteLine("Приветствуем вместо него дерзкого " + defArmy[i].GetType().Name + "'а.");
                    }
                    else
                    {
                        var unit = defArmy[i] as IHeavyUnit;
                        if (unit == null) continue; //дебафф, если возможно
                        var tmp = unit;
                        new Infantry().DeBuff(ref tmp);
                        defArmy[i] = tmp;
                    }
                    
                }
            }
            Console.WriteLine();
        }
    }
    }

