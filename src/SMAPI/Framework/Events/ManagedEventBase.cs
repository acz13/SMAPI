using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewModdingAPI.Framework.Events
{
    /// <summary>The base implementation for an event wrapper which intercepts and logs errors in handler code.</summary>
    internal abstract class ManagedEventBase<TEventHandler>
    {
        /*********
        ** Properties
        *********/
        /// <summary>A human-readable name for the event.</summary>
        private readonly string EventName;

        /// <summary>Writes messages to the log.</summary>
        private readonly IMonitor Monitor;

        /// <summary>The mod registry with which to identify mods.</summary>
        private readonly ModRegistry ModRegistry;

        /// <summary>The display names for the mods which added each delegate.</summary>
        private readonly IDictionary<TEventHandler, IModMetadata> SourceMods = new Dictionary<TEventHandler, IModMetadata>();

        /// <summary>The cached invocation list.</summary>
        protected TEventHandler[] CachedInvocationList { get; private set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get whether anything is listening to the event.</summary>
        public bool HasListeners()
        {
            return this.CachedInvocationList?.Length > 0;
        }

        /*********
        ** Protected methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="eventName">A human-readable name for the event.</param>
        /// <param name="monitor">Writes messages to the log.</param>
        /// <param name="modRegistry">The mod registry with which to identify mods.</param>
        protected ManagedEventBase(string eventName, IMonitor monitor, ModRegistry modRegistry)
        {
            this.EventName = eventName;
            this.Monitor = monitor;
            this.ModRegistry = modRegistry;
        }

        /// <summary>Track an event handler.</summary>
        /// <param name="handler">The event handler.</param>
        /// <param name="invocationList">The updated event invocation list.</param>
        protected void AddTracking(TEventHandler handler, IEnumerable<TEventHandler> invocationList)
        {
            this.SourceMods[handler] = this.ModRegistry.GetFromStack();
            this.CachedInvocationList = invocationList?.ToArray() ?? new TEventHandler[0];
        }

        /// <summary>Remove tracking for an event handler.</summary>
        /// <param name="handler">The event handler.</param>
        /// <param name="invocationList">The updated event invocation list.</param>
        protected void RemoveTracking(TEventHandler handler, IEnumerable<TEventHandler> invocationList)
        {
            this.CachedInvocationList = invocationList?.ToArray() ?? new TEventHandler[0];
            if(!this.CachedInvocationList.Contains(handler)) // don't remove if there's still a reference to the removed handler (e.g. it was added twice and removed once)
                this.SourceMods.Remove(handler);
        }

        /// <summary>Log an exception from an event handler.</summary>
        /// <param name="handler">The event handler instance.</param>
        /// <param name="ex">The exception that was raised.</param>
        protected void LogError(TEventHandler handler, Exception ex)
        {
            if (this.SourceMods.TryGetValue(handler, out IModMetadata mod))
                mod.LogAsMod($"This mod failed in the {this.EventName} event. Technical details: \n{ex.GetLogSummary()}", LogLevel.Error);
            else
                this.Monitor.Log($"A mod failed in the {this.EventName} event. Technical details: \n{ex.GetLogSummary()}", LogLevel.Error);
        }
    }
}
