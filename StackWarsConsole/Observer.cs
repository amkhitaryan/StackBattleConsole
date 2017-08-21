using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackWarsConsole
{

    /// <summary>
    /// Представляет наблюдателя, который подписывается на все уведомления наблюдаемого объекта.
    /// Определяет метод Update(), который вызывается наблюдаемым объектом для уведомления наблюдателя.
    /// </summary>
    interface IObserver
    {
        void Update();
    }

    /// <summary>
    /// Представляет наблюдаемый объект
    /// </summary>
    interface IObservable
    {
        void AddObserver(IObserver obs);
        void RemoveObserver(IObserver obs);
        void NotifyObservers();
    }
    /// <summary>
    /// Конкретный наблюдатель - издает звук
    /// </summary>
    [Serializable]
    class BeepObserver : IObserver
    {
        public void Update()
        {
            Console.Beep();
        }
    }
    /// <summary>
    /// Конкретный наблюдатель - меняет цвет
    /// </summary>
    [Serializable]
    class BlinkObserver : IObserver
    {
        public void Update()
        {
            Console.ForegroundColor = Console.ForegroundColor == ConsoleColor.DarkCyan ? ConsoleColor.Green : ConsoleColor.DarkCyan;
        }
    }
    /// <summary>
    /// Конкретный наблюдатель - пишет в файл
    /// </summary>
    [Serializable]
    class DeathLogObserver : IObserver
    {
        public async void Update()
        {
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DeathObserver.txt", true, Encoding.UTF8))
            {
                string text = (DateTime.Now + " В тяжелом сражении за своего короля пал верный Cleric...");
                Task writeTask = sw.WriteLineAsync(text.ToCharArray(), 0, text.Length);
                await writeTask;
            }
        }
    }
}
