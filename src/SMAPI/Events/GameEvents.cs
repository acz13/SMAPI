using System;
using StardewModdingAPI.Framework.Events;

namespace StardewModdingAPI.Events
{
    /// <summary>Events raised when the game changes state.</summary>
    public static class GameEvents
    {
        /*********
        ** Properties
        *********/
        /// <summary>The core event manager.</summary>
        private static EventManager EventManager;


        /*********
        ** Events
        *********/
        /// <summary>Raised when the game updates its state (≈60 times per second).</summary>
        public static event EventHandler UpdateTick
        {
            add => GameEvents.EventManager.Game_UpdateTick.Add(value);
            remove => GameEvents.EventManager.Game_UpdateTick.Remove(value);
        }

        /// <summary>Raised every other tick (≈30 times per second).</summary>
        public static event EventHandler SecondUpdateTick
        {
            add => GameEvents.EventManager.Game_SecondUpdateTick.Add(value);
            remove => GameEvents.EventManager.Game_SecondUpdateTick.Remove(value);
        }

        /// <summary>Raised every fourth tick (≈15 times per second).</summary>
        public static event EventHandler FourthUpdateTick
        {
            add => GameEvents.EventManager.Game_FourthUpdateTick.Add(value);
            remove => GameEvents.EventManager.Game_FourthUpdateTick.Remove(value);
        }

        /// <summary>Raised every eighth tick (≈8 times per second).</summary>
        public static event EventHandler EighthUpdateTick
        {
            add => GameEvents.EventManager.Game_EighthUpdateTick.Add(value);
            remove => GameEvents.EventManager.Game_EighthUpdateTick.Remove(value);
        }

        /// <summary>Raised every 15th tick (≈4 times per second).</summary>
        public static event EventHandler QuarterSecondTick
        {
            add => GameEvents.EventManager.Game_QuarterSecondTick.Add(value);
            remove => GameEvents.EventManager.Game_QuarterSecondTick.Remove(value);
        }

        /// <summary>Raised every 30th tick (≈twice per second).</summary>
        public static event EventHandler HalfSecondTick
        {
            add => GameEvents.EventManager.Game_HalfSecondTick.Add(value);
            remove => GameEvents.EventManager.Game_HalfSecondTick.Remove(value);
        }

        /// <summary>Raised every 60th tick (≈once per second).</summary>
        public static event EventHandler OneSecondTick
        {
            add => GameEvents.EventManager.Game_OneSecondTick.Add(value);
            remove => GameEvents.EventManager.Game_OneSecondTick.Remove(value);
        }

        /// <summary>Raised once after the game initialises and all <see cref="IMod.Entry"/> methods have been called.</summary>
        public static event EventHandler FirstUpdateTick
        {
            add => GameEvents.EventManager.Game_FirstUpdateTick.Add(value);
            remove => GameEvents.EventManager.Game_FirstUpdateTick.Remove(value);
        }


        /*********
        ** Public methods
        *********/
        /// <summary>Initialise the events.</summary>
        /// <param name="eventManager">The core event manager.</param>
        internal static void Init(EventManager eventManager)
        {
            GameEvents.EventManager = eventManager;
        }
    }
}
