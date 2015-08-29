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

		[ParameterType(ParameterType.Positive)]
		public int UnreachableWeightsFactor { get; set; }

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
			// Elo: 1018, Avg: 0,939, Runs: 3432, ID: 2777, Parent: 2639
			{
				FreeRowWeights = new int[] { 260, 151, 59, 27, 26, 24, 19, 18, 14, 13, 9, 7, 3, 1, 1, 1, 1, 1, 1, 1, 1 },
				ComboPotential = new int[] { 18, 6, 2, 1, -3, -3, -4, -7, -9, -11, -16, -17, -17, -18, -20, -24, -27, -36, -47, -51, -54 },
				Points = 76,
				Combo = 20,
				Holes = -85,
				Blockades = -1,
				LastBlockades = -8,
				WallsLeft = 23,
				WallsRight = 21,
				Floor = 5,
				NeighborsHorizontal = -2,
				NeighborsVertical = 14,
				TSpinPotential = 39,
			};
		}
	}
}
