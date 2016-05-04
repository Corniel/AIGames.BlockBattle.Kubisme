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
			// Elo: 1621, Runs:  1504 (53.4%, PT: 0.647, #: 50.7, T1: 4.51%, T2: 16.82%, I4: 1.88%, CL: 0.53%), ID: 5857, Parent: 5824, Gen: 39
			{
				//PointsCalc = new int[] { 44, 45, 45, 45, 46, 46, 46, 47, 47, 47, 48, 48, 48, 48, 49, 49, 49, 50, 50, 50, 50, 51 },
				//ComboCalc = new int[] { -6, -6, -5, -5, -4, -4, -3, -2, -2, -1, -1, 0, 0, 1, 2, 2, 3, 4, 4, 5, 6, 6 },
				//I0Calc = new int[] { -39, -40, -41, -41, -42, -43, -44, -44, -45, -46, -47, -47, -48, -49, -49, -50, -51, -51, -52, -53, -54, -54 },
				//I1Calc = new int[] { -21, -22, -23, -24, -25, -26, -27, -28, -29, -30, -31, -32, -33, -34, -35, -36, -37, -38, -39, -40, -41, -42 },
				//I2Calc = new int[] { -34, -32, -30, -29, -27, -26, -25, -23, -22, -21, -20, -19, -17, -16, -15, -14, -13, -12, -10, -9, -8, -7 },
				//I3Calc = new int[] { 10, 9, 8, 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7, -8, -10, -11, -12, -13 },
				//I4Calc = new int[] { 57, 57, 56, 56, 56, 55, 55, 55, 54, 54, 53, 53, 53, 52, 52, 52, 51, 51, 51, 50, 50, 50 },
				//HolesReachableCalc = new int[] { -35, -30, -28, -26, -25, -24, -23, -22, -21, -20, -19, -18, -17, -16, -16, -15, -14, -14, -13, -12, -12, -11 },
				//HolesUnreachableCalc = new int[] { -46, -43, -42, -41, -41, -40, -40, -39, -39, -38, -38, -38, -37, -37, -37, -36, -36, -36, -36, -35, -35, -35 },
				//UnreachableRowsCalc = new int[] { 5, 1, -3, -6, -9, -12, -15, -18, -21, -24, -27, -30, -32, -35, -38, -40, -43, -45, -48, -51, -53, -56 },
				//UnreachableMultiplesCalc = new int[] { 7, 6, 5, 4, 3, 2, 1, 1, 0, -1, -2, -3, -4, -5, -6, -7, -8, -8, -9, -10, -11, -12 },
				//UnreachableColumnsCalc = new int[] { 12, 12, 11, 11, 11, 11, 10, 10, 10, 10, 9, 9, 9, 9, 8, 8, 8, 8, 8, 7, 7, 7 },
				//TSpinSingle0PotentialCalc = new int[] { 26, 26, 27, 27, 28, 28, 28, 29, 29, 30, 30, 30, 31, 31, 31, 32, 32, 32, 33, 33, 33, 34 },
				//TSpinSingle1PotentialCalc = new int[] { 5, 5, 4, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, -1 },
				//TSpinDoublePotentialCalc = new int[] { -4, 29, 44, 56, 67, 76, 84, 92, 99, 106, 113, 119, 125, 130, 136, 141, 147, 152, 157, 162, 166, 171 },
				//TDoubleClearPotentialCalc = new int[] { 10, 23, 28, 33, 37, 40, 43, 46, 49, 51, 54, 56, 58, 60, 62, 64, 66, 68, 70, 72, 74, 75 },
				//EmptyRowsCalc = new int[] { -58, -40, -32, -26, -20, -15, -10, -6, -2, 2, 5, 9, 12, 15, 18, 21, 24, 27, 29, 32, 35, 37 },
				//SingleEmptiesCalc = new int[] { 0, -2, -12, -75, -260, -390 },
				//SkipsCalc = new int[] { 23, 23, 23, 22, 22, 22, 21, 21, 21, 21, 20, 20, 20, 20, 19, 19, 19, 18, 18, 18, 18, 17 },
				//PerfectClearPotentialCalc = new int[] { -12, -7, -4, -1, 3, 6, 8, 11, 14, 16, 19, 21, 24, 26, 29, 31, 34, 36, 38, 41, 43, 45 },
				SingleEmpties = new int[] { -1, -2, -6, -25, -65, -78 },
				SingleGroupBonus = new int[] { 11, 11, 24, 27 },
				Groups = new int[] { 33, 0, -17, -62, -67, -70 },
				Points = new ParamCurve(44, 51, 0.842075132379388, 0.974464439254186),
				Combo = new ParamCurve(-6, 4, 1.06681218411789, 1.06037375095449),
				HolesReachable = new ParamCurve(-35, -13, 0.497084439612225, 1.0541132234482),
				HolesUnreachable = new ParamCurve(-46, -32, 0.515145205897536, 0.849224996164409),
				UnreachableRows = new ParamCurve(5, -88, 1.01383893893938, 0.861832262012868),
				UnreachableMultiples = new ParamCurve(7, -13, 1.00607483090638, 0.984146323377256),
				UnreachableColumns = new ParamCurve(12, 6, 0.985502732522037, 0.95081967136538),
				I0 = new ParamCurve(-39, -54, 0.945709971112747, 1.00486505654971),
				I1 = new ParamCurve(-21, -45, 1.0959099618028, 0.966003149803693),
				I2 = new ParamCurve(-34, -5, 0.876082623275757, 0.971041485922484),
				I3 = new ParamCurve(10, -12, 0.940045855555118, 1.00937043107656),
				I4 = new ParamCurve(57, 47, 1.07159748544517, 0.897944624177592),
				TSpinSingle0Potential = new ParamCurve(26, 33, 0.867452717551985, 1.02637053903478),
				TSpinSingle1Potential = new ParamCurve(5, 0, 0.787029772230154, 1.05425254864738),
				TSpinDoublePotential = new ParamCurve(-4, 174, 0.553074625625779, 0.989382676563826),
				TDoubleClearPotential = new ParamCurve(10, 71, 0.520597605926996, 1.04283505344937),
				EmptyRows = new ParamCurve(-58, 43, 0.572430284442294, 0.96541672803874),
				Skips = new ParamCurve(23, 18, 1.01452716577243, 1.04172273451229),
				PerfectClearPotential = new ParamCurve(-12, 64, 0.917220664240948, 0.897030559353244),
			};

			return pars.Calc();
		}
	}
}
