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
		public int[] OppoFreeRows { get{return m_OppoFreeRows;}}
		private int[] m_OppoFreeRows;
		public int[] DifFreeRows { get { return m_DifFreeRows; } }
		private int[] m_DifFreeRows;

		public ComplexParameters Calculate()
		{
			m_OwnFreeRows = new int[OwnFreeRowsCalc.Length];
			m_OppoFreeRows = new int[OppoFreeRowsCalc.Length];
			m_DifFreeRows = new int[DifFreeRowsCalc.Length];

			CopyArrayValues(OwnFreeRowsCalc, m_OwnFreeRows);
			CopyArrayValues(OppoFreeRowsCalc, m_OppoFreeRows);
			CopyArrayValues(DifFreeRowsCalc, m_DifFreeRows);

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
			// Elo: 1028, Avg: 0,648, Runs: 3795, ID: 1405, Parent: 1364
			{
				GarbagePotential = new int[] { 13, 14, 41, 42 },
				OwnFreeRowsCalc = new int[] { 152, 109, 106, 96, 77, 62, 61, 61, 59, 57, 57, 46, 46, 43, 39, 38, 34, 34, 24, 20, 10, 4 },
				OppoFreeRowsCalc = new int[] { -106, -99, -83, -74, -71, -71, -68, -68, -66, -64, -62, -61, -58, -55, -49, -48, -46, -40, -23, -22, -3, -1 },
				DifFreeRowsCalc = new int[] { 1, 1, 3, 27, 29, 35, 37, 41, 41, 41, 42, 45, 46, 50, 57, 72, 73, 78, 78, 80, 90, 90 },
				Points = 42,
				Combo = 12,
				Holes = -104,
				WallsLeft = 28,
				WallsRight = 28,
				NeighborsHorizontal = -24,
				NeighborsVertical = 24,
				Floor = -1,
				TSpinPotential = 93,
				ComboPotential = new int[] { 28, 26, 23, 22, 16, 12, 9, 6, 5, 2, 1, -2, -2, -7, -9, -10, -11, -11, -17, -18, -19 },
				Unreachables = new int[] { -23, -28, -30, -31, -36, -40, -44, -45, -50, -51, -52, -55, -58, -60, -66, -78, -83, -83, -85, -89, -103 },
				Reachables = new int[] { 69, 55, 35, 35, 33, 29, 16, 15, 12, 8, 6, 3, 1, -1, -12, -18, -23, -29, -32, -34, -65 },
				Blockades = -1,
				LastBlockades = -10,
				//OwnFreeRows = new int[] { 152, 261, 367, 463, 540, 602, 663, 724, 783, 840, 897, 943, 989, 1032, 1071, 1109, 1143, 1177, 1201, 1221, 1231, 1235 },
				//OppoFreeRows = new int[] { -106, -205, -288, -362, -433, -504, -572, -640, -706, -770, -832, -893, -951, -1006, -1055, -1103, -1149, -1189, -1212, -1234, -1237, -1238 },
				//DifFreeRows = new int[] { 1, 2, 5, 32, 61, 96, 133, 174, 215, 256, 298, 343, 389, 439, 496, 568, 641, 719, 797, 877, 967, 1057 },
			};
			return pars.Calculate();
		}
	}
}
