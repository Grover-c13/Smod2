using System;
using System.Collections.Generic;
using Smod2.EventHandlers;

namespace Smod2.Events
{
	public enum Priority
	{
		FIRST = 0,
		EARLIER = 20,
		EARLY = 40,
		NORMAL = 50,
		LATE = 60,
		LATER = 70,
		LAST = 90,
		MONITOR = 100
	};

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
		private Dictionary<Type, List<EventHandlerWrapper>> event_meta;
		private readonly Dictionary<Plugin, Snapshot> snapshots;

		public EventManager()
		{
			event_meta = new Dictionary<Type, List<EventHandlerWrapper>>();
			snapshots = new Dictionary<Plugin, Snapshot>();
		}


		public void HandleEvent<T>(Event ev) where T : IEventHandler
		{
			foreach(T handler in GetEventHandlers<T>())
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

		public void AddEventHandlers(Plugin plugin, IEventHandler handler, Priority priority = Priority.NORMAL)
		{
			foreach(Type intfce in handler.GetType().GetInterfaces()) {
				if (typeof(IEventHandler).IsAssignableFrom(intfce))
				{
					plugin.Debug("Adding event handler for " + intfce.Name);
					AddEventHandler(plugin, intfce, handler, priority);
				}
			}
		}

		public void AddEventHandler(Plugin plugin, Type eventType, IEventHandler handler, Priority priority = Priority.NORMAL)
		{
			plugin.Debug(string.Format("Adding event handler from: {0} type: {1} priority: {2} handler: {3}", plugin.Details.name, eventType, priority, handler.GetType()));
			EventHandlerWrapper wrapper = new EventHandlerWrapper(plugin, priority, handler);

			// If the plugin is not enabled
			if (PluginManager.Manager.GetEnabledPlugin(plugin.Details.id) == null)
			{
				if (!snapshots.ContainsKey(plugin))
				{
					snapshots.Add(plugin, new Snapshot());
				}
				snapshots[plugin].Entries.Add(new Snapshot.SnapshotEntry(eventType, wrapper));
			}

			AddEventMeta(eventType, wrapper, handler);
		}

		private void AddEventMeta(Type eventType, EventHandlerWrapper wrapper, IEventHandler handler)
		{
			if (!event_meta.ContainsKey(eventType))
			{
				event_meta.Add(eventType, new List<EventHandlerWrapper>
				{
					wrapper
				});
			}
			else
			{
				List<EventHandlerWrapper> meta = event_meta[eventType];
				meta.Add(wrapper);
				// Doing this stuff on register instead of when the event is called for events that trigger lots (OnUpdate etc)
				meta.Sort(priorityCompare);
			}
		}

		public void AddSnapshotEventHandlers(Plugin plugin)
		{
			if (!snapshots.ContainsKey(plugin) || !snapshots[plugin].Active)
			{
				return;
			}

			snapshots[plugin].Active = false;
			foreach (Snapshot.SnapshotEntry entry in snapshots[plugin].Entries)
			{
				AddEventMeta(entry.Type, entry.Wrapper, entry.Wrapper.Handler);
			}
		}

		public void RemoveEventHandlers(Plugin plugin)
		{
			Dictionary<Type, List<EventHandlerWrapper>> new_event_meta = new Dictionary<Type, List<EventHandlerWrapper>>();
			// loop through the meta dict finding any handlers from this plugin
			foreach (var meta in event_meta)
			{
				List<EventHandlerWrapper> newList = new List<EventHandlerWrapper>();

				foreach (EventHandlerWrapper wrapper in meta.Value)
				{
					if (wrapper.Plugin != plugin)
					{
						newList.Add(wrapper);
					}
				}
				
				if (newList.Count > 0)
				{
					new_event_meta.Add(meta.Key, newList);
				}
			}
			
			event_meta = new_event_meta;

			if (snapshots.ContainsKey(plugin))
			{
				snapshots[plugin].Active = true;
			}
		}


		public List<T> GetEventHandlers<T>() where T : IEventHandler
		{
			List<T> events = new List<T>();
			if (event_meta.ContainsKey(typeof(T)))
			{
				foreach (EventHandlerWrapper wrapper in event_meta[typeof(T)])
				{
					if (wrapper.Handler is T tHandler)
					{
						events.Add(tHandler);
					}
				}
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

		private class Snapshot
		{
			public List<SnapshotEntry> Entries { get; private set; }
			public bool Active { get; set; }

			public Snapshot()
			{
				Entries = new List<SnapshotEntry>();
			}

			public class SnapshotEntry
			{
				public Type Type { get; }
				public EventHandlerWrapper Wrapper { get; }

				public SnapshotEntry(Type type, EventHandlerWrapper wrapper)
				{
					Type = type;
					Wrapper = wrapper;
				}
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
