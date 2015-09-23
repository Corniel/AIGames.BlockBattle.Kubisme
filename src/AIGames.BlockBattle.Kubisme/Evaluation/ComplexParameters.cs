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
			// Elo: 1034, Avg: 0,645, Runs: 1338, ID: 1718, Parent: 1685
			{
				GarbagePotential = new int[] { 2, 11, 16, 20 },
				OwnFreeRowsCalc = new int[] { 98, 84, 76, 73, 58, 57, 51, 50, 45, 44, 42, 41, 40, 39, 39, 35, 32, 31, 30, 21, 3, 2 },
				OppoFreeRowsCalc = new int[] { -114, -108, -94, -93, -91, -83, -81, -79, -77, -77, -75, -73, -73, -69, -67, -49, -47, -41, -40, -33, -24, -1 },
				DifFreeRowsCalc = new int[] { 1, 29, 30, 34, 37, 37, 39, 42, 50, 50, 50, 50, 58, 59, 59, 70, 71, 81, 86, 89, 101, 141 },
				Points = 35,
				Combo = 45,
				Holes = -52,
				WallsLeft = 12,
				WallsRight = 15,
				NeighborsHorizontal = -4,
				NeighborsVertical = 10,
				Floor = -4,
				TSpinPotential = 48,
				ComboPotential = new int[] { 22, 9, 7, 7, 5, 5, 2, 2, 1, 0, -4, -5, -6, -7, -7, -12, -12, -18, -24, -25, -29 },
				Unreachables = new int[] { -1, -2, -5, -8, -10, -24, -26, -27, -27, -28, -31, -33, -36, -37, -38, -39, -39, -44, -45, -45, -49 },
				Reachables = new int[] { 28, 27, 17, 15, 15, 9, 9, 8, 2, -7, -7, -8, -14, -14, -20, -36, -51, -52, -63, -64, -65 },
				Blockades = -1,
				LastBlockades = -1,
			};
			return pars.Calculate();
		}
	}
}
