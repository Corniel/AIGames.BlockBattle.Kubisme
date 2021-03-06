﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class ParameterRandomizer
	{
		public const double Threshold = 0.4;
		public const int MinimumVariation = 20;

		public ParameterRandomizer(MT19937Generator rnd)
		{
			Rnd = rnd;
		}
		public MT19937Generator Rnd { get; set; }
				
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
				var tp = GetType(prop);

				if (prop.PropertyType == typeof(int))
				{
					int val = Randomize((int)prop.GetValue(org));

					if (tp.HasFlag(ParameterType.Positive) && val < 1)
					{
						val = 1;
					}
					else if (tp.HasFlag(ParameterType.Negative) && val >= 0)
					{
						val = -1;
					}
					prop.SetValue(target, val);
				}
				else if (prop.PropertyType == typeof(bool))
				{
					bool val = Randomize((bool)prop.GetValue(org));
					prop.SetValue(target, val);
				}
				else if (prop.PropertyType == typeof(int[]))
				{
					int[] vals = (int[])prop.GetValue(org);
					var copy = new List<int>();
					for (var i = 0; i < vals.Length; i++)
					{
						var val = Randomize(vals[i]);
						copy.Add(val);
					}

					if (tp.HasFlag(ParameterType.Ascending))
					{
						copy.Sort();
					}
					else if (tp.HasFlag(ParameterType.Descending))
					{
						copy = copy.OrderByDescending(v => v).ToList();
					}

					if (tp.HasFlag(ParameterType.Positive))
					{
						var min = copy.Min();

						if (min < 1)
						{
							for (var i = 0; i < copy.Count; i++)
							{
								copy[i] += 1 - min;
							}
						}
					}
					else if (tp.HasFlag(ParameterType.Negative))
					{
						var max = copy.Max();

						if (max >= 0)
						{
							for (var i = 0; i < copy.Count; i++)
							{
								copy[i] -= 1 + max;
							}
						}
					}
					prop.SetValue(target, copy.ToArray());
				}
				else if (prop.PropertyType == typeof(ParamCurve))
				{
					prop.SetValue(target, Randomize((ParamCurve)prop.GetValue(org)));
				}
			}
			return target;
		}

		private int Randomize(int value)
		{
			if (Rnd.NextDouble() < Threshold)
			{
				var factor = Rnd.NextDouble(-1, 1) * Rnd.NextDouble();
				var mp = Math.Max(MinimumVariation, Math.Abs(value) / 2);
				var val = value + mp * factor;
				return (int)Math.Round(val);
			}
			return value;
		}
		private bool Randomize(bool value)
		{
			if (Rnd.NextDouble() < Threshold)
			{
				return Rnd.NextBoolean();
			}
			return value;
		}

		private double Randomize(double value)
		{
			var min = Math.Min(value * 0.95, value - 0.1);
			var max = Math.Max(value * 1.05, value + 0.1);
			return Rnd.NextDouble(min, max);
		}

		private ParamCurve Randomize(ParamCurve value)
		{
			var a = Randomize(value.A);
			var power = Randomize(value.Power);
			var delta = Randomize(value.Delta);
			return new ParamCurve(a, power, delta);
		}

		private static readonly Dictionary<Type, PropertyInfo[]> Properties = new Dictionary<Type, PropertyInfo[]>();
		private static PropertyInfo[] GetProperties<T>()
		{
			PropertyInfo[] props;
			if (!Properties.TryGetValue(typeof(T), out props))
			{
				props = typeof(T)
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(prop => prop.CanWrite)
					.ToArray();

				Properties[typeof(T)] = props;
			}
			return props;
		}

		private static ParameterType GetType(PropertyInfo prop)
		{
			var attr = prop.GetCustomAttribute<ParameterTypeAttribute>();
			return attr == null ? ParameterType.None : attr.ParameterType;
		}
	}
}
