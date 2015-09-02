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
			UnreachableWeights = new int[21];
			ReachableRange = new int[21];
			ComboPotential = new int[21];
		}

		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] FreeRowWeights { get; set; }

		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] UnreachableWeights { get; set; }

		[ParameterType(ParameterType.Descending)]
		public int[] ReachableRange { get; set; }

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
			// Elo: 1030, Avg: 0,891, Runs: 1475, ID: 13202, Parent: 13077
			{
				FreeRowWeights = new int[] { 50, 49, 41, 30, 27, 26, 24, 24, 20, 18, 16, 11, 10, 9, 9, 6, 5, 5, 4, 3, 1 },
				UnreachableWeights = new int[] { -1, -2, -11, -11, -12, -13, -13, -22, -25, -26, -35, -36, -40, -48, -51, -61, -66, -66, -75, -78, -96 },
				ReachableRange = new int[] { 41, 36, 32, 26, 18, 18, 15, 5, 1, -2, -8, -11, -13, -13, -24, -25, -27, -33, -43, -49, -66 },
				ComboPotential = new int[] { 13, 7, 5, 3, 2, 0, -5, -6, -8, -8, -11, -11, -18, -19, -22, -23, -33, -39, -42, -50, -79 },
				Points = 28,
				Combo = 9,
				Holes = -75,
				Blockades = 0,
				LastBlockades = -9,
				WallsLeft = 23,
				WallsRight = 24,
				Floor = 4,
				NeighborsHorizontal = -10,
				NeighborsVertical = 15,
				TSpinPotential = 49,
			};
		}
	}
}
