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

			I0 = new ParamCurve();
			I1 = new ParamCurve();
			I2 = new ParamCurve();
			I3 = new ParamCurve();
			I4 = new ParamCurve();

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
			Delta = new ParamCurve();
			Groups = new int[6];
			SingleGroupBonus = new int[4];
			SingleEmpties = new int[6];
		}


		public int[] PointsCalc { get { return m_Points; } }
		private int[] m_Points;

		public int[] ComboCalc { get { return m_Combo; } }
		private int[] m_Combo;

		public int[] I0Calc { get { return m_I0; } }
		public int[] I1Calc { get { return m_I1; } }
		public int[] I2Calc { get { return m_I2; } }
		public int[] I3Calc { get { return m_I3; } }
		public int[] I4Calc { get { return m_I4; } }
		
		private int[] m_I0;
		private int[] m_I1;
		private int[] m_I2;
		private int[] m_I3;
		private int[] m_I4;


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

		public int[] DeltaCalc { get { return m_DeltaCalc; } }
		private int[] m_DeltaCalc;
		
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

		/// <summary>Points for a potential Tetris, triple, double and single.</summary>
		[ParameterType(ParameterType.Ascending)]
		public ParamCurve I0 { get; set; }
		public ParamCurve I1 { get; set; }
		public ParamCurve I2 { get; set; }
		public ParamCurve I3 { get; set; }
		public ParamCurve I4 { get; set; }

		public ParamCurve TSpinSingle0Potential { get; set; }
		public ParamCurve TSpinSingle1Potential { get; set; }
		public ParamCurve TSpinDoublePotential { get; set; }
		public ParamCurve TDoubleClearPotential { get; set; }
		public ParamCurve EmptyRows { get; set; }
		public ParamCurve Skips { get; set; }
		public ParamCurve PerfectClearPotential { get; set; }
		public ParamCurve Delta { get; set; }

		public EvaluatorParameters Calc()
		{
			m_Points = Points.Calculate();
			m_Combo = Combo.Calculate();

			m_I0 = I0.Calculate();
			m_I1 = I1.Calculate();
			m_I2 = I2.Calculate();
			m_I3 = I3.Calculate();
			m_I4 = I4.Calculate();

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
			m_DeltaCalc = Delta.Calculate();

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
			// Elo: 1620, Runs:   810 (54.9%, PT: 0.748, #: 47.9, T1: 5.18%, T2: 24.48%, I4: 2.00%, CL: 0.74%), ID: 15340, Parent: 15295, Gen: 128, Ply: 3
			{
				//PointsCalc = new int[] { 46, 47, 47, 48, 48, 49, 49, 50, 50, 51, 51, 52, 52, 53, 53, 54, 54, 55, 55, 55, 56, 56 },
				//ComboCalc = new int[] { -4, -4, -3, -3, -2, -1, -1, 0, 0, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9 },
				//I0Calc = new int[] { 64, 58, 53, 48, 44, 39, 35, 31, 27, 23, 20, 16, 12, 9, 5, 2, -2, -5, -9, -12, -16, -19 },
				//I1Calc = new int[] { -23, -18, -16, -14, -12, -10, -8, -7, -5, -4, -2, -1, 0, 1, 2, 4, 5, 6, 7, 8, 9, 10 },
				//I2Calc = new int[] { 30, 28, 24, 20, 14, 8, 1, -6, -14, -22, -31, -40, -50, -60, -71, -81, -93, -104, -116, -128, -141, -154 },
				//I3Calc = new int[] { 0, 3, 5, 7, 9, 10, 12, 14, 15, 17, 18, 20, 21, 22, 24, 25, 26, 28, 29, 30, 31, 33 },
				//I4Calc = new int[] { 31, 29, 27, 26, 25, 24, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 8 },
				//HolesReachableCalc = new int[] { -64, -61, -59, -56, -54, -52, -49, -47, -45, -43, -41, -39, -37, -35, -33, -31, -29, -27, -25, -23, -21, -19 },
				//HolesUnreachableCalc = new int[] { -67, -64, -62, -61, -59, -58, -56, -55, -54, -52, -51, -50, -49, -48, -47, -45, -44, -43, -42, -41, -40, -39 },
				//UnreachableRowsCalc = new int[] { -60, -8, 37, 79, 121, 161, 200, 239, 277, 314, 351, 388, 425, 461, 497, 532, 568, 603, 638, 672, 707, 741 },
				//TSpinSingle0PotentialCalc = new int[] { 14, 13, 12, 12, 11, 10, 10, 9, 8, 8, 7, 7, 6, 5, 5, 4, 4, 3, 3, 2, 1, 1 },
				//TSpinSingle1PotentialCalc = new int[] { -32, -32, -32, -32, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33, -33 },
				//TSpinDoublePotentialCalc = new int[] { 10, 17, 27, 37, 47, 59, 70, 82, 94, 106, 119, 131, 144, 157, 170, 184, 197, 211, 225, 239, 253, 267 },
				//TDoubleClearPotentialCalc = new int[] { 56, 54, 53, 51, 50, 49, 48, 46, 45, 44, 43, 42, 41, 40, 38, 37, 36, 35, 34, 33, 32, 31 },
				//EmptyRowsCalc = new int[] { 19, 20, 20, 20, 21, 21, 21, 22, 22, 22, 22, 23, 23, 23, 23, 24, 24, 24, 25, 25, 25, 25 },
				//SingleEmptiesCalc = new int[] { 0, -3, -8, -99, -232, -530 },
				//SkipsCalc = new int[] { 16, 15, 13, 12, 11, 9, 8, 7, 6, 5, 4, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7, -9 },
				//PerfectClearPotentialCalc = new int[] { -27, -24, -22, -19, -16, -13, -10, -7, -4, -1, 1, 4, 7, 10, 13, 16, 19, 22, 26, 29, 32, 35 },
				//DeltaCalc = new int[] { 2, 1, 0, 0, -1, -1, -2, -2, -3, -4, -4, -5, -5, -6, -6, -6, -7, -7, -8, -8, -9, -9 },
				SingleEmpties = new int[] { -3, -3, -4, -33, -58, -106 },
				SingleGroupBonus = new int[] { 20, 30, 29, 43 },
				Groups = new int[] { 50, 4, -22, -47, -62, -182 },
				Points = new ParamCurve(46, 59, 0.982464273658126, 0.923865588473537),
				Combo = new ParamCurve(-4, 12, 1.17931873106475, 0.931724573249337),
				HolesReachable = new ParamCurve(-64, 1, 1.01779355459707, 0.880127609895466),
				HolesUnreachable = new ParamCurve(-67, -15, 0.964614208435645, 0.787239021236976),
				UnreachableRows = new ParamCurve(-60, 519, 0.791688003323969, 1.13480606727193),
				I0 = new ParamCurve(64, -13, 0.821491520087516, 1.03024631174621),
				I1 = new ParamCurve(-23, 39, 0.848418802935529, 0.756226554546666),
				I2 = new ParamCurve(30, -22, 1.07058355598567, 1.38699799980154),
				I3 = new ParamCurve(0, 39, 0.851792399093953, 0.930326619511863),
				I4 = new ParamCurve(31, -7, 0.964958965411296, 0.835642914806356),
				TSpinSingle0Potential = new ParamCurve(14, -13, 1.0953695677393, 0.781692313346563),
				TSpinSingle1Potential = new ParamCurve(-32, -35, 0.831301937725097, 0.704934481905502),
				TSpinDoublePotential = new ParamCurve(10, 188, 1.0404560030663, 1.11573564793918),
				TDoubleClearPotential = new ParamCurve(56, 37, 0.784760977555421, 1.11526331817929),
				EmptyRows = new ParamCurve(19, 31, 1.02858524676715, 0.792250941629362),
				Skips = new ParamCurve(16, -11, 0.955798926237415, 0.967247369135811),
				PerfectClearPotential = new ParamCurve(-27, 23, 0.973177443404497, 1.07076601949121),
				Delta = new ParamCurve(2, -13, 0.929580807001583, 0.896832377666578),
			};
			return pars.Calc();
		}
	}
}
