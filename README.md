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
� ������ ��������� ������������ �������� �������� ��������������. �������� ����� ��������� ��������� �������� ��������� ����� � ������������ ���������� � ���������� ��������� ��������������. ��� ������ ������� ���������� ��������� �� C# Console Application, �� ������ ��� ���� ������� ������, ������� ��� ������� �� �����. ������� ������� ��� �����������, �.�. ������ �������� ������������� ��������� �� �������� ������������. � ����� ��� ������ ����� user-friendly, ��� ������, ���������� �� WinForms.

���� �������:
����������� ���������-����, ��������� �������� ��������������, ������� ����������� � ����������� ��������.

1.	���� ������������ ����� ����� ������� ��������� ���������. ���� ��� �����, � ������� ����� ����� � ���� ������� � ������� ���� ���� ����� �� �������. ���� ����������� ������� �����, ��������, ������� ���, ��������� ����� �������� ����������.
������� 2 ���� ������: ������(infantry) � �������(armored). ���� ������������ ����� ��������� �����, ������������� �� ���������� IUnit, ����� ������� �������������� - hp,damage,armor, cost. 
���� �������� ������ ���� - Engine - ����������� � ������� �������� ��������.
�������� �����, ����� �������� ��������� ������ ����������� � ������� ��������� ����������� ������� � ��������� �����.

2.	�������� ������ ����� - Archer unit, ����������� �� ISpecialAbility, ����� �������� � ����������. ����������� �������� ���������� ����� ������� ����� ����(����� ������� � ������� ��� - �������� ��� ���� ������ ������ �������)

3.	�������� ������ ����� - Cleric unit, ����� ������ ���������� ��������� ����� � �����, ������� ��������� ICanBeHealed, �������� �������� MaxHealth.
�������� ������ ����� - Mage unit, ����� ����������� ���������� ��������� ����� � �����, ������� ��������� ICloneable.

4.	�������� ������ ����� - GulyayGorod.dll, � ������� �������� �������. �� ����� ����, ����� ����� ��, ��������� ��� ������, ������� ������ ��������� ����.

5.	�������� ����������� ����������� �������� �����, ����� ������������� �������� ���������. �������� ������ �����(����������) ����� � ��������� ������ ���� �������� ����� ����� ������� ������/���/����/������, ��� ����� �������� ��� ��������������. ����� ��������� �����, ��������� � ��������� ������ �������.

6.	� ������� �������� ������ ����������� ����������� �������� �� ������ �� ����� ������(� ������ ��������� -  Mage)������������ ������� �����������. 3 �����������. 1) ��������� �� ������� ������ ���� ������(Cleric) � ����� � ����. 2) ������ ����. 3) ������ ����.

7.	������� Undo-Redo � ������� �������� �������. 

8.	�������� ������� ���������: 1) ����������� ��������� ��� 1 �� 1, ����� ����� ����� ���� �� ������ � ���� �������. 2) ��������� ��� 3 �� 3, ����� ����� ����� � ������� �� ����. 3) ������� ��� ������ �� ������, ����� ����� ����� �������� ����-�����, � 1 �������.
��� �����������, ���������� ������ � �����(� ������� �� ����) ����������� � ��������� ����, ��������� � ����� "Indexes 3 vs 3.txt"