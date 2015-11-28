using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			Unreachables = new int[22];
			EmptyRows = new int[22];
			Combos = new int[22];
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			ComboPotential = new int[16, 32];
			SingleEmpties = new int[6];
		}

		public int[] EmptyRowsCalc { get { return m_EmptyRows; } }
		private int[] m_EmptyRows;

		public int[] UnreachableRowsCalc { get { return m_UnreachableRows; } }
		private int[] m_UnreachableRows;

		public int[] SingleEmptiesCalc { get { return m_SingleEmpties; } }
		private int[] m_SingleEmpties;

		/// <summary>Factor for current combo's.</summary>
		public int Combo { get { return Combos[0]; } }

		public int EmptyRowStaffle { get { return Groups[0]; } }

		/// <summary>Get the score per hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int Holes { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		/// <summary>Genetics input.</summary>
		[ParameterType(ParameterType.Descending)]
		public int[] Combos { get; set; }

		/// <summary>Gets the combo potential given the current combo value (x) and the the counter (y).</summary>
		/// <remarks>
		/// 16 x 32
		/// </remarks>
		[XmlIgnore]
		public int[,] ComboPotential { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int Skips { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int DoublePotentialJLT { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int DoublePotentialTSZ { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int DoublePotentialO { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int DoublePotentialI { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int TriplePotentialJL { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int TriplePotentialI { get; set; }

		/// <summary>Points for a potential Tetris.</summary>
		[ParameterType(ParameterType.Positive)]
		public int TetrisPotential { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int TSpinPontential { get; set; }

		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] SingleEmpties { get; set; }

		/// <summary>Rows with a single (empty cell) group, of at least 6 cells filled,
		/// get a bonus, as they can be cleared easily.
		/// </summary>
		[ParameterType(ParameterType.Positive)]
		public int[] SingleGroupBonus { get; set; }

		/// <summary>Points for the different number of groups per reachable hole.</summary>
		[ParameterType(ParameterType.Descending)]
		public int[] Groups { get; set; }

		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] EmptyRows { get; set; }

		/// <summary>The less Unreachable.</summary>
		[ParameterType(ParameterType.Descending | ParameterType.Negative)]
		public int[] Unreachables { get; set; }

		public EvaluatorParameters Calc()
		{
			m_EmptyRows = new int[EmptyRows.Length];

			for (var i = 1; i < m_EmptyRows.Length; i++)
			{
				m_EmptyRows[i] = m_EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRowStaffle;
			}

			m_UnreachableRows = new int[Unreachables.Length];
			for (var i = 1; i < m_UnreachableRows.Length; i++)
			{
				m_UnreachableRows[i] = m_UnreachableRows[i - 1];
				m_UnreachableRows[i] += Unreachables[i - 1];
				// Take two holes for unreachables.
				m_UnreachableRows[i] += Holes << 1;
			}

			m_SingleEmpties = new int[SingleEmpties.Length];
			for (var i = 0; i < m_SingleEmpties.Length; i++)
			{
				m_SingleEmpties[i] = i * SingleEmpties[i];
			}

			for (var combo = 0; combo < 16; combo++)
			{
				var score = 0;
				for (var potential = 1; potential < Combos.Length; potential++)
				{
					score += (combo + 1 + potential) * Combos[potential];
					ComboPotential[combo, potential] = score;
				}
			}
			return this;
		}

		/// <summary>Gets the default parameters.</summary>
		/// <remarks>
		/// Input for the genetics algorithm and used by the 'real' bot.
		/// </remarks>
		public static EvaluatorParameters GetDefault()
		{
			var pars = new EvaluatorParameters()
			// Elo: 1631, Avg: 0,618, Runs: 1615, ID: 8738, Parent: 8679, Gen: 0,618321189220792
			{
				//EmptyRowsCalc = new int[] { 0, 130, 244, 330, 410, 488, 561, 634, 690, 743, 795, 842, 875, 908, 937, 961, 981, 1001, 1020, 1032, 1044, 1051 },
				//UnreachableRowsCalc = new int[] { 0, -130, -268, -413, -569, -725, -890, -1055, -1230, -1417, -1617, -1818, -2026, -2241, -2459, -2685, -2911, -3152, -3403, -3659, -3920, -4187 },
				//SingleEmptiesCalc = new int[] { 0, -32, -66, -216, -352, -525 },
				//Combo = 52,
				Holes = -217,
				Points = 95,
				Combos = new int[] { 52, 52, 32, 26, 12, 9, 9, 5, 3, 2, 2, -1, -3, -12, -13, -17, -21, -21, -24, -25, -26, -63 },
				//EmptyRowStaffle = 2,
				Skips = 157,
				DoublePotentialJLT = 10,
				DoublePotentialTSZ = 11,
				DoublePotentialO = 67,
				DoublePotentialI = 54,
				TriplePotentialJL = 28,
				TriplePotentialI = 5,
				TetrisPotential = 157,
				TSpinPontential = 549,
				SingleEmpties = new int[] { -13, -32, -33, -72, -88, -105 },
				SingleGroupBonus = new int[] { 51, 12, 36, 155 },
				Groups = new int[] { 2, -8, -60, -111, -113, -134 },
				EmptyRows = new int[] { 128, 112, 84, 78, 76, 71, 71, 54, 51, 50, 45, 31, 31, 27, 22, 18, 18, 17, 10, 10, 5, 1 },
				Unreachables = new int[] { -7, -15, -22, -33, -33, -42, -42, -52, -64, -77, -78, -85, -92, -95, -103, -103, -118, -128, -133, -138, -144, -156 },
			};
			return pars.Calc();
		}

		/// <summary>Gets the default parameters.</summary>
		/// <remarks>
		/// Input for the genetics algorithm and used by the 'real' bot.
		/// </remarks>
		public static EvaluatorParameters GetEndGame()
		{
			var pars = new EvaluatorParameters()
			{
				//EmptyRowsCalc = new int[] { 0, 183, 350, 509, 662, 811, 957, 1096, 1232, 1366, 1499, 1630, 1756, 1881, 2001, 2118, 2230, 2337, 2434, 2525, 2616, 2684 },
				//UnreachableRowsCalc = new int[] { 0, -277, -581, -890, -1211, -1534, -1859, -2185, -2518, -2853, -3188, -3534, -3881, -4228, -4578, -4935, -5298, -5665, -6033, -6414, -6799, -7201 },
				//SingleEmptiesCalc = new int[] { 0, -35, -90, -171, -428, -585 },
				//Combo = 49,
				//Holes = -202,
				Points = 71,
				Combos = new int[] { 49, 29, 23, 21, 9, 6, -1, -2, -4, -9, -15, -16, -18, -18, -29, -31, -32, -34, -40, -41, -42, -81 },
				//EmptyRowStaffle = 46,
				Skips = 73,
				DoublePotentialJLT = 30,
				DoublePotentialTSZ = 11,
				DoublePotentialO = 11,
				DoublePotentialI = 6,
				TriplePotentialJL = 8,
				TriplePotentialI = 18,
				TetrisPotential = 40,
				TSpinPontential = 24,
				SingleEmpties = new int[] { -9, -35, -45, -57, -107, -117 },
				SingleGroupBonus = new int[] { 39, 39, 90, 49 },
				Groups = new int[] { 46, 2, -12, -43, -55, -60 },
				EmptyRows = new int[] { 137, 121, 113, 107, 103, 100, 93, 90, 88, 87, 85, 80, 79, 74, 71, 66, 61, 51, 45, 45, 22, 4 },
				Unreachables = new int[] { -29, -56, -61, -73, -75, -77, -78, -85, -87, -87, -98, -99, -99, -102, -109, -115, -119, -120, -133, -137, -154, -158 },
			};
			return pars.Calc();
		}
	}
}
