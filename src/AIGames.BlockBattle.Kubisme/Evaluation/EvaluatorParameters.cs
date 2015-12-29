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

		/// <summary>When to use the endgame instead of the default parameters.</summary>
		/// <remarks>Only the value for default parameters are taken into account.</remarks>
		public int Endgame { get; set; }

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
			// Elo: 1615, Avg: 0.755, Runs: 2798, ID: 13084, Parent: 12961, Gen: 151
			{
				//EmptyRowsCalc = new int[] { 0, 185, 348, 452, 539, 614, 687, 750, 794, 836, 875, 912, 947, 981, 1013, 1035, 1051, 1064, 1074, 1083, 1091, 1098 },
				//UnreachableRowsCalc = new int[] { 0, -237, -483, -733, -987, -1245, -1509, -1780, -2063, -2351, -2646, -2949, -3253, -3566, -3880, -4210, -4547, -4887, -5236, -5608, -5999, -6395 },
				//SingleEmptiesCalc = new int[] { 0, -34, -94, -399, -600, -900 },
				//Combo = 69,
				//EmptyRowStaffle = 0,
				HolesUnreachable = -118,
				HolesReachable = -118,
				Points = 161,
				Skips = 373,
				TSpinPontential = 548,
				SingleEmpties = new int[] { -26, -34, -47, -133, -150, -180 },
				SingleGroupBonus = new int[] { 131, 31, 1, 216 },
				Groups = new int[] { 0, -4, -83, -163, -168, -173 },
				EmptyRows = new int[] { 185, 163, 104, 87, 75, 73, 63, 44, 42, 39, 37, 35, 34, 32, 22, 16, 13, 10, 9, 8, 7, 5 },
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
