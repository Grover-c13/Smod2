﻿using System;
using System.Collections.Generic;
using System.Linq;
using Smod2.EventHandlers;

namespace Smod2.Events
{


	public enum Priority {Highest = 100, High = 80, Normal = 50, Low = 20, Lowest = 0};

	public class EventManager
	{
		private static EventManager singleton;
		
		public static EventManager Manager
		{
			get
			{
				if (singleton == null)
				{
					singleton = new EventManager();
				}
				return singleton;
			}
		}

		private static PriorityComparator priorityCompare = new PriorityComparator();
		Dictionary<Type, List<EventHandlerWrapper>> event_meta;
		Dictionary<Type, List<IEventHandler>> event_handlers;

		public EventManager()
		{
			event_meta = new Dictionary<Type, List<EventHandlerWrapper>>();
			event_handlers = new Dictionary<Type, List<IEventHandler>>();
		}


		public void HandleEvent<T>(Event ev)
		{
			var list = this.GetEventHandlers<T>();

			foreach(IEventHandler handler in list)
			{
				try
				{
					ev.ExecuteHandler(handler);
				} catch (Exception e)
				{
					PluginManager.Manager.Logger.Error("Event", "Event Handler: " + handler.GetType().ToString() + " Failed to handle event:" + ev.GetType().ToString());
					PluginManager.Manager.Logger.Error("Event", e.ToString());
				}
			}
		}

		public void AddEventHandlers(Plugin plugin, IEventHandler handler, Priority priority = Priority.Normal)
		{
			foreach(Type intfce in handler.GetType().GetInterfaces()) {
				if (typeof(IEventHandler).IsAssignableFrom(intfce))
				{
					plugin.Debug("Adding event handler for " + intfce.Name);
					AddEventHandler(plugin, intfce, handler, priority);
				}
			}
		}

		public void AddEventHandler(Plugin plugin, Type eventType, IEventHandler handler, Priority priority=Priority.Normal)
		{
			plugin.Debug(string.Format("Adding event handler from: {0} type: {1} priority: {2} handler: {3}", plugin.Details.name, eventType, priority, handler.GetType()));
			EventHandlerWrapper wrapper = new EventHandlerWrapper(plugin, priority, handler);

			if (!event_meta.ContainsKey(eventType))
			{
				event_meta.Add(eventType, new List<EventHandlerWrapper>());
				event_meta[eventType].Add(wrapper);
				event_handlers.Add(eventType, new List<IEventHandler>());
				event_handlers[eventType].Add(handler);
			}
			else
			{
				List<EventHandlerWrapper> meta = event_meta[eventType];
				meta.Add(wrapper);
				// Doing this stuff on register instead of when the event is called for events that trigger lots (OnUpdate etc)
				meta.Sort(priorityCompare);
				meta.Reverse();
				RebuildHandlerList(eventType);
			}
		}

		public void RemoveEventHandlers(Plugin plugin)
		{
			Dictionary<Type, List<EventHandlerWrapper>> new_event_meta = new Dictionary<Type, List<EventHandlerWrapper>>();
			// loop through the meta dict finding any handlers from this plugin
			foreach (var meta in event_meta)
			{
				List<EventHandlerWrapper> metaList = meta.Value;
				List<EventHandlerWrapper> newList = new List<EventHandlerWrapper>();
				foreach (EventHandlerWrapper wrapper in metaList)
				{
					if (wrapper.Plugin != plugin)
					{
						newList.Add(wrapper);
					}
				}

				new_event_meta.Add(meta.Key, newList);
			}

			event_meta = new_event_meta;
			// rebuild handler list for each type
			foreach (var meta in event_meta)
			{
				RebuildHandlerList(meta.Key);
			}

		}

		private void RebuildHandlerList(Type eventType)
		{
			List<EventHandlerWrapper> meta = event_meta[eventType];
			List<IEventHandler> handlers = new List<IEventHandler>();
			foreach (EventHandlerWrapper metaDetails in meta)
			{
				handlers.Add(metaDetails.Handler);
			}

			if (event_handlers.ContainsKey(eventType))
			{
				event_handlers[eventType] = handlers;
			}
			else
			{
				event_handlers.Add(eventType, handlers);
			}
		}


		public List<T> GetEventHandlers<T>()
		{
			List<T> events;
			if (event_handlers.ContainsKey(typeof(T)))
			{
				events = event_handlers[typeof(T)].Cast<T>().ToList();
			}
			else
			{
				events = new List<T>();
			}

			return events;
		}

		private class PriorityComparator : IComparer<EventHandlerWrapper>
		{
			public int Compare(EventHandlerWrapper x, EventHandlerWrapper y)
			{
				return x.Priority.CompareTo(y.Priority);
			}
		}

	}


	public class EventHandlerWrapper
	{
		public Priority Priority { get; }
		public IEventHandler Handler { get; }
		public Plugin Plugin { get; }

		public EventHandlerWrapper(Plugin plugin, Priority priority, IEventHandler handler)
		{
			this.Plugin = plugin;
			this.Priority = priority;
			this.Handler = handler;
		}
	}

}
