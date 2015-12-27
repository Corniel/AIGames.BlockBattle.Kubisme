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
			// Elo: 1615, Avg: 0.755, Runs: 2798, ID: 13084, Parent: 12961, Gen: 151
			{
				//EmptyRowsCalc = new int[] { 0, 185, 348, 452, 539, 614, 687, 750, 794, 836, 875, 912, 947, 981, 1013, 1035, 1051, 1064, 1074, 1083, 1091, 1098 },
				//UnreachableRowsCalc = new int[] { 0, -237, -483, -733, -987, -1245, -1509, -1780, -2063, -2351, -2646, -2949, -3253, -3566, -3880, -4210, -4547, -4887, -5236, -5608, -5999, -6395 },
				//SingleEmptiesCalc = new int[] { 0, -34, -94, -399, -600, -900 },
				//Combo = 69,
				//EmptyRowStaffle = 0,
				Holes = -118,
				Points = 161,
				Combos = new int[] { 69, 52, 46, 36, 21, 16, 8, 3, -8, -14, -33, -37, -49, -51, -62, -68, -75, -83, -103, -108, -113, -133 },
				Skips = 373,
				DoublePotentialJLT = 50,
				DoublePotentialTSZ = 207,
				DoublePotentialO = 100,
				DoublePotentialI = 16,
				TriplePotentialJL = 8,
				TriplePotentialI = 62,
				TetrisPotential = 390,
				TSpinPontential = 548,
				SingleEmpties = new int[] { -26, -34, -47, -133, -150, -180 },
				SingleGroupBonus = new int[] { 131, 31, 1, 216 },
				Groups = new int[] { 0, -4, -83, -163, -168, -173 },
				EmptyRows = new int[] { 185, 163, 104, 87, 75, 73, 63, 44, 42, 39, 37, 35, 34, 32, 22, 16, 13, 10, 9, 8, 7, 5 },
				Unreachables = new int[] { -1, -10, -14, -18, -22, -28, -35, -47, -52, -59, -67, -68, -77, -78, -94, -101, -104, -113, -136, -155, -160, -162 },
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
				//EmptyRowsCalc = new int[] { 0,178,352,505,649,789,912,1034,1147,1257,1359,1459,1555,1644,1731,1816,1889,1961,2032,2101,2146,2190 },
				//UnreachableRowsCalc = new int[] { 0,-271,-579,-899,-1222,-1551,-1884,-2232,-2581,-2936,-3293,-3656,-4022,-4399,-4779,-5159,-5554,-5951,-6353,-6799,-7266,-7735 },
				//SingleEmptiesCalc = new int[] { 0,-61,-220,-348,-548,-865 },
				//Combo = 104,
				//EmptyRowStaffle = 22,
				Holes = -135,
				Points = 201,
				Combos = new int[] { 104, 38, 24, 19, 18, 14, 13, 13, -2, -9, -14, -16, -17, -24, -27, -35, -35, -36, -50, -62, -95, -117 },
				Skips = 1,
				DoublePotentialJLT = 2,
				DoublePotentialTSZ = 26,
				DoublePotentialO = 19,
				DoublePotentialI = 65,
				TriplePotentialJL = 94,
				TriplePotentialI = 1,
				TetrisPotential = 1,
				TSpinPontential = 74,
				SingleEmpties = new int[] { -47, -61, -110, -116, -137, -173 },
				SingleGroupBonus = new int[] { 45, 8, 33, 68 },
				Groups = new int[] { 22, -13, -80, -109, -124, -142 },
				EmptyRows = new int[] { 156, 152, 131, 122, 118, 101, 100, 91, 88, 80, 78, 74, 67, 65, 63, 51, 50, 49, 47, 23, 22, 4 },
				Unreachables = new int[] { -1, -38, -50, -53, -59, -63, -78, -79, -85, -87, -93, -96, -107, -110, -110, -125, -127, -132, -176, -197, -199, -200 },
			};
			return pars.Calc();
		}
	}
}
