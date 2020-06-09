using System;
using Smod2.Attributes;
using Smod2.Commands;
using Smod2.Config;
using Smod2.Events;
using System.Collections.Generic;
using Smod2.EventHandlers;
using Smod2.Lang;
using Smod2.API;
using Smod2.Permissions;
using Smod2.Logging;
using Smod2.Piping;

namespace Smod2
{
	public abstract class Plugin
	{
		public PluginDetails Details
		{
			get;
			internal set;
		}

		public PluginPipes Pipes
		{
			get;
			internal set;
		}

		[Obsolete("Use EventManager instead.")]
		public readonly EventManager eventManager = EventManager.Manager;
		public EventManager EventManager => EventManager.Manager;
		[Obsolete("Use PluginManager instead.")]
		public readonly PluginManager pluginManager = PluginManager.Manager;
		public PluginManager PluginManager => PluginManager.Manager;
		public LangManager LangManager => LangManager.Manager;
		public ConfigManager ConfigManager => ConfigManager.Manager;
		public ICommandManager CommandManager => PluginManager.CommandManager;
		public Logger Logger => PluginManager.Logger;
		public Server Server => PluginManager.Manager.Server;
		public Round Round => PluginManager.Manager.Server.Round;
		public abstract void Register();
		public abstract void OnEnable();
		public abstract void OnDisable();

		public void AddEventHandlers(IEventHandler handler, Priority priority = Priority.NORMAL)
		{
			EventManager.AddEventHandlers(this, handler, priority);
		}

		public void AddEventHandler(Type eventType, IEventHandler handler, Priority priority=Priority.NORMAL)
		{
			EventManager.AddEventHandler(this, eventType, handler, priority);
		}

		public void AddCommands(string[] commands, ICommandHandler handler)
		{
			foreach (string command in commands)
			{
				this.AddCommand(command, handler);
			}
		}

		public void AddCommand(string command, ICommandHandler handler)
		{
			if (PluginManager.CommandManager == null)
			{
				this.Error("Failed to register command handler because the command manager is null");
			}
			else
			{
				this.Debug("Command handler registered for command " + command);
				PluginManager.CommandManager.RegisterCommand(this, command, handler);
			}
		}

		public void RegisterPermissionsHandler(IPermissionsHandler handler)
		{
			if (PluginManager.Manager.PermissionsManager == null)
			{
				this.Error("Failed to register permissions handler because the permissions manager is null");
			}
			else
			{
				this.Debug(this.Details.name + " registered a permission handler.");
				PluginManager.Manager.PermissionsManager.RegisterHandler(handler);
			}
		}

		public void UnregisterPermissionsHandler(IPermissionsHandler handler)
		{
			if (PluginManager.Manager.PermissionsManager == null)
			{
				this.Error("Failed to remove permissions handler because the permissions manager is null");
			}
			else
			{
				this.Debug(this.Details.name + " unregistered a permission handler.");
				PluginManager.Manager.PermissionsManager.UnregisterHandler(handler);
			}
		}

		public void AddDefaultPermission(string permissionName)
		{
			if (PluginManager.Manager.PermissionsManager == null)
			{
				this.Error("Failed to register default permission because the permissions manager is null");
			}
			PluginManager.Manager.PermissionsManager.RegisterDefaultPermission(permissionName);
		}

		public void RemoveDefaultPermission(string permissionName)
		{
			if (PluginManager.Manager.PermissionsManager == null)
			{
				this.Error("Failed to unregister default permission because the permissions manager is null");
			}
			PluginManager.Manager.PermissionsManager.UnregisterDefaultPermission(permissionName);
		}

		public void AddConfig(ConfigSetting setting)
		{
			ConfigManager.RegisterConfig(this, setting);
		}

		public void AddTranslation(LangSetting setting)
		{
			LangManager.RegisterTranslation(this, setting);
		}

		private void CheckLangRegistered(string key)
		{
			if (!LangManager.IsRegistered(this, key))
			{
				this.Warn("Trying to access a lang setting [" + key + "] that isn't registered to the plugin, this is bad practice.");
			}
		}

		public string GetTranslation(string key)
		{
			CheckLangRegistered(key.ToUpper());
			return LangManager.GetTranslation(key);
		}

		private void CheckConfigRegistered(string key)
		{
			if (!ConfigManager.IsRegistered(this, key))
			{
				this.Warn("Trying to access a config setting that isn't registered to the plugin, this is bad practice.");
			}
		}

		public string GetConfigString(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is string def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetStringValue(key.ToUpper(), def, randomValues);
			}

			return string.Empty;
		}

		public int GetConfigInt(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is int def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetIntValue(key.ToUpper(), def, randomValues);
			}

			return -1;
		}

		public float GetConfigFloat(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is float def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetFloatValue(key.ToUpper(), def, randomValues);
			}

			return -1;
		}

		public bool GetConfigBool(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is bool def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetBoolValue(key.ToUpper(), def, randomValues);
			}

			return false;
		}

		public string[] GetConfigList(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is string[] def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetListValue(key.ToUpper(), def, randomValues);
			}

			return new string[0];
		}

		public int[] GetConfigIntList(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is int[] def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetIntListValue(key.ToUpper(), def, randomValues);
			}

			return new int[0];
		}

		public Dictionary<string, string> GetConfigDict(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is Dictionary<string, string> def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetDictValue(key.ToUpper(), def, randomValues);
			}

			return new Dictionary<string, string>();
		}

		public Dictionary<int, int> GetConfigIntDict(string key)
		{
			CheckConfigRegistered(key.ToUpper());

			if (ConfigManager.ResolveDefault(key.ToUpper()) is Dictionary<int, int> def)
			{
				ConfigSetting setting = ConfigManager.ResolvePrimary(key.ToUpper());
				bool randomValues = setting?.RandomizedValue ?? false;
				return ConfigManager.Config.GetIntDictValue(key.ToUpper(), def, randomValues);
			}

			return new Dictionary<int, int>();
		}

		public void InvokeEvent(string eventName) => InvokeEvent(eventName, null);
		public void InvokeEvent(string eventName, params object[] args) => InvokeExternalEvent(Details.id + "." + eventName, args);

		public void InvokeExternalEvent(string fullName) => InvokeExternalEvent(fullName, null);
		public void InvokeExternalEvent(string fullName, params object[] args)
		{
			if (fullName == null)
			{
				throw new ArgumentNullException(nameof(fullName));
			}

			PipeManager.Manager.InvokeEvent(fullName, Details.id, args);
		}

		public void Debug(string message)
		{
			PluginManager.Manager.Logger.Debug(this.Details.id, message);
		}

		public void Info(string message)
		{
			PluginManager.Manager.Logger.Info(this.Details.id, message);
		}

		public void Warn(string message)
		{
			PluginManager.Manager.Logger.Warn(this.Details.id, message);
		}

		public void Error(string message)
		{
			PluginManager.Manager.Logger.Error(this.Details.id, message);
		}

		public override string ToString()
		{
			if (Details == null)
			{
				return base.ToString();
			}
			return Details.name + "(" + Details.id + ")";
		}
	}
}
