### EN
This program uses the basic design patterns. The main goal of this program is to practice the implementations of design patterns. This is the second attempt of writing the C # Console Application, based on the project that was started but not finished by another person, and was completed by myself. Therefore, the code is repeated in some places, since I didn't want to redo the logic of the partially implemented patterns. Overall, this version is more user-friendly than the first one written using WinForms.

The task itself:
To develop a game program using design patterns, the task was supplemented gradually and carried out in stages.

1. The game is a kind of turn-based strategy. There are two armies in which the warriors stand in one column and strike each other in turn. There is an opportunity to create an army, to show, to make a move, the cost of the armies is about the same.
Create 2 types of warriors: infantry and armored. A warrior is a separate class inherited from the IUnit interface, has basic characteristics - hp, damage, armor, cost.
There is the main engine of the game - Engine - to be implemented using the Singleton pattern.
Creating armies, by creating individual warriors, implement using abstract factory and factory method patterns.

2. Add a new warrior - Archer unit, inherit from ISpecialAbility, can shoot from a distance. Special actions are called after the first stage of the turn (exchange of blows in melee - actions for all warriors of the first line)

3. Add a new warrior - Cleric unit, can heal suitable allies near him, create an ICanBeHealed interface, add the MaxHealth property.
Add a new warrior - Mage unit, can clone suitable allies next to him, create an ICloneable interface.

4. Add a new warrior - GulyayGorod.dll(WalkingCity), using the adapter pattern. Can't hit, has a lot of HP, acts as a wall that just takes damage.

5. Add the ability to modify a heavy warrior by using the decorator pattern. Adjacent light warriors (squires) can, with some chance, give a heavy warrior a stronger weapon / shield / helmet / horse, thereby increasing its characteristics. After being hit, the upgrades have a chance to fall.

6. Using the proxy pattern, implement logging of actions for one of the types of warriors (in this program - Mage) Use the observer pattern. 3 observers. 1) Watches the death of one type of warrior (Cleric) and writes to a file. 2) Changes color. 3) Makes a sound.

7. Make Undo-Redo using the command pattern.

8. Introduce the strategy pattern: 1) implement a 1v1 battle strategy, when the warriors stand behind each other in one column. 2) a 3v3 battle strategy, when the soldiers are standing as three columns. 3) the strategy of the battle wall to wall, when the soldiers stand opposite each other, in one line.
For clarity, the indexing of soldiers in the army (as three columns) is implemented in the following form, specified in the file "Indexes 3 vs 3.txt"

### RU
В данной программе используются основные паттерны проектирования. Основной целью написания программы является получение опыта в практическом применении и реализации паттернов проектирования. Это вторая попытка выполнения программы на C# Console Application, за основу был взят начатый проект, который был доделан до конца. Поэтому местами код повторяется, т.к. логику частично реализованных паттернов не хотелось переделывать. В целом эта версия более user-friendly, чем первая, написанная на WinForms.

Само задание:
Разработать программу-игру, используя паттерны проектирования, задание дополнялось и выполнялось поэтапно.

1.	Игра представляет собой некое подобие пошаговой стратегии. Есть две армии, в которой воины стоят в одну колонну и наносят удар друг другу по очереди. Есть возможность создать армию, показать, сделать ход, стоимость армий примерно одинаковая.
Создать 2 типа воинов: легкий(infantry) и тяжелый(armored). Воин представляет собой отдельный класс, наследующийся от интерфейса IUnit, имеет базовые характеристики - hp,damage,armor, cost. 
Есть основной движок игры - Engine - реализовать с помощью паттерна Одиночка.
Создание армий, путем создания отдельных воинов реализовать с помощью паттернов абстрактная фабрика и фабричный метод.

2.	Добавить нового воина - Archer unit, наследуется от ISpecialAbility, может стрелять с расстояния. Специальные действия вызываются после первого этапа хода(обмен ударами в ближнем бою - действия для всех воинов первой шеренги)

3.	Добавить нового воина - Cleric unit, может лечить подходящих союзников рядом с собой, создать интерфейс ICanBeHealed, добавить свойство MaxHealth.
Добавить нового воина - Mage unit, может клонировать подходящих союзников рядом с собой, создать интерфейс ICloneable.

4.	Добавить нового воина - GulyayGorod.dll, с помощью паттерна адаптер. Не может бить, имеет много хп, выступает как стенка, которая просто принимает урон.

5.	Добавить возможность модификации тяжелого воина, путем использования паттерна декоратор. Соседние легкие воины(оруженосцы) могут с некоторым шансом дать тяжелому воину более сильное оружие/щит/шлем/лошадь, тем самым увеличив его характеристики. После получения удара, улучшения с некоторым шансом спадают.

6.	С помощью паттерна прокси реализовать логирование действий по одному из типов воинов(в данной программе -  Mage)Использовать паттерн наблюдатель. 3 наблюдателя. 1) Наблюдает за смертью одного типа воинов(Cleric) и пишет в файл. 2) Меняет цвет. 3) Издает звук.

7.	Сделать Undo-Redo с помощью паттерна команда. 

8.	Внедрить паттерн стратегия: 1) реализовать стратегию боя 1 на 1, когда воины стоят друг за другом в одну колонну. 2) стратегию боя 3 на 3, когда воины стоят в колонну по трое. 3) страгию боя стенка на стенку, когда воины стоят напротив друг-друга, в 1 шеренгу.
Для наглядности, индексация воинов в армии(в колонну по трое) реализована в следующем виде, указанном в файле "Indexes 3 vs 3.txt"