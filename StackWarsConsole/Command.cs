using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StackWarsConsole
{
    interface ICommand
    {
        void Undo(ref List<IUnit> a1, ref List<IUnit> a2);
        void Redo(ref List<IUnit> a1, ref List<IUnit> a2);

    }
    /// <summary>
    /// Инициатор команды - вызывает команду для выполнения определенного запроса
    /// </summary>
    class Invoker
    {
        internal Stack<ICommand> _commandsUndo = new Stack<ICommand>();//Стек отмены
        internal Stack<ICommand> _commandsRedo = new Stack<ICommand>();//Стек выполнения

        public Tuple<List<IUnit>, List<IUnit>> Undo(List<IUnit> a, List<IUnit> b)
        {
            if (_commandsUndo.Count == 0) return null; //Если нечего отменять
            _commandsRedo.Push(DeepClone.DoDeepClone(new NextTurnCommand(a, b))); //Текущую команду кладем в стек Redo
            _commandsUndo.Peek().Undo(ref a, ref b); //Изменяем состояния на предыдущие
            _commandsUndo.Pop();//Удаляем предыдущую команду 
            return Tuple.Create(a, b); //Возвращаем предыдущие состояния
        }

        public Tuple<List<IUnit>, List<IUnit>> Redo(List<IUnit> a, List<IUnit> b)
        {
            if (_commandsRedo.Count == 0) return null; //Если нечего выполнять
            _commandsUndo.Push(DeepClone.DoDeepClone(new NextTurnCommand(a, b)));//Текущую команду кладем в стек Undo
            _commandsRedo.Peek().Redo(ref a, ref b);//Изменяем состояния на последующие
            _commandsRedo.Pop();//Удаляем последующую команду
            return Tuple.Create(a, b);//Возвращаем последующие состояния
        }

        public void AddCommand(NextTurnCommand nextTurnCommand)
        {
            _commandsUndo.Push(nextTurnCommand);
        }

        public void ClearRedo()
        {
            _commandsRedo.Clear();
        }

    }
    /// <summary>
    /// Конкретная реализация команды
    /// </summary>
    [Serializable]
    class NextTurnCommand : ICommand
    {
        //Получатели команд
        private List<IUnit> _a;
        private List<IUnit> _b;

        public NextTurnCommand(List<IUnit> a, List<IUnit> b)
        {
            _a = a;
            _b = b;
        }
        public void Redo(ref List<IUnit> a1, ref List<IUnit> a2)
        {
            a1 = DeepClone.DoDeepClone(_a);
            a2 = DeepClone.DoDeepClone(_b);
        }

        public void Undo(ref List<IUnit> a1,ref  List<IUnit> a2)
        {
            a1 = DeepClone.DoDeepClone(_a);
            a2 = DeepClone.DoDeepClone(_b);
        }
    }

    static class DeepClone
    {
        public static T DoDeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
