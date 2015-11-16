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
			/// Elo: 1621, Avg: 0,602, Runs: 4110, ID: 37411, Parent: 37381
			{
				//EmptyRowsCalc = new int[] { -4681, -4282, 198, 325, 604, 702, 792, 874, 951, 1021, 1090, 1158, 1225, 1292, 1354, 1415, 1475, 1534, 1592, 1643, 1694, 1743 },
				//UnreachableRowsCalc = new int[] { 0, -234, -479, -740, -1004, -1272, -1549, -1832, -2116, -2409, -2704, -3001, -3312, -3632, -3961, -4298, -4644, -4994, -5345, -5700, -6073, -6476 },
				Holes = -233,
				Points = 87,
				//Combo = 35,
				Combos = new int[] { 35, 30, 26, 15, 14, 13, 11, 0, -2, -3, -7, -9, -18, -21, -25, -34, -40, -62, -70, -72, -92, -102 },
				Skips = 102,
				DoublePotentialJLT = 49,
				DoublePotentialTSZ = 60,
				DoublePotentialO = 74,
				DoublePotentialI = 48,
				TriplePotentialJL = 1,
				TriplePotentialI = 70,
				TetrisPotential = 154,
				TSpinPontential = 104,
				SingleGroupBonus = new int[] { 67, 1, 1, 138 },
				Groups = new int[] { 44, 41, 3, -58, -62, -96 },
				//EmptyRowStaffle = 44,
				EmptyRows = new int[] { 187, 90, 83, 68, 54, 46, 38, 33, 26, 25, 24, 23, 23, 18, 17, 16, 15, 14, 7, 7, 5, 2 },
				Unreachables = new int[] { -1, -12, -28, -31, -35, -44, -50, -51, -60, -62, -64, -78, -87, -96, -104, -113, -117, -118, -122, -140, -170, -177 },
				LosingChanges = new int[] { 196, 28, 27, 10, 5 },
			};
			return pars.Calc();
		}
	}
}
