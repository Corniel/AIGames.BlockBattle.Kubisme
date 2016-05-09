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

			RowsDelta = new ParamCurve();
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

		public int[] RowsDeltaCalc { get { return m_RowsDelta; } }
		private int[] m_RowsDelta;

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

		/// <summary>The difference between the first filled of own and opponent.</summary>
		/// <remarks>
		/// When we are more then 4 ahead, we use the value of +4;
		/// </remarks>
		public ParamCurve RowsDelta { get; set; }

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
			m_RowsDelta = RowsDelta.Calculate(ParamCurve.DefaultLength + 4);

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
			// Elo: 1622, Runs:  1438 (53.2%, PT: 0.668, #: 51.5, T1: 4.42%, T2: 18.83%, I4: 2.11%, CL: 0.21%), ID: 22945, Parent: 22738, Gen: 132
			{
				//RowsDeltaCalc = new[] { -55, -58, -61, -63, -64, -66, -67, -69, -70, -71, -72, -73, -74, -75, -76, -77, -78, -79, -79, -80, -81, -82, -82, -83, -84, -84 },
				//PointsCalc = new[] { 59, 58, 57, 56, 54, 53, 51, 49, 46, 44, 41, 39, 36, 33, 30, 26, 23, 19, 16, 12, 8, 4 },
				//ComboCalc = new[] { -29, -30, -31, -32, -33, -34, -35, -37, -38, -39, -41, -42, -44, -45, -47, -48, -50, -52, -53, -55, -57, -59 },
				//I0Calc = new[] { -20, -19, -19, -19, -18, -18, -18, -17, -17, -17, -17, -16, -16, -16, -15, -15, -15, -15, -14, -14, -14, -13 },
				//I1Calc = new[] { -38, -37, -36, -35, -34, -33, -32, -31, -30, -29, -28, -27, -26, -25, -24, -23, -22, -21, -20, -19, -18, -17 },
				//I2Calc = new[] { -16, -16, -16, -15, -15, -14, -14, -13, -13, -12, -11, -10, -9, -8, -6, -5, -4, -2, -1, 1, 3, 5 },
				//I3Calc = new[] { 1, -1, -3, -5, -7, -9, -11, -12, -14, -16, -18, -19, -21, -23, -25, -26, -28, -30, -31, -33, -35, -36 },
				//I4Calc = new[] { 29, 29, 29, 29, 29, 29, 29, 29, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 },
				//HolesReachableCalc = new[] { -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26 },
				//HolesUnreachableCalc = new[] { -38, -39, -39, -39, -39, -40, -40, -40, -40, -40, -40, -40, -40, -41, -41, -41, -41, -41, -41, -41, -41, -41 },
				//UnreachableRowsCalc = new[] { 103, 104, 106, 108, 110, 113, 116, 119, 122, 126, 129, 133, 137, 141, 145, 149, 153, 158, 163, 167, 172, 177 },
				//TSpinSingle0PotentialCalc = new[] { 47, 47, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 49, 49, 49, 49, 49, 49, 49, 49, 49, 49 },
				//TSpinSingle1PotentialCalc = new[] { -10, -10, -10, -11, -11, -11, -11, -12, -12, -12, -12, -13, -13, -13, -13, -13, -13, -14, -14, -14, -14, -14 },
				//TSpinDoublePotentialCalc = new[] { 35, 37, 40, 44, 49, 55, 62, 69, 77, 85, 94, 104, 115, 126, 138, 150, 163, 176, 190, 205, 220, 236 },
				//TDoubleClearPotentialCalc = new[] { 21, 22, 22, 23, 25, 27, 29, 31, 34, 37, 40, 44, 47, 52, 56, 61, 67, 72, 78, 84, 91, 98 },
				//EmptyRowsCalc = new[] { -8, 22, 43, 59, 73, 85, 96, 105, 114, 123, 130, 137, 144, 151, 157, 163, 168, 174, 179, 184, 189, 194 },
				//SingleEmptiesCalc = new[] { 0, -5, -20, -123, -604, -1115 },
				//SkipsCalc = new[] { 29, 29, 30, 31, 31, 32, 33, 34, 36, 37, 39, 40, 42, 44, 46, 48, 50, 52, 54, 57, 59, 62 },
				//PerfectClearPotentialCalc = new[] { 36, 36, 36, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35 },
				SingleEmpties = new[] { -1, -5, -10, -41, -151, -223 },
				SingleGroupBonus = new[] { 8, 11, 21, 16 },
				Groups = new[] { 33, 10, -6, -44, -50, -58 },
				RowsDelta = new ParamCurve(-9.7857523271814, 0.426691958867014, -45),
				Points = new ParamCurve(-0.331773455161601, 1.65499373776286, 59),
				Combo = new ParamCurve(-0.448103449493647, 1.3563450188376, -29),
				HolesReachable = new ParamCurve(-0.723594194930047, -0.153819699399173, -26),
				HolesUnreachable = new ParamCurve(-1.27539118248969, 0.390544179640711, -37),
				UnreachableRows = new ParamCurve(0.794327462930232, 1.47243836661801, 102),
				I0 = new ParamCurve(0.353587734699249, 0.944901988655329, -20),
				I1 = new ParamCurve(0.938166968338192, 1.0201384909451, -39),
				I2 = new ParamCurve(0.0419399619102478, 2.00637665372342, -16),
				I3 = new ParamCurve(-2.24261474935338, 0.927209151443097, 3),
				I4 = new ParamCurve(0.411012839525938, 0.0933502649888402, 29),
				TSpinSingle0Potential = new ParamCurve(0.286975296121091, 0.657752170134337, 47),
				TSpinSingle1Potential = new ParamCurve(-0.600265072938055, 0.709926800057293, -9),
				TSpinDoublePotential = new ParamCurve(0.933030793443324, 1.73932215953246, 34),
				TDoubleClearPotential = new ParamCurve(0.149940008856356, 2.01733793446689, 21),
				EmptyRows = new ParamCurve(123.968100643668, 0.31243646396324, -132),
				Skips = new ParamCurve(0.136069554369896, 1.77642528722063, 29),
				PerfectClearPotential = new ParamCurve(-0.226618608832359, 0.594814163073898, 36),
			};
			return pars.Calc();
		}
	}
}
