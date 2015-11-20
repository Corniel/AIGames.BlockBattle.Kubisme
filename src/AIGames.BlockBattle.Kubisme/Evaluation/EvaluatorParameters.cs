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
			SingleEmpties = new int[5];
		}

		public int[] EmptyRowsCalc { get { return m_EmptyRows; } }
		private int[] m_EmptyRows;

		public int[] UnreachableRowsCalc { get { return m_UnreachableRows; } }
		private int[] m_UnreachableRows;

		/// <summary>Get the score per hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int Holes { get; set; }
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		/// <summary>Factor for current combo's.</summary>
		public int Combo { get { return Combos[0]; } }
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

		[ParameterType(ParameterType.Negative)]
		public int[] SingleEmpties { get; set; }

		/// <summary>Rows with a single (empty cell) group, of at least 6 cells filled,
		/// get a bonus, as they can be cleared easily.
		/// </summary>
		[ParameterType(ParameterType.Positive)]
		public int[] SingleGroupBonus { get; set; }

		/// <summary>Points for the different number of groups per reachable hole.</summary>
		[ParameterType(ParameterType.Descending)]
		public int[] Groups { get; set; }

		public int EmptyRowStaffle { get { return Groups[0]; } }

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
		/// 
		/// <code>
		/// var pars = new EvaluatorParameters()
		/// {
		///     Groups = new int[] { 2, 1, 0, -1, -2, -3 },
		///     Holes = -1,
		/// ;
		/// </code>
		/// </remarks>
		public static EvaluatorParameters GetDefault()
		{
			var pars = new EvaluatorParameters()
			// Elo: 1625, Avg: 0.566, Runs: 5766, ID: 2115, Parent: 2080
			{
				//EmptyRowsCalc = new int[] { -6939, -2290, 359, 486, 580, 669, 750, 829, 903, 975, 1034, 1092, 1148, 1197, 1240, 1283, 1322, 1360, 1397, 1432, 1467, 1501 },
				//UnreachableRowsCalc = new int[] { 0, -237, -484, -738, -994, -1254, -1519, -1806, -2097, -2396, -2700, -3005, -3313, -3633, -3953, -4288, -4625, -4974, -5344, -5716, -6089, -6466 },
				Holes = -236,
				Points = 90,
				//Combo = 47,
				Combos = new int[] { 47, 38, 31, 24, 23, 14, 13, 12, 7, 5, 5, -14, -23, -26, -32, -39, -44, -58, -62, -97, -112, -133 },
				Skips = 85,
				DoublePotentialJLT = 16,
				DoublePotentialTSZ = 98,
				DoublePotentialO = 86,
				DoublePotentialI = 23,
				TriplePotentialJL = 3,
				TriplePotentialI = 63,
				TetrisPotential = 152,
				TSpinPontential = 137,
				SingleGroupBonus = new int[] { 87, 21, 1, 171 },
				Groups = new int[] { 27, 24, -18, -73, -90, -104 },
				//EmptyRowStaffle = 27,
				EmptyRows = new int[] { 188, 117, 100, 67, 62, 54, 52, 47, 45, 32, 31, 29, 22, 16, 16, 12, 11, 10, 8, 8, 7, 1 },
				Unreachables = new int[] { -1, -11, -18, -20, -24, -29, -51, -55, -63, -68, -69, -72, -84, -84, -99, -101, -113, -134, -136, -137, -141, -166 },
			};
			return pars.Calc();
		}
	}
}
