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
			// Elo: 1623, Avg: 0.591, Runs: 4078, ID: 612, Parent: 474
			{
				//EmptyRowsCalc = new int[] { 0, 96, 178, 250, 310, 369, 425, 476, 524, 569, 613, 655, 693, 730, 765, 800, 835, 864, 891, 918, 939, 960 },
				//UnreachableRowsCalc = new int[] { 0, -175, -360, -546, -734, -927, -1131, -1335, -1540, -1749, -1961, -2174, -2390, -2607, -2824, -3043, -3264, -3495, -3739, -3985, -4234, -4483 },
				//SingleEmptiesCalc = new int[] { 0, -8, -30, -102, -144, -235 },
				Holes = -174,
				Points = 66,
				//Combo = 28,
				Combos = new int[] { 28, 19, 17, 15, 12, 11, 9, 8, 8, 4, 4, 2, 0, 0, -2, -2, -3, -6, -14, -17, -23, -26 },
				Skips = 25,
				DoublePotentialJLT = 1,
				DoublePotentialTSZ = 58,
				DoublePotentialO = 5,
				DoublePotentialI = 35,
				TriplePotentialJL = 1,
				TriplePotentialI = 23,
				TetrisPotential = 76,
				TSpinPontential = 498,
				SingleEmpties = new int[] { -1, -8, -15, -34, -36, -47 },
				SingleGroupBonus = new int[] { 28, 31, 28, 91 },
				Groups = new int[] { 10, 8, -3, -10, -25, -37 },
				//EmptyRowStaffle = 10,
				EmptyRows = new int[] { 86, 72, 62, 50, 49, 46, 41, 38, 35, 34, 32, 28, 27, 25, 25, 25, 19, 17, 17, 11, 11, 5 },
				Unreachables = new int[] { -1, -11, -12, -14, -19, -30, -30, -31, -35, -38, -39, -42, -43, -43, -45, -47, -57, -70, -72, -75, -75, -101 },
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
			// Elo: 1623, Avg: 0.591, Runs: 4078, ID: 612, Parent: 474
			{
				//EmptyRowsCalc = new int[] { 0, 96, 178, 250, 310, 369, 425, 476, 524, 569, 613, 655, 693, 730, 765, 800, 835, 864, 891, 918, 939, 960 },
				//UnreachableRowsCalc = new int[] { 0, -175, -360, -546, -734, -927, -1131, -1335, -1540, -1749, -1961, -2174, -2390, -2607, -2824, -3043, -3264, -3495, -3739, -3985, -4234, -4483 },
				//SingleEmptiesCalc = new int[] { 0, -8, -30, -102, -144, -235 },
				Holes = -174,
				Points = 66,
				//Combo = 28,
				Combos = new int[] { 28, 19, 17, 15, 12, 11, 9, 8, 8, 4, 4, 2, 0, 0, -2, -2, -3, -6, -14, -17, -23, -26 },
				Skips = 25,
				DoublePotentialJLT = 1,
				DoublePotentialTSZ = 58,
				DoublePotentialO = 5,
				DoublePotentialI = 35,
				TriplePotentialJL = 1,
				TriplePotentialI = 23,
				TetrisPotential = 76,
				TSpinPontential = 498,
				SingleEmpties = new int[] { -1, -8, -15, -34, -36, -47 },
				SingleGroupBonus = new int[] { 28, 31, 28, 91 },
				Groups = new int[] { 10, 8, -3, -10, -25, -37 },
				//EmptyRowStaffle = 10,
				EmptyRows = new int[] { 86, 72, 62, 50, 49, 46, 41, 38, 35, 34, 32, 28, 27, 25, 25, 25, 19, 17, 17, 11, 11, 5 },
				Unreachables = new int[] { -1, -11, -12, -14, -19, -30, -30, -31, -35, -38, -39, -42, -43, -43, -45, -47, -57, -70, -72, -75, -75, -101 },
			};
			return pars.Calc();
		}
	}
}
