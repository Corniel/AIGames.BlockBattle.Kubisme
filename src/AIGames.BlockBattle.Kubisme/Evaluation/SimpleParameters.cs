﻿using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	[Serializable]
	public class SimpleParameters : IParameters
	{
		private static readonly PropertyInfo[] Props = typeof(SimpleParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);

		public SimpleParameters()
		{
			RowWeights = new int[21];
			RowCountWeights = new int[11];
		}

		public int[] RowWeights { get; set; }
		public int[] RowCountWeights { get; set; }

		public int Points { get; set; }
		public int Combo { get; set; }
		public int Holes { get; set; }
		public int Blockades { get; set; }
		public int WallsLeft { get; set; }
		public int WallsRight { get; set; }
		public int Floor { get; set; }
		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }

		public override string ToString()
		{
			var writer = new StringBuilder();
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int))
				{
					var val = (int)prop.GetValue(this);
					writer.AppendFormat("{0}: {1}, ", prop.Name, val);
				}
			}
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int[]))
				{
					var vals = (int[])prop.GetValue(this);
					writer.AppendFormat("{0}: {{{1}}}, ", prop.Name, String.Join(",", vals));
				}
			}
			writer.Remove(writer.Length - 2, 2);
			return writer.ToString();
		}

		public static SimpleParameters GetDefault()
		{
			return new SimpleParameters()
			// 1.060.699  0.07:22:16 Score: 70,44%, Win: 109,6, Lose: 99,3 Runs: 67.160, ID: 31076, Max: 1, Turns: 36, Points: 81
			{
				RowWeights = new int[] { -449, -401, -130, -149, -68, -48, -20, 6, 10, 20, 7, 10, 13, 35, 9, 37, 10, 8, 28, -6, 45 },
				RowCountWeights = new int[] { 73, 15, 27, 49, 47, 72, 88, 103, 95, 84, -51 },
				Points = 2450,
				Combo = 990,
				Holes = -1391,
				Blockades = -1207,
				WallsLeft = 946,
				WallsRight = 162,
				Floor = -544,
				NeighborsHorizontal = -757,
				NeighborsVertical = 1011,
			};
		}
	}
}
