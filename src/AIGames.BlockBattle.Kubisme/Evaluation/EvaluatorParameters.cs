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
			// Elo: 1612, Runs:  2300 (52.7%, PT: 0.670, #: 46.8, T1: 6.48%, T2: 22.12%, I4: 1.27%, CL: 0.78%), ID: 2773, Parent: 2751, Gen: 31
			{
				//PointsCalc = new int[] { 139, 139, 139, 139, 139, 139, 139, 139, 139, 139, 139, 138, 138, 138, 138, 138, 138, 138, 138, 138, 138, 138 },
				//ComboCalc = new int[] { 5, 5, 6, 7, 7, 8, 9, 10, 11, 12, 12, 13, 14, 16, 17, 18, 19, 20, 21, 22, 23, 25 },
				//I0Calc = new int[] { -118, -126, -133, -140, -147, -154, -160, -166, -173, -179, -185, -191, -197, -203, -209, -215, -221, -227, -233, -239, -245, -250 },
				//I1Calc = new int[] { -137, -132, -128, -125, -121, -118, -115, -112, -108, -105, -102, -99, -97, -94, -91, -88, -85, -82, -80, -77, -74, -72 },
				//I2Calc = new int[] { -52, -52, -52, -52, -52, -53, -53, -53, -53, -53, -53, -53, -54, -54, -54, -54, -54, -55, -55, -55, -55, -55 },
				//I3Calc = new int[] { 0, -2, -4, -5, -7, -8, -9, -10, -11, -12, -13, -15, -16, -17, -18, -19, -19, -20, -21, -22, -23, -24 },
				//I4Calc = new int[] { 173, 175, 176, 178, 180, 182, 184, 186, 188, 190, 192, 194, 197, 199, 201, 203, 205, 208, 210, 212, 214, 216 },
				//HolesReachableCalc = new int[] { -123, -108, -99, -91, -85, -79, -73, -68, -63, -58, -53, -49, -44, -40, -36, -32, -28, -24, -20, -17, -13, -9 },
				//HolesUnreachableCalc = new int[] { -183, -155, -145, -137, -131, -126, -121, -117, -113, -109, -105, -102, -99, -96, -93, -90, -87, -85, -82, -80, -77, -75 },
				//UnreachableRowsCalc = new int[] { -20, -20, -19, -19, -19, -18, -18, -18, -17, -17, -17, -16, -16, -16, -15, -15, -14, -14, -14, -13, -13, -13 },
				//TSpinSingle0PotentialCalc = new int[] { -12, 25, 48, 67, 84, 100, 115, 130, 144, 157, 170, 182, 194, 206, 217, 228, 239, 250, 261, 271, 281, 292 },
				//TSpinSingle1PotentialCalc = new int[] { -21, -19, -18, -17, -15, -14, -13, -12, -11, -10, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3 },
				//TSpinDoublePotentialCalc = new int[] { 119, 145, 172, 199, 226, 254, 281, 308, 336, 363, 390, 418, 445, 473, 500, 528, 555, 583, 610, 638, 666, 693 },
				//TDoubleClearPotentialCalc = new int[] { 83, 95, 104, 112, 119, 126, 132, 138, 144, 150, 156, 161, 166, 172, 177, 182, 187, 192, 197, 202, 206, 211 },
				//EmptyRowsCalc = new int[] { 78, 278, 385, 473, 550, 619, 684, 744, 802, 856, 909, 959, 1008, 1055, 1100, 1145, 1188, 1231, 1272, 1313, 1352, 1391 },
				//SingleEmptiesCalc = new int[] { 0, -10, -78, -246, -364, -1295 },
				//SkipsCalc = new int[] { 8, 46, 72, 95, 117, 137, 156, 174, 192, 209, 226, 242, 258, 274, 290, 305, 320, 334, 349, 363, 377, 391 },
				//PerfectClearPotentialCalc = new int[] { 626, 606, 591, 578, 566, 554, 543, 532, 522, 512, 502, 492, 483, 473, 464, 455, 446, 437, 428, 420, 411, 403 },
				SingleEmpties = new int[] { -7, -10, -39, -82, -91, -259 },
				SingleGroupBonus = new int[] { 2, 32, 54, 53 },
				Groups = new int[] { 67, 56, -31, -121, -136, -144 },
				Points = new ParamCurve(139, 138, 0.970666331950408, 0.972896319697802),
				Combo = new ParamCurve(5, 22, 1.25766125246622, 1.03791391670927),
				HolesReachable = new ParamCurve(-123, -19, 0.629513364113739, 1.04611978313002),
				HolesUnreachable = new ParamCurve(-183, -72, 0.451721182246221, 0.979976512702606),
				UnreachableRows = new ParamCurve(-20, -12, 1.05387279979766, 0.970494273781367),
				I0 = new ParamCurve(-118, -241, 0.892971081516056, 1.02704881460232),
				I1 = new ParamCurve(-137, -51, 0.948729689524615, 0.905502489270812),
				I2 = new ParamCurve(-52, -55, 1.16310252351995, 1.01988831519631),
				I3 = new ParamCurve(0, -27, 0.819224948207769, 0.954336154177267),
				I4 = new ParamCurve(173, 214, 1.07230285752719, 1.01783473581288),
				TSpinSingle0Potential = new ParamCurve(-12, 230, 0.618176989342658, 1.12046670239935),
				TSpinSingle1Potential = new ParamCurve(-21, 4, 0.896502781954365, 0.989527753566806),
				TSpinDoublePotential = new ParamCurve(119, 729, 1.03057281890693, 0.980807380301445),
				TDoubleClearPotential = new ParamCurve(83, 235, 0.822979991546495, 0.931939689197822),
				EmptyRows = new ParamCurve(78, 1060, 0.522109886809152, 1.18291539940776),
				Skips = new ParamCurve(8, 430, 0.792415633085224, 0.960092848091289),
				PerfectClearPotential = new ParamCurve(626, 311, 0.904412254835467, 0.875188695335339),
			};
			return pars.Calc();
		}
	}
}
