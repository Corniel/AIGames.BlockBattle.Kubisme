using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class ParameterRandomizer
	{
		private enum ScaleType
		{
			None = 0,
			DivideBy2 = 1,
			MultiplyBy2 = 2,
		}

		public ParameterRandomizer(MT19937Generator rnd)
		{
			Rnd = rnd;
		}
		public MT19937Generator Rnd { get; set; }

		public T Copy<T>(T org)
		{
			var target = Activator.CreateInstance<T>();
			foreach (var prop in GetProperties<T>())
			{
				if (prop.PropertyType == typeof(int))
				{
					var val = (int)prop.GetValue(org);
					prop.SetValue(target, val);
				}
				else if (prop.PropertyType == typeof(int[]))
				{
					int[] vals = (int[])prop.GetValue(org);
					var copy = vals.ToArray();
					prop.SetValue(target, copy);
				}
			}
			return target;
		}
		public void Generate<T>(T org, Queue<T> queue, int count)
		{
			var props = GetProperties<T>();

			for (var c = 0; c < count; c++)
			{
				var target = Activator.CreateInstance<T>();

				foreach (var prop in props)
				{
					if (prop.PropertyType == typeof(int))
					{
						int val = Randomize((int)prop.GetValue(org));
						prop.SetValue(target, val);
					}
					else if (prop.PropertyType == typeof(int[]))
					{
						int[] vals = (int[])prop.GetValue(org);
						var copy = vals.ToArray();
						for (var i = 0; i < copy.Length; i++)
						{
							copy[i] = Randomize(vals[i]);
						}
						prop.SetValue(target, copy);
					}
				}
				queue.Enqueue(target);
			}
		}

		private int Randomize(int value)
		{
			var val = value;
			val +=
				Rnd.Next(-3, 3) *
				Rnd.Next(0, 3) * 
				Rnd.Next(0, 3) * 
				Rnd.Next(0, 3) * 
				Rnd.Next(0, 3);
			return val;
		}

		private static readonly Dictionary<Type, PropertyInfo[]> Properties = new Dictionary<Type, PropertyInfo[]>();
		private static PropertyInfo[] GetProperties<T>()
		{
			PropertyInfo[] props;
			if (!Properties.TryGetValue(typeof(T), out props))
			{
				props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
				Properties[typeof(T)] = props;
			}
			return props;
		}
	}
}
