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
			// Elo: 1614, Avg: 0,667, Runs: 1606, ID: 1010, Parent: 971, Gen: 40
			{
				//EmptyRowsCalc = new int[] { 0, 178, 305, 424, 527, 629, 729, 828, 909, 989, 1064, 1137, 1208, 1272, 1333, 1393, 1451, 1505, 1556, 1601, 1643, 1682 },
				//UnreachableRowsCalc = new int[] { 0, -271, -545, -828, -1113, -1408, -1708, -2010, -2320, -2633, -2951, -3269, -3595, -3924, -4254, -4588, -4923, -5261, -5605, -5953, -6304, -6691 },
				//SingleEmptiesCalc = new int[] { 0, -27, -74, -237, -320, -455 },
				//Combo = 26,
				//EmptyRowStaffle = 38,
				Holes = -135,
				Points = 136,
				Combos = new int[] { 26, 18, 15, 7, 6, 6, 5, 2, 0, -3, -5, -8, -11, -15, -19, -23, -28, -28, -34, -40, -42, -84 },
				Skips = 296,
				DoublePotentialJLT = 14,
				DoublePotentialTSZ = 69,
				DoublePotentialO = 15,
				DoublePotentialI = 17,
				TriplePotentialJL = 2,
				TriplePotentialI = 143,
				TetrisPotential = 318,
				TSpinPontential = 519,
				SingleEmpties = new int[] { -19, -27, -37, -79, -80, -91 },
				SingleGroupBonus = new int[] { 17, 51, 4, 84 },
				Groups = new int[] { 38, 34, -38, -57, -59, -61 },
				EmptyRows = new int[] { 140, 89, 81, 65, 64, 62, 61, 43, 42, 37, 35, 33, 26, 23, 22, 20, 16, 13, 7, 4, 1, 1 },
				Unreachables = new int[] { -1, -4, -13, -15, -25, -30, -32, -40, -43, -48, -48, -56, -59, -60, -64, -65, -68, -74, -78, -81, -117, -136 },
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
				//EmptyRowsCalc = new int[] { 0, 158, 308, 434, 550, 660, 760, 859, 955, 1046, 1135, 1222, 1305, 1387, 1464, 1541, 1616, 1689, 1760, 1826, 1889, 1951 },
				//UnreachableRowsCalc = new int[] { 0, -454, -915, -1380, -1855, -2333, -2816, -3300, -3786, -4273, -4760, -5247, -5737, -6229, -6722, -7221, -7723, -8251, -8781, -9313, -9849, -10404 },
				//SingleEmptiesCalc = new int[] { 0, -10, -88, -135, -248, -420 },
				//Combo = 40,
				//EmptyRowStaffle = 29,
				Holes = -221,
				Points = 79,
				Combos = new int[] { 40, 38, 26, 13, 4, 2, 0, 0, 0, -3, -7, -9, -12, -12, -13, -19, -20, -21, -24, -35, -36, -46 },
				Skips = 1,
				DoublePotentialJLT = 29,
				DoublePotentialTSZ = 4,
				DoublePotentialO = 27,
				DoublePotentialI = 12,
				TriplePotentialJL = 1,
				TriplePotentialI = 7,
				TetrisPotential = 1,
				TSpinPontential = 48,
				SingleEmpties = new int[] { -7, -10, -44, -45, -62, -84 },
				SingleGroupBonus = new int[] { 60, 11, 39, 29 },
				Groups = new int[] { 29, -2, -24, -29, -55, -56 },
				EmptyRows = new int[] { 129, 121, 97, 87, 81, 71, 70, 67, 62, 60, 58, 54, 53, 48, 48, 46, 44, 42, 37, 34, 33, 1 },
				Unreachables = new int[] { -12, -19, -23, -33, -36, -41, -42, -44, -45, -45, -45, -48, -50, -51, -57, -60, -86, -88, -90, -94, -113, -145 },
			};
			return pars.Calc();
		}
	}
}
