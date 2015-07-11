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

		public void Generate<T>(T org, Queue<T> queue, int count)
		{
			PropertyInfo[] props;
			if (!Properties.TryGetValue(typeof(T), out props))
			{
				props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
				Properties[typeof(T)] = props;
			}

			for (var c = 0; c < count; c++)
			{
				var scaling = ScaleType.None;// (ScaleType)Rnd.Next(0, 4);

				var target = Activator.CreateInstance<T>();

				foreach (var prop in props)
				{
					if (prop.PropertyType == typeof(int))
					{
						int val = Randomize((int)prop.GetValue(org), scaling);
						prop.SetValue(target, val);
					}
					else if (prop.PropertyType == typeof(int[]))
					{
						int[] vals = (int[])prop.GetValue(org);
						var copy = vals.ToArray();
						for (var i = 0; i < copy.Length; i++)
						{
							copy[i] = Randomize(vals[i], scaling);
						}
						prop.SetValue(target, copy);
					}
				}
				queue.Enqueue(target);
			}
		}
		private int Randomize(int value, ScaleType scaling)
		{
			var val = value;
			switch (scaling)
			{
				case ScaleType.DivideBy2: val >>= 1; break;
				case ScaleType.MultiplyBy2: val <<= 1; break;
				default: break;
			}
			val += Rnd.Next(-20, 21);
			return val;
		}

		Dictionary<Type, PropertyInfo[]> Properties = new Dictionary<Type, PropertyInfo[]>();
	}
}
