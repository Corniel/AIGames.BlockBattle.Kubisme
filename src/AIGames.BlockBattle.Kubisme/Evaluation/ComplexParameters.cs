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
			// Elo: 1625, Avg: 0,628, Runs: 6261, ID: 6406, Parent: 6339
			{
				GarbagePotential = new int[] { 7, 21, 30, 53 },
				OwnFreeRows = new int[] { 27, 55, 61, 61, 69, 69, 71, 73, 78, 78, 80, 83, 84, 87, 93, 109, 110, 116, 134, 148, 153 },
				OppoFreeRows = new int[] { -4, -34, -45, -59, -66, -78, -79, -84, -88, -96, -96, -105, -108, -110, -114, -114, -115, -115, -116, -118, -153 },
				DifFreeRows = new int[] { 13, 17, 23, 25, 34, 57, 57, 61, 66, 66, 76, 78, 81, 81, 85, 92, 98, 131, 144, 146, 168 },
				Combo = 77,
				Holes = -174,
				WallsLeft = 53,
				WallsRight = 30,
				NeighborsHorizontal = -15,
				NeighborsVertical = 22,
				Floor = 171,
				TSpinPotential = 90,
				ComboPotential = new int[] { 76, 36, 30, 28, 28, 27, 22, 20, 19, 6, 1, -1, -17, -21, -23, -26, -33, -33, -49, -59, -61 },
				Unreachables = new int[] { -6, -13, -14, -23, -27, -29, -31, -37, -45, -50, -53, -53, -57, -60, -65, -73, -75, -86, -86, -94, -129 },
				Reachables = new int[] { 41, 35, 34, 33, 32, 12, 2, -5, -11, -16, -20, -22, -29, -41, -51, -56, -68, -69, -69, -73, -106 },
				Blockades = -3,
				LastBlockades = -6,
			};
		}
	}
}
