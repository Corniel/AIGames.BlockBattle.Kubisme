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
			LosingChanges = new int[] { 1, 0, 0, 0, 0, 0, 0 };
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

		[ParameterType(ParameterType.Descending | ParameterType.Positive)]
		public int[] LosingChanges { get; set; }

		public EvaluatorParameters Calc()
		{
			m_EmptyRows = new int[EmptyRows.Length];

			for (var i = 1; i < m_EmptyRows.Length; i++)
			{
				m_EmptyRows[i] = m_EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRowStaffle;
			}

			double Losing100 = LosingChanges[0];
			for (var i = 0; i < 6; i++)
			{
				var change = Math.Max(0.0, (double)(LosingChanges[i + 1] - 1)) / Losing100;
				m_EmptyRows[i] -= (int)(change * (double)Scores.Max);
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
			// Elo: 1617, Avg: 0.561, Runs: 1354, ID: 1084, Parent: 1074
			{
				// EmptyRowsCalc = new int[] { -4706, -1345, 203, 484, 580, 676, 768, 856, 942, 1027, 1106, 1181, 1251, 1320, 1387, 1451, 1514, 1575, 1636, 1693, 1745, 1792 },
				// UnreachableRowsCalc = new int[] { 0, -233, -484, -744, -1007, -1272, -1544, -1832, -2122, -2412, -2702, -2994, -3288, -3589, -3903, -4239, -4586, -4949, -5313, -5680, -6058, -6445 },
				Holes = -230,
				Points = 96,
				// Combo = 45,
				Combos = new int[] { 45, 33, 30, 23, 22, 21, 15, 10, 6, 2, -2, -2, -5, -17, -37, -38, -38, -57, -76, -102, -111, -145 },
				Skips = 102,
				DoublePotentialJLT = 97,
				DoublePotentialTSZ = 62,
				DoublePotentialO = 69,
				DoublePotentialI = 44,
				TriplePotentialJL = 57,
				TriplePotentialI = 68,
				TetrisPotential = 143,
				TSpinPontential = 102,
				SingleGroupBonus = new int[] { 76, 7, 11, 160 },
				Groups = new int[] { 40, 40, -15, -58, -99, -110 },
				// EmptyRowStaffle = 40,
				EmptyRows = new int[] { 183, 114, 67, 56, 56, 52, 48, 46, 45, 39, 35, 30, 29, 27, 24, 23, 21, 21, 17, 12, 7, 6 },
				Unreachables = new int[] { -3, -21, -30, -33, -35, -42, -58, -60, -60, -60, -62, -64, -71, -84, -106, -117, -133, -134, -137, -148, -157, -181 },
				LosingChanges = new int[] { 188, 28, 10, 2, 1, 1, 1 },
			};
			return pars.Calc();
		}
	}
}
