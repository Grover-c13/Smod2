﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Smod2.Piping
{
	public class PluginPipes
	{
		private readonly Dictionary<string, FieldPipe> fields;
		private readonly Dictionary<string, PropertyPipe> properties;
		private readonly Dictionary<string, MethodPipe> methods;
		private readonly Dictionary<string, EventPipe> events;

		public PluginPipes(Plugin plugin)
		{
			Type pluginType = plugin.GetType();
			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

			fields = new Dictionary<string, FieldPipe>();
			foreach (FieldInfo field in pluginType.GetFields(flags))
			{
				PipeField pipe = field.GetCustomAttribute<PipeField>();
				if (pipe != null)
				{
					fields.Add(field.Name, Create<FieldPipe>(typeof(FieldPipe<>), field.FieldType, plugin, field, pipe));
				}
			}

			properties = new Dictionary<string, PropertyPipe>();
			foreach (PropertyInfo property in pluginType.GetProperties(flags))
			{
				PipeProperty pipe = property.GetCustomAttribute<PipeProperty>();
				if (pipe != null)
				{
					properties.Add(property.Name, Create<PropertyPipe>(typeof(PropertyPipe<>), property.PropertyType, plugin, property, pipe));
				}
			}

			methods = new Dictionary<string, MethodPipe>();
			events = new Dictionary<string, EventPipe>();
			foreach (MethodInfo method in pluginType.GetMethods(flags | BindingFlags.NonPublic))
			{
				if (method.IsPublic)
				{
					PipeMethod pipeMethod = method.GetCustomAttribute<PipeMethod>();
					if (pipeMethod != null)
					{
						methods.Add(method.Name, Create<MethodPipe>(typeof(MethodPipe<>), method.ReturnType, plugin, method, pipeMethod));

						continue;
					}
				}

				PipeEvent pipeEvent = method.GetCustomAttribute<PipeEvent>();
				if (pipeEvent != null)
				{
					events.Add(method.Name, new EventPipe(plugin, method, pipeEvent));
				}
			}
		}
		
		private static T Create<T>(Type pipeType, Type returnType, Plugin source, MemberInfo info) where T : MemberPipe
		{
			return (T)Activator.CreateInstance(pipeType.MakeGenericType(returnType), source, info);
		}
		
		private static T Create<T>(Type pipeType, Type returnType, Plugin source, MemberInfo info, Attribute pipe) where T : MemberPipe
		{
			return (T)Activator.CreateInstance(pipeType.MakeGenericType(returnType), source, info, pipe);
		}

		private static T[] DuplicateCollection<T>(ICollection<T> collection)
		{
			T[] result = new T[collection.Count];
			int i = 0;

			foreach (T element in collection)
			{
				result[i] = element;

				i++;
			}

			return result;
		}

		public bool HasField(string name) => fields.ContainsKey(name);
		public FieldPipe GetField(string name) => fields[name];
		public FieldPipe[] GetFields() => DuplicateCollection(fields.Values);

		public bool HasProperty(string name) => properties.ContainsKey(name);
		public PropertyPipe GetProperty(string name) => properties[name];
		public PropertyPipe[] GetProperties() => DuplicateCollection(properties.Values);

		public bool HasMethod(string name) => methods.ContainsKey(name);
		public MethodPipe GetMethod(string name) => methods[name];
		public MethodPipe[] GetMethods() => DuplicateCollection(methods.Values);

		public bool HasEvent(string name) => events.ContainsKey(name);
		public EventPipe GetEvent(string name) => events[name];
		public EventPipe[] GetEvents() => DuplicateCollection(events.Values);
	}
}
