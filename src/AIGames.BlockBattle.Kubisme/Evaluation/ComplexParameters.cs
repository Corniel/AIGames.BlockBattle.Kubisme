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
			GarbagePotential = new int[3];

			OwnFreeRowsCalc = new int[22];
			OppoFreeRowsCalc = new int[22];
			DifFreeRowsCalc = new int[22];

			ComboPotential = new int[21];
			Unreachables = new int[21];
			Reachables = new int[21];
		}

		/// <summary>A value that corrects for the time it takes for creating new garbage.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] GarbagePotential { get; set; }

		/// <summary>The more free rows the we have, the better it is.</summary>
		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] OwnFreeRowsCalc { get; set; }
	
		/// <summary>The more free rows the opponent has, the worse it is.</summary>
		[ParameterType(ParameterType.Ascending | ParameterType.Negative)]
		public int[] OppoFreeRowsCalc { get; set; }

		[ParameterType(ParameterType.Ascending | ParameterType.Positive)]
		public int[] DifFreeRowsCalc { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }
		
		public int Combo { get; set; }
		public int Skips { get; set; }

		[ParameterType(ParameterType.Negative)]
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

		[ParameterType(ParameterType.Negative)]
		public int Blockades { get; set; }
		[ParameterType(ParameterType.Negative)]
		public int LastBlockades { get; set; }

		public int[] OwnFreeRows { get { return m_OwnFreeRows; } }
		private int[] m_OwnFreeRows;

		public ComplexParameters Calculate()
		{
			m_OwnFreeRows = new int[OwnFreeRowsCalc.Length];
			CopyArrayValues(OwnFreeRowsCalc, m_OwnFreeRows);

			return this;
		}

		private void CopyArrayValues(int[] source, int[] target)
		{
			var sum = 0;
			for (var i = 0; i < source.Length; i++)
			{
				sum += source[i];
				target[i] = sum;
			}
		}

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
			var pars = new ComplexParameters()
			// Elo: 1728, Avg: 0,478, Runs: 2158, ID: 200, Parent: 168
			{
				GarbagePotential = new int[] { 1, 7, 28 },
				OwnFreeRowsCalc = new int[] { 157, 156, 144, 136, 133, 130, 125, 122, 119, 116, 115, 115, 112, 106, 103, 103, 103, 101, 97, 84, 58, 8 },
				OppoFreeRowsCalc = new int[] { -104, -101, -79, -73, -71, -63, -60, -57, -50, -45, -35, -34, -32, -28, -23, -23, -21, -17, -13, -12, -7, -1 },
				DifFreeRowsCalc = new int[] { 1, 50, 53, 64, 66, 67, 69, 69, 70, 70, 70, 70, 70, 71, 71, 72, 74, 75, 76, 77, 80, 109 },
				Points = 25,
				Combo = 6,
				Holes = -114,
				WallsLeft = 20,
				WallsRight = 24,
				NeighborsHorizontal = -22,
				NeighborsVertical = 33,
				Floor = 44,
				TSpinPotential = 35,
				ComboPotential = new int[] { 23, 11, 11, 8, 8, 5, 2, 1, 0, -4, -4, -4, -5, -9, -9, -11, -12, -29, -34, -51, -65 },
				Unreachables = new int[] { -4, -6, -19, -31, -31, -39, -43, -43, -44, -46, -52, -55, -59, -59, -61, -62, -64, -65, -74, -76, -77 },
				Reachables = new int[] { 90, 60, 19, 18, 1, -2, -2, -4, -5, -7, -7, -8, -9, -10, -12, -13, -15, -26, -30, -46, -61 },
				Blockades = -1,
				LastBlockades = -12,
				//OwnFreeRows = new int[] { 157, 313, 457, 593, 726, 856, 981, 1103, 1222, 1338, 1453, 1568, 1680, 1786, 1889, 1992, 2095, 2196, 2293, 2377, 2435, 2443 },
				//OppoFreeRows = new int[] { -104, -205, -284, -357, -428, -491, -551, -608, -658, -703, -738, -772, -804, -832, -855, -878, -899, -916, -929, -941, -948, -949 },
				//DifFreeRows = new int[] { 1, 51, 104, 168, 234, 301, 370, 439, 509, 579, 649, 719, 789, 860, 931, 1003, 1077, 1152, 1228, 1305, 1385, 1494 },
			};
			return pars.Calculate();
		}
	}
}
