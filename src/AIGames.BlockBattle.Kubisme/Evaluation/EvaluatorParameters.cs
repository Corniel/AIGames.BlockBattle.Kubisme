using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			UnreachableFactor = 150;
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

		/// <summary>Get the score per hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int Holes { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		/// <summary>Genetics input.</summary>
		[ParameterType(ParameterType.Descending)]
		public int[] Combos { get; set; }

		public int EmptyRowStaffle { get { return Groups[0]; } }

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

		[ParameterType(ParameterType.Positive)]
		public int UnreachableFactor { get; set; }

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
				// On average, 1.5 hole per unreachable.
				m_UnreachableRows[i] += (Holes * UnreachableFactor) / 100;
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
			// Elo: 1643, Avg: 0.638, Runs: 1435, ID: 2364, Parent: 2353, Gen: 0.638231720858535
			{
				//EmptyRowsCalc = new int[] { 0, 118, 207, 288, 355, 402, 446, 487, 522, 555, 574, 588, 598, 606, 614, 620, 625, 629, 630, 630, 628, 621 },
				//UnreachableRowsCalc = new int[] { 0, -183, -369, -568, -768, -969, -1173, -1382, -1594, -1823, -2059, -2306, -2557, -2811, -3071, -3342, -3614, -3887, -4162, -4446, -4737, -5041 },
				//SingleEmptiesCalc = new int[] { 0, -16, -54, -183, -292, -465 },
				//Holes = -182,
				Points = 97,
				//Combo = 48,
				Combos = new int[] { 48, 23, 20, 18, 16, 7, 7, 6, -2, -3, -5, -6, -7, -15, -17, -19, -19, -20, -24, -25, -32, -35 },
				Skips = 135,
				DoublePotentialJLT = 22,
				DoublePotentialTSZ = 1,
				DoublePotentialO = 5,
				DoublePotentialI = 37,
				TriplePotentialJL = 13,
				TriplePotentialI = 5,
				TetrisPotential = 131,
				TSpinPontential = 550,
				SingleEmpties = new int[] { -14, -16, -27, -61, -73, -93 },
				SingleGroupBonus = new int[] { 37, 31, 24, 155 },
				Groups = new int[] { -10, -11, -65, -84, -85, -102 },
				//EmptyRowStaffle = -10,
				EmptyRows = new int[] { 128, 99, 91, 77, 57, 54, 51, 45, 43, 29, 24, 20, 18, 18, 16, 15, 14, 11, 10, 8, 3, 3 },
				Unreachables = new int[] { -1, -4, -17, -18, -19, -22, -27, -30, -47, -54, -65, -69, -72, -78, -89, -90, -91, -93, -102, -109, -122, -135 },
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
				//EmptyRowsCalc = new int[] { 0, 167, 326, 469, 610, 743, 873, 995, 1113, 1231, 1348, 1464, 1579, 1683, 1780, 1875, 1970, 2063, 2147, 2225, 2302, 2361 },
				//UnreachableRowsCalc = new int[] { 0, -205, -452, -719, -990, -1267, -1544, -1821, -2101, -2381, -2664, -2948, -3238, -3530, -3823, -4119, -4430, -4746, -5064, -5383, -5703, -6037 },
				//SingleEmptiesCalc = new int[] { 0, -21, -46, -117, -288, -390 },
				Holes = -198,
				Points = 69,
				//Combo = 49,
				Combos = new int[] { 49, 31, 27, 26, 25, 24, -1, -3, -4, -6, -7, -10, -12, -13, -19, -23, -29, -31, -33, -50, -80, -80 },
				Skips = 33,
				DoublePotentialJLT = 2,
				DoublePotentialTSZ = 1,
				DoublePotentialO = 2,
				DoublePotentialI = 33,
				TriplePotentialJL = 24,
				TriplePotentialI = 9,
				TetrisPotential = 3,
				TSpinPontential = 1,
				SingleEmpties = new int[] { -20, -21, -23, -39, -72, -78 },
				SingleGroupBonus = new int[] { 54, 1, 67, 46 },
				Groups = new int[] { 50, 8, 4, -11, -20, -46 },
				//EmptyRowStaffle = 50,
				EmptyRows = new int[] { 117, 109, 93, 91, 83, 80, 72, 68, 68, 67, 66, 65, 54, 47, 45, 45, 43, 34, 28, 27, 9, 1 },
				Unreachables = new int[] { -7, -49, -69, -73, -79, -79, -79, -82, -82, -85, -86, -92, -94, -95, -98, -113, -118, -120, -121, -122, -136, -149 },
			};
			return pars.Calc();
		}
	}
}
