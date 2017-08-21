using System;
using SpecialUnits;

namespace StackWarsConsole
{
    /// <summary>
    /// Общий интерфейс для фабрик
    /// </summary>
    interface ICaptain
    {   //Фабричный метод
        IUnit GiveLife();
    }

    /// <summary>
    /// Фабрика для создания Armored
    /// </summary>
    [Serializable]
    class ArmoredCaptain : ICaptain
    {   
        public IUnit GiveLife()
        {
            return new Armored();
        }
    }
    /// <summary>
    /// Фабрика для создания Infantry
    /// </summary>
    [Serializable]
    class InfantryCaptain : ICaptain
    {
        public IUnit GiveLife()
        {
            return new Infantry();
        }
    }
    /// <summary>
    /// Фабрика для создания Archer
    /// </summary>
    [Serializable]
    class ArcherCaptain : ICaptain
    {
        public IUnit GiveLife()
        {
            return new Archer();
        }
    }
    /// <summary>
    /// Фабрика для создания Cleric
    /// </summary>
    [Serializable]
    class ClericCaptain : ICaptain
    {
        public IUnit GiveLife()
        {
            return new Cleric();
        }
    }
    /// <summary>
    /// Фабрика для создания Mage
    /// </summary>
    [Serializable]
    class MageCaptain : ICaptain
    {
        public IUnit GiveLife()
        {
            return new ProxyMage(new Mage());
        }
    }
    /// <summary>
    /// Фабрика для создания GulayGorod
    /// </summary>
    [Serializable]
    class GulyayGorodCaptain : ICaptain
    {
        public IUnit GiveLife()
        {
            return new GGAdapted(new GulyayGorod(1000, 50, 150));
        }
    }
}