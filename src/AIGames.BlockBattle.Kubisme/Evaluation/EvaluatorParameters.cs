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
			// Elo: 1635, Avg: 0,500, Runs: 1311, ID: 3114, Parent: 3044
			{
				//EmptyRowsCalc = new int[] { 0,199,386,517,645,770,893,1001,1107,1207,1302,1395,1486,1571,1655,1739,1822,1904,1983,2062,2136,2207 },
				//UnreachableRowsCalc = new int[] { 0,-160,-346,-544,-750,-958,-1172,-1397,-1625,-1858,-2092,-2330,-2568,-2807,-3052,-3310,-3573,-3837,-4132,-4432,-4733,-5038 },
				Holes = -159,
				Points = 118,
				//Combo = 73,
				Combos = new int[] { 73, 37, 35, 22, 19, 16, 10, 8, 0, -2, -3, -5, -8, -15, -17, -26, -26, -42, -49, -70, -74, -88 },
				Skips = 3,
				DoublePotentialJLT = 43,
				DoublePotentialTSZ = 66,
				DoublePotentialO = 1,
				DoublePotentialI = 11,
				TriplePotentialJL = 10,
				TriplePotentialI = 20,
				TetrisPotential = 70,
				TSpinPontential = 46,
				SingleGroupBonus = new int[] { 95, 27, 49, 183 },
				Groups = new int[] { 68, 61, 49, -48, -61, -74 },
				//EmptyRowStaffle = 68,
				EmptyRows = new int[] { 131, 119, 63, 60, 57, 55, 40, 38, 32, 27, 25, 23, 17, 16, 16, 15, 14, 11, 11, 6, 3, 1 },
				Unreachables = new int[] { -1, -27, -39, -47, -49, -55, -66, -69, -74, -75, -79, -79, -80, -86, -99, -104, -105, -136, -141, -142, -146, -152 },
			};
			return pars.Calc();
		}
	}
}
