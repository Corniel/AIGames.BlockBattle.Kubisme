using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			TetrisPotential = new int[5];
			EmptyRows = new int[22];
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			SingleEmpties = new int[6];
		}

		public int[] EmptyRowsCalc { get { return m_EmptyRows; } }
		private int[] m_EmptyRows;

		public int[] SingleEmptiesCalc { get { return m_SingleEmpties; } }
		private int[] m_SingleEmpties;
				
		/// <summary>Factor for current combo's.</summary>
		public int Combo { get; set; }

		public int EmptyRowStaffle { get { return Groups[0]; } }

		/// <summary>Get the score per reachable hole.</summary>
		public int HolesReachable { get; set; }
		
		/// <summary>Get the score per unreachable hole.</summary>
		[ParameterType(ParameterType.Negative)]
		public int HolesUnreachable { get; set; }
		
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int Skips { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int TSpinPontential { get; set; }

		[ParameterType(ParameterType.Positive)]
		public int PerfectClearPontential { get; set; }

		/// <summary>Points for a potential Tetris, triple, double and single.</summary>
		[ParameterType(ParameterType.Ascending)]
		public int[] TetrisPotential { get; set; }

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

		public EvaluatorParameters Calc()
		{
			m_EmptyRows = new int[EmptyRows.Length];

			for (var i = 1; i < m_EmptyRows.Length; i++)
			{
				m_EmptyRows[i] = m_EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRows[i - 1];
				m_EmptyRows[i] += EmptyRowStaffle;
			}
			
			m_SingleEmpties = new int[SingleEmpties.Length];
			for (var i = 0; i < m_SingleEmpties.Length; i++)
			{
				m_SingleEmpties[i] = i * SingleEmpties[i];
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
			// Elo: 1613, Avg: 0.601, Runs: 1363, ID: 3301, Parent: 3284, Gen: 43
			{
				//EmptyRowsCalc = new int[] { 0, 106, 179, 242, 295, 346, 383, 419, 452, 484, 516, 548, 579, 607, 635, 660, 685, 708, 730, 751, 770, 788 },
				//SingleEmptiesCalc = new int[] { 0, -11, -60, -90, -180, -490 },
				//Combo = 1,
				//EmptyRowStaffle = 15,
				HolesReachable = -47,
				HolesUnreachable = -104,
				Points = 130,
				Skips = 313,
				TSpinPontential = 586,
				PerfectClearPontential = 727,
				TetrisPotential = new int[] { -68, -52, 17, 19, 59 },
				SingleEmpties = new int[] { -2, -11, -30, -30, -45, -98 },
				SingleGroupBonus = new int[] { 36, 41, 54, 1 },
				Groups = new int[] { 15, 12, -24, -50, -52, -60 },
				EmptyRows = new int[] { 91, 58, 48, 38, 36, 22, 21, 18, 17, 17, 17, 16, 13, 13, 10, 10, 8, 7, 6, 4, 3, 3 },
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
				HolesUnreachable = -118,
				HolesReachable = -118,
				Points = 201,
				Skips = 1,
				TSpinPontential = 74,
				SingleEmpties = new int[] { -47, -61, -110, -116, -137, -173 },
				SingleGroupBonus = new int[] { 45, 8, 33, 68 },
				Groups = new int[] { 22, -13, -80, -109, -124, -142 },
				EmptyRows = new int[] { 156, 152, 131, 122, 118, 101, 100, 91, 88, 80, 78, 74, 67, 65, 63, 51, 50, 49, 47, 23, 22, 4 },
			};
			return pars.Calc();
		}
	}
}
