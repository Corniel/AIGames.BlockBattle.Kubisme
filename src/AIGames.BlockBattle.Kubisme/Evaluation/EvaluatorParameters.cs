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
			// Elo: 1616, Runs:  3854 (54.3%, PT: 0.661, #: 50.3, T1: 4.96%, T2: 17.70%, I4: 2.08%, CL: 0.42%), ID: 8450, Parent: 8338, Gen: 67
			{
				//PointsCalc = new[] { 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 36, 36, 36, 36, 36 },
				//ComboCalc = new[] { -16, -17, -18, -19, -21, -22, -24, -25, -27, -29, -31, -33, -35, -38, -40, -42, -45, -47, -50, -53, -56, -58 },
				//I0Calc = new[] { -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2 },
				//I1Calc = new[] { -21, -20, -20, -20, -19, -19, -18, -18, -18, -17, -17, -16, -16, -15, -15, -14, -14, -13, -13, -12, -12, -11 },
				//I2Calc = new[] { -11, -11, -11, -11, -11, -11, -10, -10, -10, -10, -10, -10, -10, -10, -10, -9, -9, -9, -9, -9, -9, -9 },
				//I3Calc = new[] { 6, 5, 4, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14, -15, -16 },
				//I4Calc = new[] { -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24 },
				//HolesReachableCalc = new[] { -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23 },
				//HolesUnreachableCalc = new[] { -38, -39, -40, -41, -41, -42, -43, -44, -45, -45, -46, -47, -47, -48, -49, -49, -50, -51, -51, -52, -53, -53 },
				//UnreachableRowsCalc = new[] { 61, 62, 62, 62, 63, 63, 63, 64, 64, 64, 65, 65, 66, 66, 66, 67, 67, 67, 68, 68, 69, 69 },
				//UnreachableMultiplesCalc = new[] { 14, 13, 13, 12, 12, 11, 10, 9, 8, 6, 5, 4, 2, 1, -1, -3, -4, -6, -8, -10, -12, -14 },
				//UnreachableColumnsCalc = new[] { 10, 11, 11, 12, 12, 13, 14, 14, 15, 15, 16, 17, 17, 18, 18, 19, 19, 20, 21, 21, 22, 22 },
				//TSpinSingle0PotentialCalc = new[] { 40, 41, 42, 43, 44, 46, 47, 49, 50, 52, 54, 55, 57, 59, 61, 63, 65, 67, 69, 71, 73, 75 },
				//TSpinSingle1PotentialCalc = new[] { -9, -9, -9, -9, -9, -10, -10, -10, -10, -10, -10, -10, -10, -11, -11, -11, -11, -11, -11, -11, -11, -11 },
				//TSpinDoublePotentialCalc = new[] { 19, 21, 24, 28, 33, 38, 44, 51, 58, 66, 74, 83, 92, 101, 111, 122, 133, 145, 156, 169, 181, 195 },
				//TDoubleClearPotentialCalc = new[] { 15, 16, 16, 17, 18, 19, 20, 21, 22, 23, 24, 26, 27, 28, 30, 32, 33, 35, 37, 38, 40, 42 },
				//EmptyRowsCalc = new[] { 154, 197, 226, 249, 269, 286, 301, 315, 328, 339, 350, 360, 370, 379, 388, 396, 404, 412, 419, 427, 433, 440 },
				//SingleEmptiesCalc = new[] { 0, -7, -16, -255, -380, -1070 },
				//SkipsCalc = new[] { 20, 21, 21, 22, 22, 23, 24, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 39 },
				//PerfectClearPotentialCalc = new[] { 20, 20, 19, 19, 19, 18, 18, 17, 17, 17, 16, 16, 15, 15, 14, 14, 14, 13, 13, 12, 12, 11 },
				//SingleEmpties = new[] { -5, -7, -8, -85, -95, -214 },
				SingleGroupBonus = new[] { 13, 12, 20, 40 },
				Groups = new[] { 34, 15, 2, -28, -44, -114 },
				Points = new ParamCurve(-0.00269371699541816, 1.8427738823215, 37),
				Combo = new ParamCurve(-0.416598697658628, 1.49614904196933, -16),
				HolesReachable = new ParamCurve(-0.574790046084673, 0.393293974921108, -21),
				HolesUnreachable = new ParamCurve(-1.52366691483185, 0.785239743255079, -36),
				UnreachableRows = new ParamCurve(0.281426395568997, 1.08350361716002, 61),
				UnreachableMultiples = new ParamCurve(-0.176417581737041, 1.63899709284306, 14),
				UnreachableColumns = new ParamCurve(0.821464849170297, 0.897040984313935, 9),
				I0 = new ParamCurve(0.0458790774457156, 1.47235902352259, -2),
				I1 = new ParamCurve(0.26223344001919, 1.17302411142737, -21),
				I2 = new ParamCurve(0.0611244338564575, 1.15604654001072, -11),
				I3 = new ParamCurve(-1.24130248324946, 0.942443723976613, 7),
				I4 = new ParamCurve(0.119504593499005, 0.127602401468903, -24),
				TSpinSingle0Potential = new ParamCurve(0.647956361155957, 1.30002203900367, 39),
				TSpinSingle1Potential = new ParamCurve(-0.55246735624969, 0.587226667441429, -8),
				TSpinDoublePotential = new ParamCurve(1.04210500437766, 1.66042613657191, 18),
				TDoubleClearPotential = new ParamCurve(0.220820861402899, 1.55640363935381, 15),
				EmptyRows = new ParamCurve(176.092187271858, 0.312093177437782, -22),
				Skips = new ParamCurve(0.240097513888031, 1.40618517464027, 20),
				PerfectClearPotential = new ParamCurve(-0.212880156561732, 1.20667782975361, 20),
			};
			return pars.Calc();
		}
	}
}
