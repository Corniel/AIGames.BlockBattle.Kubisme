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
			UnreachableMultiples = new ParamCurve();
			UnreachableColumns = new ParamCurve();
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
		public int[] UnreachableMultiplesCalc { get { return m_UnreachableMultiples; } }
		private int[] m_UnreachableMultiples;

		public int[] UnreachableColumnsCalc { get { return m_UnreachableColumns; } }
		private int[] m_UnreachableColumns;

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
		public ParamCurve UnreachableMultiples { get; set; }
		public ParamCurve UnreachableColumns { get; set; }

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
			m_UnreachableMultiples = UnreachableMultiples.Calculate();
			m_UnreachableColumns = UnreachableColumns.Calculate();
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
			// Elo: 1625, Runs: 591(56.0 %, PT: 0.737, #: 46.7, T1: 4.77%, T2: 24.95%, I4: 3.81%, CL: 3.21%), ID: 7536, Parent: 7531, Gen: 70
			{
				//PointsCalc = new int[] { 42, 42, 42, 41, 41, 41, 41, 40, 40, 40, 40, 39, 39, 39, 39, 39, 38, 38, 38, 38, 37, 37 },
				//ComboCalc = new int[] { -4, -3, -2, -1, 0, 0, 1, 1, 2, 2, 3, 3, 3, 4, 4, 5, 5, 5, 6, 6, 7, 7 },
				//I0Calc = new int[] { -49, -51, -52, -53, -54, -55, -55, -56, -57, -57, -58, -58, -59, -59, -60, -60, -60, -61, -61, -62, -62, -63 },
				//I1Calc = new int[] { -91, -85, -81, -77, -73, -69, -66, -63, -60, -57, -54, -51, -48, -45, -42, -39, -37, -34, -31, -29, -26, -23 },
				//I2Calc = new int[] { -22, -22, -21, -21, -20, -20, -19, -19, -18, -18, -17, -17, -16, -16, -15, -15, -14, -14, -13, -13, -12, -11 },
				//I3Calc = new int[] { -23, -25, -28, -30, -32, -34, -36, -38, -40, -42, -43, -45, -47, -49, -51, -52, -54, -56, -58, -60, -61, -63 },
				//I4Calc = new int[] { 132, 128, 124, 120, 116, 112, 108, 104, 100, 96, 92, 88, 84, 79, 75, 71, 67, 63, 58, 54, 50, 46 },
				//HolesReachableCalc = new int[] { -106, -81, -72, -66, -61, -56, -52, -48, -44, -41, -38, -35, -32, -30, -27, -25, -22, -20, -18, -16, -14, -12 },
				//HolesUnreachableCalc = new int[] { -17, -24, -27, -29, -31, -33, -34, -36, -37, -38, -39, -40, -41, -42, -43, -44, -45, -46, -46, -47, -48, -49 },
				//UnreachableRowsCalc = new int[] { -73, -72, -72, -71, -71, -70, -70, -69, -69, -68, -68, -67, -67, -66, -66, -65, -65, -65, -64, -64, -63, -63 },
				//TSpinSingle0PotentialCalc = new int[] { 51, 50, 49, 48, 48, 47, 46, 45, 44, 43, 43, 42, 41, 40, 39, 38, 38, 37, 36, 35, 34, 34 },
				//TSpinSingle1PotentialCalc = new int[] { -23, -22, -21, -19, -18, -18, -17, -16, -15, -14, -13, -12, -11, -11, -10, -9, -8, -7, -7, -6, -5, -4 },
				//TSpinDoublePotentialCalc = new int[] { -5, 40, 61, 78, 92, 105, 117, 128, 138, 148, 157, 166, 175, 183, 191, 199, 206, 213, 221, 227, 234, 241 },
				//TDoubleClearPotentialCalc = new int[] { 61, 52, 47, 44, 41, 39, 37, 34, 32, 31, 29, 27, 26, 24, 23, 21, 20, 18, 17, 16, 15, 13 },
				//EmptyRowsCalc = new int[] { -83, -50, -38, -29, -22, -15, -9, -4, 1, 5, 10, 14, 17, 21, 25, 28, 31, 34, 38, 40, 43, 46 },
				//SingleEmptiesCalc = new int[] { 0, -7, -16, -57, -364, -1385 },
				//SkipsCalc = new int[] { 4, 7, 10, 12, 15, 17, 19, 21, 24, 26, 28, 30, 32, 34, 37, 39, 41, 43, 45, 47, 49, 51 },
				//PerfectClearPotentialCalc = new int[] { -25, -20, -13, -6, 1, 8, 16, 24, 32, 40, 48, 57, 65, 74, 82, 91, 100, 109, 118, 127, 136, 146 },
				SingleEmpties = new int[] { -3, -7, -8, -19, -91, -277 },
				SingleGroupBonus = new int[] { 6, 18, 25, 21 },
				Groups = new int[] { 16, 8, -21, -49, -59, -73 },
				Points = new ParamCurve(42, 38, 0.939773555759971, 1.06805771979087),
				Combo = new ParamCurve(-4, 14, 0.835779413708345, 0.80189662128851),
				HolesReachable = new ParamCurve(-106, 1, 0.481478990365491, 0.914574784792488),
				HolesUnreachable = new ParamCurve(-17, -57, 0.561397633097327, 0.863961289776509),
				UnreachableRows = new ParamCurve(-73, -58, 1.04241970470316, 0.878730877495148),
				I0 = new ParamCurve(-49, -79, 0.863638464671635, 0.697195919722891),
				I1 = new ParamCurve(-91, -18, 0.82118954175867, 0.969382468301258),
				I2 = new ParamCurve(-22, -15, 0.902458456691797, 1.1486742127219),
				I3 = new ParamCurve(-23, -98, 1.11724162108637, 0.815743741907824),
				I4 = new ParamCurve(132, 44, 1.03738039618112, 0.993753997613579),
				TSpinSingle0Potential = new ParamCurve(51, 35, 0.959700316286889, 1.02953646105924),
				TSpinSingle1Potential = new ParamCurve(-23, -6, 0.829511389533183, 1.04064745848013),
				TSpinDoublePotential = new ParamCurve(-5, 218, 0.52790174800426, 1.06070995146161),
				TDoubleClearPotential = new ParamCurve(61, -5, 0.637847237636955, 0.831031965860813),
				EmptyRows = new ParamCurve(-83, 130, 0.612722081493057, 0.731740780200647),
				Skips = new ParamCurve(4, 35, 0.766597129952551, 1.17793324463963),
				PerfectClearPotential = new ParamCurve(-25, 85, 0.995818649064174, 1.14469742398624),
			};
			return pars.Calc();
		}
	}
}
