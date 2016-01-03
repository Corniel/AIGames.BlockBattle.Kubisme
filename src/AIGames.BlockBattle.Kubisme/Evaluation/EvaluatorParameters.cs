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
			TSpinSinglePotential = new ParamCurve();
			TSpinDoublePotential = new ParamCurve();
			TDoubleClearPotential = new ParamCurve();
			EmptyRows = new ParamCurve();
			HolesReachable = new ParamCurve();
			HolesUnreachable = new ParamCurve();
			Skips = new ParamCurve();
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			SingleEmpties = new int[6];
		}


		public int[] TSpinSinglePotentialCalc { get { return m_TSpinSinglePotential; } }
		private int[] m_TSpinSinglePotential;
		public int[] TSpinDoublePotentialCalc { get { return m_TSpinDoublePotential; } }
		private int[] m_TSpinDoublePotential;

		public int[] TDoubleClearPotentialCalc { get { return m_TDoubleClearPotential; } }
		private int[] m_TDoubleClearPotential;

		public int[] EmptyRowsCalc { get { return m_EmptyRows; } }
		private int[] m_EmptyRows;

		public int[] HolesReachableCalc { get { return m_HolesReachable; } }
		private int[] m_HolesReachable;

		public int[] HolesUnreachableCalc { get { return m_HolesUnreachable; } }
		private int[] m_HolesUnreachable;

		public int[] SingleEmptiesCalc { get { return m_SingleEmpties; } }
		private int[] m_SingleEmpties;

		public int[] SkipsCalc { get { return m_SkipsCalc; } }
		private int[] m_SkipsCalc;

		/// <summary>Factor for current combo's.</summary>
		public int Combo { get; set; }
		
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		

		[ParameterType(ParameterType.Positive)]
		public int PerfectClearPotential { get; set; }

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

		[ParameterType(ParameterType.Negative)]
		public int UnreachableRow { get; set; }


		/// <summary>Get the score per reachable hole.</summary>
		public ParamCurve HolesReachable { get; set; }
		/// <summary>Get the score per unreachable hole.</summary>
		public ParamCurve HolesUnreachable { get; set; }

		public ParamCurve TSpinSinglePotential { get; set; }
		public ParamCurve TSpinDoublePotential { get; set; }
		public ParamCurve TDoubleClearPotential { get; set; }
		public ParamCurve EmptyRows { get; set; }
		public ParamCurve Skips { get; set; }
		
		public EvaluatorParameters Calc()
		{
			m_EmptyRows = EmptyRows.Calculate(22);
			m_TSpinSinglePotential = TSpinSinglePotential.Calculate(22);
			m_TSpinDoublePotential = TSpinDoublePotential.Calculate(22);
			m_TDoubleClearPotential = TDoubleClearPotential.Calculate(22);
			m_HolesReachable = HolesReachable.Calculate(22);
			m_HolesUnreachable = HolesUnreachable.Calculate(22);
			m_SkipsCalc = Skips.Calculate(22);

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
				Combo = 26,
				HolesReachable = new ParamCurve(- 47),
				HolesUnreachable = new ParamCurve(-104),
				Points = 130,
				Skips = new ParamCurve(-313),
				TSpinDoublePotential = new ParamCurve(500, 600, 1),
				PerfectClearPotential = 727,
				TetrisPotential = new int[] { -68, -52, 17, 19, 59 },
				SingleEmpties = new int[] { -2, -11, -30, -30, -45, -98 },
				SingleGroupBonus = new int[] { 36, 41, 54, 1 },
				Groups = new int[] { 15, 12, -24, -50, -52, -60 },
				EmptyRows = new ParamCurve(0, 1000, 1.8),
			};

			return pars.Calc();
		}
	}
}
