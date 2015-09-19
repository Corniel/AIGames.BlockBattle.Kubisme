using System;
#if DEBUG
using System.Reflection;
using System.Text;
#endif

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class ComplexParameters : IParameters
	{
#if DEBUG
		private static readonly PropertyInfo[] Props = typeof(ComplexParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
#endif

		public ComplexParameters()
		{
			GarbagePotential = new int[4];
			
			OwnFreeRows = new int[21];
			OppoFreeRows = new int[21];
			DifFreeRows = new int[21];

			ComboPotential = new int[21];
			Unreachables = new int[21];
			Reachables = new int[21];
		}

		/// <summary>A value that corrects for the time it takes for creating new garbage.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] GarbagePotential { get; set; }

		/// <summary>The more free rows the we have, the better it is.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] OwnFreeRows { get; set; }

		/// <summary>The more free rows the opponent has, the worse it is.</summary>
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] OppoFreeRows { get; set; }

		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] DifFreeRows { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int Free { get; set; }

		public int Points { get; set; }
		public int Combo { get; set; }

		public int Holes { get; set; }

		public int WallsLeft { get; set; }
		public int WallsRight { get; set; }

		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }
		public int Floor { get; set; }

		/// <summary>Reachable lines, without combo bonus.</summary>
		public int PseudoGarbage { get; set; }
		
		public int TSpinPotential { get; set; }

		[ParameterType(ParameterType.Descending)]
		public int[] ComboPotential { get; set; }
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] Unreachables { get; set; }
		[ParameterType(ParameterType.Descending)]
		public int[] Reachables { get; set; }

		public int Blockades { get; set; }
		public int LastBlockades { get; set; }

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

		public static ComplexParameters GetDefault()
		{
			return new ComplexParameters()
			// Elo: 1030, Avg: 0,663, Runs: 4461, ID: 4385, Parent: 4098
			{
				GarbagePotential = new int[] { 18, 27, 29, 38 },
				OwnFreeRows = new int[] { 1, 11, 53, 53, 59, 59, 62, 68, 74, 75, 86, 91, 104, 109, 119, 123, 124, 125, 134, 136, 153 },
				OppoFreeRows = new int[] { -6, -7, -12, -15, -18, -27, -28, -42, -42, -53, -56, -65, -66, -70, -72, -90, -95, -99, -105, -110, -128 },
				DifFreeRows = new int[] { 7, 7, 12, 26, 33, 33, 41, 45, 49, 52, 56, 58, 61, 67, 82, 91, 96, 103, 115, 138, 168 },
				Free = 1,
				Points = 99,
				Combo = 32,
				Holes = -71,
				WallsLeft = 20,
				WallsRight = 20,
				NeighborsHorizontal = -8,
				NeighborsVertical = 20,
				Floor = -11,
				PseudoGarbage = 0,
				TSpinPotential = 56,
				ComboPotential = new int[] { 13, 12, 2, -1, -3, -4, -5, -5, -6, -11, -15, -17, -28, -28, -28, -31, -43, -53, -59, -67, -73 },
				Unreachables = new int[] { -1, -5, -42, -50, -51, -60, -60, -61, -63, -67, -75, -86, -97, -102, -112, -113, -117, -122, -137, -150, -171 },
				Reachables = new int[] { 39, 38, 38, 36, 31, 29, 20, 19, 17, 16, 5, 2, -4, -6, -17, -37, -41, -57, -59, -71, -78 },
				Blockades = -1,
				LastBlockades = -8,
			};
		}
	}
}
