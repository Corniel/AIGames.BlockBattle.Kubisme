using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class ParameterRandomizer
	{
		public ParameterRandomizer(MT19937Generator rnd)
		{
			Rnd = rnd;

			var list = new List<sbyte>()
			{
				20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10,
				9, 9,
				8, 8,
				7, 7, 7,
				6, 6, 6, 6,
				5, 5, 5, 5, 5,
				4, 4, 4, 4, 4, 4,
				3, 3, 3, 3, 3, 3, 3, 3,
				2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
				1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			};

			var ls2 = list.ToList();
			foreach (var l in list)
			{
				ls2.Add((sbyte)-l);
			}

			Distribution = ls2.ToArray();
		}

		public sbyte[] Distribution { get; set; }
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
			for (var c = 0; c < count; c++)
			{
				var target = Randomize<T>(org);
				queue.Enqueue(target);
			}
		}

		public T Randomize<T>(T org)
		{
			var props = GetProperties<T>();
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
			return target;
		}

		private int Randomize(int value)
		{
			if (Rnd.Next(5) == 0)
			{
				var val = value + Distribution[Rnd.Next(Distribution.Length)];
				return val;
			}
			return value;
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
