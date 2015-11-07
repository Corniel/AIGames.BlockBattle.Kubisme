using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public const int LosingScore = short.MinValue;

		public EvaluatorParameters()
		{
			Unreachables = new int[22];
			EmptyRows = new int[22];
			Combos = new int[22];
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			ComboPotential = new int[16, 32];
			LosingChanges = new int[] { 1, 0, 0, 0, 0 };
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
			for (var i = 0; i < 4; i++)
			{
				var change = (double)LosingChanges[i + 1] / Losing100;
				m_EmptyRows[i] += (int)(change * (double)LosingScore);
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
			// Elo: 1628, Avg: 0,581, Runs: 4562, ID: 3815, Parent: 3677
			{
				//EmptyRowsCalc = new int[] { -6428, -6126, 123, 242, 514, 626, 717, 797, 873, 940, 1006, 1069, 1131, 1190, 1248, 1304, 1359, 1414, 1459, 1503, 1547, 1588 },
				//UnreachableRowsCalc = new int[] { 0, -163, -338, -518, -700, -891, -1082, -1278, -1478, -1683, -1889, -2101, -2313, -2526, -2740, -2959, -3183, -3411, -3641, -3873, -4116, -4380 },
				Holes = -152,
				Points = 65,
				//Combo = 33,
				Combos = new int[] { 33, 26, 18, 15, 10, 6, 5, 3, 2, 2, 0, -8, -11, -11, -20, -24, -32, -33, -37, -40, -42, -44 },
				Skips = 77,
				DoublePotentialJLT = 4,
				DoublePotentialTSZ = 11,
				DoublePotentialO = 39,
				DoublePotentialI = 55,
				TriplePotentialJL = 46,
				TriplePotentialI = 9,
				TetrisPotential = 109,
				TSpinPontential = 24,
				SingleGroupBonus = new int[] { 58, 37, 1, 115 },
				Groups = new int[] { 36, 36, 19, -17, -25, -83 },
				//EmptyRowStaffle = 36,
				EmptyRows = new int[] { 109, 98, 83, 80, 76, 55, 44, 40, 31, 30, 27, 26, 23, 22, 20, 19, 19, 9, 8, 8, 5, 1 },
				Unreachables = new int[] { -11, -23, -28, -30, -39, -39, -44, -48, -53, -54, -60, -60, -61, -62, -67, -72, -76, -78, -80, -91, -112, -129 },
				LosingChanges = new int[] { 209, 41, 40, 1, 1 },
			};
			return pars.Calc();
		}
	}
}
