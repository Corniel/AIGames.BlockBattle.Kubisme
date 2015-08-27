using System;
#if DEBUG
using System.Reflection;
using System.Text;
#endif

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class SimpleParameters : IParameters
	{
#if DEBUG
		private static readonly PropertyInfo[] Props = typeof(SimpleParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
#endif
		public SimpleParameters()
		{
			FreeRowWeights = new int[21];
			ComboPotential = new int[21];
		}

		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] FreeRowWeights { get; set; }

		[ParameterType(ParameterType.Descending)]
		public int[] ComboPotential { get; set; }

		public int Points { get; set; }
		public int Combo { get; set; }
		public int Holes { get; set; }
		public int Blockades { get; set; }
		public int LastBlockades { get; set; }
		public int WallsLeft { get; set; }
		public int WallsRight { get; set; }
		public int Floor { get; set; }
		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }
		public int TSpinPotential { get; set; }

		/// <summary>Gets a string representation of the simple evaluator parameters.</summary>
		/// <remarks>
		/// Apparently, this code does not compile under Mono. As it is only for
		/// debug purposes, it is disabled in release mode.
		/// </remarks>
		public override string ToString()
		{
#if DEBUG
			var writer = new StringBuilder();
			foreach (var prop in Props)
			{
				if (prop.PropertyType == typeof(int))
				{
					var val = (int)prop.GetValue(this);
					writer.AppendFormat("{0}: {1}, ", prop.Name, val);
				}
				else if (prop.PropertyType == typeof(bool))
				{
					var val = (bool)prop.GetValue(this);
					writer.AppendFormat("{0}: {1}, ", prop.Name, val.ToString().ToLowerInvariant());
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
#else
			return base.ToString();
#endif
		}

		public static SimpleParameters GetDefault()
		{
			return new SimpleParameters()
			// Elo: 1064, Avg: 0,907, Runs: 1205, ID: 2323, Parent: 2284
			{
				FreeRowWeights = new int[] { 66, 59, 39, 36, 26, 22, 18, 16, 16, 10, 10, 9, 9, 1, 1, 1, 1, 1, 1, 1, 1 },
				ComboPotential = new int[] { 33, 10, 9, 6, 3, 1, 1, 1, -2, -9, -10, -10, -14, -21, -22, -23, -26, -33, -40, -49, -61 },
				Points = 73,
				Combo = 20,
				Holes = -84,
				Blockades = -1,
				LastBlockades = -12,
				WallsLeft = 21,
				WallsRight = 18,
				Floor = 9,
				NeighborsHorizontal = -4,
				NeighborsVertical = 14,
				TSpinPotential = 22,
			};
		}
	}
}
