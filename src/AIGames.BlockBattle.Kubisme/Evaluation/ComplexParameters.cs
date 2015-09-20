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
			// Elo: 1029, Avg: 0,661, Runs: 7922, ID: 1040, Parent: 1016
			{
				GarbagePotential = new int[] { 1, 4, 20, 32 },
				OwnFreeRows = new int[] { 1, 30, 46, 53, 61, 61, 61, 63, 68, 69, 74, 76, 77, 77, 79, 82, 85, 86, 88, 92, 98 },
				OppoFreeRows = new int[] { -1, -3, -8, -17, -20, -22, -28, -29, -32, -33, -38, -40, -41, -41, -43, -47, -51, -56, -63, -73, -95 },
				DifFreeRows = new int[] { 5, 7, 23, 25, 32, 36, 37, 38, 47, 47, 50, 51, 51, 54, 57, 58, 63, 69, 75, 81, 93 },
				Free = 1,
				Points = 13,
				Combo = 7,
				Holes = -122,
				WallsLeft = 20,
				WallsRight = 24,
				NeighborsHorizontal = -22,
				NeighborsVertical = 21,
				Floor = 14,
				PseudoGarbage = -2,
				TSpinPotential = 31,
				ComboPotential = new int[] { 24, 12, 8, 5, 4, 4, 3, 0, 0, -1, -1, -2, -4, -7, -8, -8, -11, -31, -34, -50, -63 },
				Unreachables = new int[] { -3, -10, -21, -29, -31, -39, -40, -40, -41, -44, -51, -51, -51, -55, -58, -59, -61, -64, -68, -73, -76 },
				Reachables = new int[] { 83, 34, 22, 16, 8, 8, 2, -3, -4, -5, -6, -7, -8, -9, -14, -18, -20, -24, -29, -43, -64 },
				Blockades = 2,
				LastBlockades = -13,
			};
		}
	}
}
