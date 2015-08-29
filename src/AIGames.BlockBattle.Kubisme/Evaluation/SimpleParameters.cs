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
			// Elo: 1022, Avg: 0.905, Runs: 4635, ID: 3267, Parent: 3219
			{
				FreeRowWeights = new int[] { 264, 252, 57, 41, 41, 30, 30, 25, 24, 24, 22, 19, 14, 14, 13, 13, 10, 7, 7, 5, 4 },
				UnreachableWeights = new int[] { -26, -42, -45, -65, -79, -80, -83, -86, -91, -93, -96, -104, -107, -108, -110, -124, -126, -133, -133, -147, -160 },
				ReachableRange = new int[] { 40, 20, 14, 12, 11, 10, 10, 10, 9, 8, 7, 4, -1, -2, -9, -15, -15, -17, -17, -30, -68 },
				ComboPotential = new int[] { 22, 16, 8, 1, 1, -1, -2, -5, -7, -8, -8, -8, -9, -10, -15, -19, -20, -22, -31, -66, -66 },
				UnreachableWeightsFactor = 11,
				Points = 51,
				Combo = 31,
				Holes = -108,
				Blockades = 0,
				LastBlockades = -10,
				WallsLeft = 35,
				WallsRight = 33,
				Floor = 47,
				NeighborsHorizontal = -14,
				NeighborsVertical = 21,
				TSpinPotential = 51,
			};
		}
	}
}
