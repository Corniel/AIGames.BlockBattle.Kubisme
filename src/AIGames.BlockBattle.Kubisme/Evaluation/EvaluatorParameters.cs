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
				m_UnreachableRows[i] += Holes + Holes >> 1;
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
			// Elo: 1643, Avg: 0.599, Runs: 1125, ID: 346, Parent: 331, Gen: 0.599222225778574
			{
				//EmptyRowsCalc = new int[] { 0, 106, 210, 292, 354, 413, 470, 518, 566, 612, 657, 698, 738, 777, 812, 846, 878, 910, 939, 964, 987, 1006 },
				//UnreachableRowsCalc = new int[] { 0, -190, -380, -580, -782, -990, -1199, -1408, -1622, -1838, -2055, -2274, -2500, -2727, -2957, -3200, -3455, -3711, -3970, -4232, -4495, -4762 },
				//SingleEmptiesCalc = new int[] { 0, -15, -56, -141, -216, -360 },
				Holes = -189,
				Points = 66,
				//Combo = 30,
				Combos = new int[] { 30, 22, 18, 17, 16, 15, 14, 14, 11, 7, 6, 6, 4, 3, 2, 2, -8, -11, -11, -17, -18, -20 },
				Skips = 42,
				DoublePotentialJLT = 1,
				DoublePotentialTSZ = 26,
				DoublePotentialO = 4,
				DoublePotentialI = 3,
				TriplePotentialJL = 1,
				TriplePotentialI = 21,
				TetrisPotential = 76,
				TSpinPontential = 524,
				SingleEmpties = new int[] { -1, -15, -28, -47, -54, -72 },
				SingleGroupBonus = new int[] { 37, 31, 27, 123 },
				Groups = new int[] { 10, 8, -10, -20, -22, -23 },
				//EmptyRowStaffle = 10,
				EmptyRows = new int[] { 96, 94, 72, 52, 49, 47, 38, 38, 36, 35, 31, 30, 29, 25, 24, 22, 22, 19, 15, 13, 9, 9 },
				Unreachables = new int[] { -1, -1, -11, -13, -19, -20, -20, -25, -27, -28, -30, -37, -38, -41, -54, -66, -67, -70, -73, -74, -78, -110 },
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
				//EmptyRowsCalc = new int[] { 0, 50, 96, 140, 183, 223, 262, 299, 335, 371, 403, 435, 465, 495, 525, 555, 585, 613, 641, 659, 672, 678 },
				//UnreachableRowsCalc = new int[] { 0, -201, -418, -638, -862, -1092, -1323, -1554, -1785, -2017, -2249, -2482, -2717, -2953, -3190, -3428, -3668, -3909, -4153, -4407, -4662, -4918 },
				//SingleEmptiesCalc = new int[] { 0, -14, -34, -96, -132, -390 },
				Holes = -200,
				Points = 22,
				//Combo = 32,
				Combos = new int[] { 32, 13, 10, 9, 6, 2, 1, 0, 0, -2, -3, -3, -4, -4, -4, -7, -9, -10, -10, -11, -21, -32 },
				Skips = 26,
				DoublePotentialJLT = 9,
				DoublePotentialTSZ = 1,
				DoublePotentialO = 1,
				DoublePotentialI = 1,
				TriplePotentialJL = 3,
				TriplePotentialI = 13,
				TetrisPotential = 1,
				TSpinPontential = 1,
				SingleEmpties = new int[] { -13, -14, -17, -32, -33, -78 },
				SingleGroupBonus = new int[] { 11, 1, 24, 23 },
				Groups = new int[] { 0, 0, -1, -3, -17, -19 },
				//EmptyRowStaffle = 0,
				EmptyRows = new int[] { 50, 46, 44, 43, 40, 39, 37, 36, 36, 32, 32, 30, 30, 30, 30, 30, 28, 28, 18, 13, 6, 1 },
				Unreachables = new int[] { -1, -17, -20, -24, -30, -31, -31, -31, -32, -32, -33, -35, -36, -37, -38, -40, -41, -44, -54, -55, -56, -58 },
			};
			return pars.Calc();
		}
	}
}
