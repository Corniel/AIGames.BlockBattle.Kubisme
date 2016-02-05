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
			// Elo: 1615, Runs:  2335 (53.0%, PT: 0.724, #: 46.1, T1: 4.57%, T2: 28.85%, I4: 0.88%, CL: 0.77%), ID: 6649, Parent: 6520, Gen: 37
			{
				//PointsCalc = new int[] { 39, 39, 39, 39, 39, 38, 38, 38, 38, 38, 38, 38, 38, 37, 37, 37, 37, 37, 37, 37, 37, 36 },
				//ComboCalc = new int[] { -2, 0, 2, 5, 7, 10, 12, 14, 17, 19, 21, 24, 26, 29, 31, 34, 36, 38, 41, 43, 46, 48 },
				//I0Calc = new int[] { 8, 4, 1, -2, -4, -7, -9, -11, -14, -16, -18, -20, -22, -24, -26, -28, -30, -32, -34, -35, -37, -39 },
				//I1Calc = new int[] { -52, -51, -50, -49, -48, -46, -45, -44, -42, -41, -40, -38, -37, -35, -34, -32, -31, -29, -28, -26, -25, -23 },
				//I2Calc = new int[] { -31, -32, -32, -33, -34, -35, -36, -36, -37, -38, -39, -39, -40, -41, -42, -42, -43, -44, -45, -45, -46, -47 },
				//I3Calc = new int[] { 11, 12, 12, 13, 13, 14, 14, 15, 15, 15, 16, 16, 17, 17, 17, 18, 18, 19, 19, 19, 20, 20 },
				//I4Calc = new int[] { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
				//HolesReachableCalc = new int[] { -22, -19, -18, -18, -17, -16, -16, -15, -15, -15, -14, -14, -14, -13, -13, -13, -12, -12, -12, -12, -11, -11 },
				//HolesUnreachableCalc = new int[] { -53, -45, -41, -38, -35, -33, -31, -29, -28, -26, -25, -23, -22, -20, -19, -18, -17, -15, -14, -13, -12, -11 },
				//UnreachableRowsCalc = new int[] { -14, -14, -14, -13, -13, -13, -13, -13, -13, -13, -13, -12, -12, -12, -12, -12, -12, -12, -12, -12, -11, -11 },
				//TSpinSingle0PotentialCalc = new int[] { 4, 5, 6, 7, 8, 8, 9, 10, 10, 11, 12, 12, 13, 14, 14, 15, 16, 16, 17, 18, 18, 19 },
				//TSpinSingle1PotentialCalc = new int[] { -29, -27, -26, -24, -23, -21, -19, -18, -16, -15, -13, -12, -10, -9, -7, -6, -4, -3, -1, 0, 2, 3 },
				//TSpinDoublePotentialCalc = new int[] { -28, 13, 32, 47, 60, 71, 82, 91, 100, 109, 117, 125, 132, 139, 146, 153, 160, 166, 172, 178, 184, 190 },
				//TDoubleClearPotentialCalc = new int[] { -18, 21, 34, 43, 51, 57, 63, 68, 73, 78, 82, 86, 90, 94, 97, 100, 103, 107, 110, 112, 115, 118 },
				//EmptyRowsCalc = new int[] { -61, -13, 6, 21, 33, 43, 53, 62, 70, 78, 85, 92, 98, 104, 110, 116, 122, 127, 132, 137, 142, 147 },
				//SingleEmptiesCalc = new int[] { 0, -8, -46, -105, -328, -560 },
				//SkipsCalc = new int[] { 14, 18, 22, 25, 28, 31, 34, 36, 39, 41, 44, 46, 49, 51, 54, 56, 58, 61, 63, 65, 67, 70 },
				//PerfectClearPotentialCalc = new int[] { 19, 20, 20, 20, 21, 21, 22, 22, 23, 23, 24, 24, 24, 25, 25, 26, 26, 26, 27, 27, 28, 28 },
				SingleEmpties = new int[] { -4, -8, -23, -35, -82, -112 },
				SingleGroupBonus = new int[] { 9, 15, 13, 19 },
				Groups = new int[] { 25, 9, -25, -40, -43, -52 },
				Points = new ParamCurve(39, 37, 0.978751950550506, 1.0872901634181),
				Combo = new ParamCurve(-2, 54, 1.06239757236192, 0.965990135534836),
				HolesReachable = new ParamCurve(-22, -10, 0.495423531301773, 0.934261354296262),
				HolesUnreachable = new ParamCurve(-53, -12, 0.518868607592336, 1.01455373253663),
				UnreachableRows = new ParamCurve(-14, -11, 0.876383032829627, 0.960272202883828),
				I0 = new ParamCurve(8, -44, 0.835291351514647, 0.959971191778264),
				I1 = new ParamCurve(-52, -29, 1.05313681783702, 1.06808225640425),
				I2 = new ParamCurve(-31, -51, 1.08212834573541, 0.930424773987383),
				I3 = new ParamCurve(11, 24, 0.993132630081788, 0.888425761477171),
				I4 = new ParamCurve(16, 16, 0.959745035270708, 0.857341108564648),
				TSpinSingle0Potential = new ParamCurve(4, 26, 0.997375629202244, 0.870630344590973),
				TSpinSingle1Potential = new ParamCurve(-29, 11, 1.04241327457591, 0.932774796883662),
				TSpinDoublePotential = new ParamCurve(-28, 194, 0.553907927652281, 0.98846162484164),
				TDoubleClearPotential = new ParamCurve(-18, 155, 0.49046582143932, 0.838144749686831),
				EmptyRows = new ParamCurve(-61, 154, 0.491188092007989, 0.978213989442313),
				Skips = new ParamCurve(14, 73, 0.853096937292526, 0.977814281274923),
				PerfectClearPotential = new ParamCurve(19, 27, 0.884144165743302, 1.04151839690489),
			};
			return pars.Calc();
		}
	}
}
