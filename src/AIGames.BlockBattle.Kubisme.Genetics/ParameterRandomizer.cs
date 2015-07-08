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

			for(var c = 0; c < count; c++)
			{
				var target = Activator.CreateInstance<T>();

				foreach(var prop in props)
				{
					if (prop.PropertyType == typeof(int))
					{
						int val = (int)prop.GetValue(org);
						val+= Rnd.Next(-100, 100);
						prop.SetValue(target, val);
					
					}
					else if (prop.PropertyType == typeof(int[]))
					{
						int[] vals = (int[])prop.GetValue(org);
						var copy = vals.ToArray();
						for(var i = 0; i < copy.Length; i++)
						{
							copy[i] = vals[i]+Rnd.Next(-100, 100);
						}
						prop.SetValue(target, copy);
					}
				}
				queue.Enqueue(target);
			}
		}
		Dictionary<Type, PropertyInfo[]> Properties = new Dictionary<Type, PropertyInfo[]>();
	}
}
