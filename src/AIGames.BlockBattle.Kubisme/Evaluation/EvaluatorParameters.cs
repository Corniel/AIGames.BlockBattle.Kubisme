using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			Points = new ParamCurve();
			Combo = new ParamCurve();

			TSpinSingle0Potential = new ParamCurve();
			TSpinSingle1Potential = new ParamCurve();
			TSpinDoublePotential = new ParamCurve();
			TDoubleClearPotential = new ParamCurve();
			EmptyRows = new ParamCurve();

			UnreachableRows = new ParamCurve();
			HolesReachable = new ParamCurve();
			HolesUnreachable = new ParamCurve();
			Skips = new ParamCurve();
			PerfectClearPotential = new ParamCurve();
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			SingleEmpties = new int[6];
		}

		public int[] PointsCalc { get { return m_Points; } }
		private int[] m_Points;

		public int[] ComboCalc { get { return m_Combo; } }
		private int[] m_Combo;

		public int[] HolesReachableCalc { get { return m_HolesReachable; } }
		private int[] m_HolesReachable;

		public int[] HolesUnreachableCalc { get { return m_HolesUnreachable; } }
		private int[] m_HolesUnreachable;

		public int[] UnreachableRowsCalc { get { return m_UnreachableRows; } }
		private int[] m_UnreachableRows;

		public int[] TSpinSingle0PotentialCalc { get { return m_TSpinSingle0Potential; } }
		private int[] m_TSpinSingle0Potential;
		public int[] TSpinSingle1PotentialCalc { get { return m_TSpinSingle1Potential; } }
		private int[] m_TSpinSingle1Potential;
		public int[] TSpinDoublePotentialCalc { get { return m_TSpinDoublePotential; } }
		private int[] m_TSpinDoublePotential;

		public int[] TDoubleClearPotentialCalc { get { return m_TDoubleClearPotential; } }
		private int[] m_TDoubleClearPotential;

		public int[] EmptyRowsCalc { get { return m_EmptyRows; } }
		private int[] m_EmptyRows;


		public int[] SingleEmptiesCalc { get { return m_SingleEmpties; } }
		private int[] m_SingleEmpties;

		public int[] SkipsCalc { get { return m_SkipsCalc; } }
		private int[] m_SkipsCalc;

		public int[] PerfectClearPotentialCalc { get { return m_PerfectClearPotential; } }
		private int[] m_PerfectClearPotential;

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

		/// <summary>Factor for current points.</summary>
		public ParamCurve Points { get; set; }

		/// <summary>Factor for current combo's.</summary>
		public ParamCurve Combo { get; set; }
		/// <summary>Get the score per reachable hole.</summary>
		public ParamCurve HolesReachable { get; set; }
		/// <summary>Get the score per unreachable hole.</summary>
		public ParamCurve HolesUnreachable { get; set; }

		public ParamCurve UnreachableRows { get; set; }
		
		public ParamCurve TSpinSingle0Potential { get; set; }
		public ParamCurve TSpinSingle1Potential { get; set; }
		public ParamCurve TSpinDoublePotential { get; set; }
		public ParamCurve TDoubleClearPotential { get; set; }
		public ParamCurve EmptyRows { get; set; }
		public ParamCurve Skips { get; set; }
		public ParamCurve PerfectClearPotential { get; set; }

		public EvaluatorParameters Calc()
		{
			m_Points = Points.Calculate();
			m_Combo = Combo.Calculate();
			
			m_UnreachableRows = UnreachableRows.Calculate();
			m_EmptyRows = EmptyRows.Calculate();
			m_TSpinSingle0Potential = TSpinSingle0Potential.Calculate();
			m_TSpinSingle1Potential = TSpinSingle1Potential.Calculate();
			m_TSpinDoublePotential = TSpinDoublePotential.Calculate();
			m_TDoubleClearPotential = TDoubleClearPotential.Calculate();
			m_HolesReachable = HolesReachable.Calculate();
			m_HolesUnreachable = HolesUnreachable.Calculate();
			m_SkipsCalc = Skips.Calculate();
			m_PerfectClearPotential = PerfectClearPotential.Calculate();

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
			// Elo: 1628, Runs:  1128 (54.7%, PT: 0.645, #: 52.5, T1: 4.48%, T2: 19.93%, I4: 1.08%, CL: 0.27%), ID: 5996, Parent: 5963, Gen: 60
			{
				//PointsCalc = new[] { 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 },
				//ComboCalc = new[] { -10, -9, -9, -8, -7, -6, -5, -5, -4, -3, -2, -1, -1, 0, 1, 2, 3, 4, 4, 5, 6, 7 },
				//HolesReachableCalc = new[] { -22, -22, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -24, -24, -24, -24, -24 },
				//HolesUnreachableCalc = new[] { -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40 },
				//UnreachableRowsCalc = new[] { -163, -164, -165, -166, -166, -166, -167, -167, -167, -168, -168, -168, -168, -168, -168, -169, -169, -169, -169, -169, -169, -169 },
				//TSpinSingle0PotentialCalc = new[] { 44, 44, 44, 43, 43, 43, 43, 43, 43, 43, 43, 43, 43, 43, 43, 42, 42, 42, 42, 42, 42, 42 },
				//TSpinSingle1PotentialCalc = new[] { -26, -26, -26, -26, -26, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25 },
				//TSpinDoublePotentialCalc = new[] { 4, 6, 10, 14, 21, 28, 37, 48, 59, 72, 86, 102, 119, 137, 156, 177, 199, 223, 247, 273, 300, 329 },
				//TDoubleClearPotentialCalc = new[] { -5, -4, -2, 0, 3, 7, 11, 15, 21, 26, 33, 40, 47, 55, 64, 73, 82, 93, 103, 115, 126, 139 },
				//EmptyRowsCalc = new[] { 72, 97, 115, 128, 140, 149, 158, 166, 174, 180, 187, 193, 198, 204, 209, 214, 218, 223, 227, 231, 235, 239 },
				//SingleEmptiesCalc = new[] { 0, -5, -16, -45, -88, -775 },
				//SkipsCalc = new[] { 31, 32, 33, 35, 37, 39, 41, 43, 45, 47, 49, 52, 54, 57, 59, 62, 65, 68, 70, 73, 76, 79 },
				//PerfectClearPotentialCalc = new[] { 21, 21, 21, 21, 20, 20, 20, 20, 20, 20, 19, 19, 19, 19, 19, 19, 18, 18, 18, 18, 18, 18 },
				SingleEmpties = new[] { -1, -5, -8, -15, -22, -155 },
				SingleGroupBonus = new[] { 5, 8, 10, 23 },
				Groups = new[] { 175, 2, -17, -22, -25, -43 },
				Points = new ParamCurve(-0.0348845052532844, 0.807192282000627, 32),
				Combo = new ParamCurve(0.789707682281733, 1.00720274960622, -11),
				HolesReachable = new ParamCurve(-0.314208458736542, 0.551513755321503, -22),
				HolesUnreachable = new ParamCurve(-1.23040157882497, -0.0253896249458187, -39),
				UnreachableRows = new ParamCurve(-10.3059426610358, 0.149115448165686, -153),
				TSpinSingle0Potential = new ParamCurve(-0.208672148827463, 0.728266607690604, 44),
				TSpinSingle1Potential = new ParamCurve(0.123828209564089, 0.783466258551927, -26),
				TSpinDoublePotential = new ParamCurve(0.751577877998359, 1.96411220281889, 3),
				TDoubleClearPotential = new ParamCurve(0.371645939070732, 1.92727278741553, -5),
				EmptyRows = new ParamCurve(105.349659391363, 0.306601862702519, -33),
				Skips = new ParamCurve(0.802334840968254, 1.33037481382489, 30),
				PerfectClearPotential = new ParamCurve(-0.535941140167415, 0.664062715880575, 22),
			};
			return pars.Calc();
		}
	}
}
