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
			// Elo: 1615, Runs:  1318 (53.6%, PT: 0.641, #: 49.3, T1: 5.08%, T2: 16.67%, I4: 2.05%, CL: 0.91%), ID: 16821, Parent: 16776, Gen: 101
			{
				//RowsDeltaCalc = new[] { -24, -24, -23, -23, -23, -22, -22, -22, -22, -21, -21, -21, -20, -20, -20, -20, -19, -19, -19, -18, -18, -18, -18, -17, -17, -17 },
				//PointsCalc = new[] { 61, 60, 59, 58, 56, 54, 52, 50, 47, 45, 42, 38, 35, 32, 28, 24, 20, 16, 11, 7, 2, -3 },
				//ComboCalc = new[] { -6, -6, -7, -7, -7, -8, -9, -9, -10, -10, -11, -12, -13, -14, -14, -15, -16, -17, -18, -19, -20, -21 },
				//I0Calc = new[] { -40, -40, -39, -38, -37, -36, -35, -34, -32, -31, -30, -29, -27, -26, -24, -23, -22, -20, -19, -17, -16, -14 },
				//I1Calc = new[] { -32, -32, -31, -30, -30, -29, -29, -28, -27, -27, -26, -26, -25, -25, -24, -23, -23, -22, -22, -21, -21, -20 },
				//I2Calc = new[] { -54, -53, -53, -52, -51, -49, -48, -46, -44, -42, -40, -38, -35, -32, -29, -26, -23, -20, -16, -13, -9, -5 },
				//I3Calc = new[] { 14, 12, 10, 8, 6, 4, 2, 0, -2, -4, -6, -8, -10, -12, -14, -17, -19, -21, -23, -25, -27, -30 },
				//I4Calc = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
				//HolesReachableCalc = new[] { -26, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25 },
				//HolesUnreachableCalc = new[] { -40, -41, -41, -41, -41, -41, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -43, -43, -43, -43 },
				//UnreachableRowsCalc = new[] { 63, 64, 64, 64, 64, 65, 65, 66, 66, 66, 67, 67, 68, 68, 68, 69, 69, 70, 70, 71, 71, 72 },
				//TSpinSingle0PotentialCalc = new[] { 48, 49, 49, 50, 50, 50, 51, 51, 51, 52, 52, 53, 53, 53, 54, 54, 54, 55, 55, 56, 56, 56 },
				//TSpinSingle1PotentialCalc = new[] { 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 },
				//TSpinDoublePotentialCalc = new[] { 25, 28, 31, 36, 42, 48, 56, 65, 74, 84, 95, 106, 119, 132, 146, 160, 175, 191, 208, 225, 243, 261 },
				//TDoubleClearPotentialCalc = new[] { 17, 17, 17, 17, 18, 18, 18, 19, 19, 20, 21, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31, 33 },
				//EmptyRowsCalc = new[] { 89, 121, 143, 161, 175, 188, 199, 209, 218, 227, 235, 242, 249, 256, 262, 268, 274, 280, 285, 290, 295, 300 },
				//SingleEmptiesCalc = new[] { 0, -8, -16, -180, -284, -390 },
				//SkipsCalc = new[] { 6, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 },
				//PerfectClearPotentialCalc = new[] { 32, 32, 31, 31, 30, 30, 29, 28, 28, 27, 27, 26, 26, 25, 24, 24, 23, 23, 22, 22, 21, 20 },
				SingleEmpties = new[] { -1, -8, -8, -60, -71, -78 },
				SingleGroupBonus = new[] { 12, 9, 28, 31 },
				Groups = new[] { 24, 5, -14, -42, -44, -75 },
				RowsDelta = new ParamCurve(0.242018474731594, 1.04633870357648, -24),
				Points = new ParamCurve(-0.317302960250527, 1.71569393034473, 61),
				Combo = new ParamCurve(-0.116176894959062, 1.58184703448788, -6),
				HolesReachable = new ParamCurve(-0.519754959084094, -0.225559080392122, -25),
				HolesUnreachable = new ParamCurve(-1.29041400523856, 0.3442398423329, -39),
				UnreachableRows = new ParamCurve(0.221509972028434, 1.182038614247, 63),
				I0 = new ParamCurve(0.518208777531981, 1.27833465598524, -41),
				I1 = new ParamCurve(0.664213428646326, 0.962765997834504, -33),
				I2 = new ParamCurve(0.18357495777309, 1.80794683555141, -54),
				I3 = new ParamCurve(-1.77922916440293, 1.04893172206357, 16),
				I4 = new ParamCurve(0.243864928279072, 0.306654516886921, 1),
				TSpinSingle0Potential = new ParamCurve(0.399228589516133, 0.980843842960895, 48),
				TSpinSingle1Potential = new ParamCurve(0.0735597257502376, 0.802183144912125, 15),
				TSpinDoublePotential = new ParamCurve(1.06851460207254, 1.74811567012221, 24),
				TDoubleClearPotential = new ParamCurve(0.0263759855180979, 2.06340241732792, 17),
				EmptyRows = new ParamCurve(143.168627443541, 0.292779768630862, -54),
				Skips = new ParamCurve(-0.0195032819174231, 1.81556369187311, 6),
				PerfectClearPotential = new ParamCurve(-0.588190606236458, 0.989481177181006, 33),
			};
			return pars.Calc();
		}
	}
}
