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
			// Elo: 1616, Runs:  6382 (56.0%, PT: 0.631, #: 52.8, T1: 4.50%, T2: 14.55%, I4: 3.10%, CL: 0.80%), ID: 3881, Parent: 3830, Gen: 58
			{
				//PointsCalc = new[] { 33, 33, 33, 33, 33, 33, 33, 33, 32, 32, 32, 32, 32, 32, 31, 31, 31, 30, 30, 30, 29, 29 },
				//ComboCalc = new[] { -15, -16, -17, -18, -19, -20, -22, -23, -25, -26, -28, -29, -31, -33, -35, -37, -39, -41, -43, -45, -47, -49 },
				//I0Calc = new[] { -22, -22, -22, -22, -22, -22, -22, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -24, -24, -24, -24 },
				//I1Calc = new[] { -30, -30, -30, -29, -29, -29, -29, -29, -28, -28, -28, -28, -27, -27, -27, -27, -26, -26, -26, -25, -25, -25 },
				//I2Calc = new[] { -22, -22, -22, -21, -21, -21, -20, -20, -20, -19, -19, -19, -18, -18, -17, -17, -16, -16, -15, -15, -14, -14 },
				//I3Calc = new[] { -6, -7, -8, -9, -9, -10, -11, -12, -13, -13, -14, -15, -16, -17, -17, -18, -19, -20, -20, -21, -22, -23 },
				//I4Calc = new[] { -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
				//HolesReachableCalc = new[] { -23, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -25, -25, -25, -25, -25, -25, -25, -25, -25 },
				//HolesUnreachableCalc = new[] { -37, -38, -39, -39, -40, -41, -41, -42, -42, -43, -43, -44, -44, -44, -45, -45, -46, -46, -46, -47, -47, -48 },
				//UnreachableRowsCalc = new[] { 45, 45, 46, 46, 46, 46, 46, 46, 46, 46, 47, 47, 47, 47, 47, 47, 47, 47, 48, 48, 48, 48 },
				//UnreachableMultiplesCalc = new[] { 15, 14, 13, 13, 12, 11, 10, 8, 7, 6, 5, 3, 2, 0, -1, -3, -4, -6, -7, -9, -11, -13 },
				//UnreachableColumnsCalc = new[] { 8, 8, 9, 10, 11, 12, 12, 13, 14, 15, 16, 16, 17, 18, 19, 20, 21, 21, 22, 23, 24, 25 },
				//TSpinSingle0PotentialCalc = new[] { 32, 32, 33, 33, 34, 35, 36, 36, 37, 38, 39, 39, 40, 41, 42, 42, 43, 44, 45, 45, 46, 47 },
				//TSpinSingle1PotentialCalc = new[] { 0, -1, -1, -1, -1, -2, -2, -2, -2, -3, -3, -3, -3, -3, -3, -4, -4, -4, -4, -4, -5, -5 },
				//TSpinDoublePotentialCalc = new[] { -13, -10, -7, -2, 3, 9, 15, 23, 30, 39, 47, 57, 67, 77, 88, 99, 111, 123, 135, 148, 162, 175 },
				//TDoubleClearPotentialCalc = new[] { 17, 17, 17, 17, 16, 16, 16, 16, 15, 15, 15, 14, 14, 14, 13, 13, 13, 12, 12, 12, 11, 11 },
				//EmptyRowsCalc = new[] { 165, 214, 250, 278, 302, 323, 342, 360, 376, 391, 405, 418, 430, 442, 453, 464, 475, 485, 495, 504, 513, 522 },
				//SingleEmptiesCalc = new[] { 0, -11, -22, -213, -284, -480 },
				//SkipsCalc = new[] { 31, 32, 32, 33, 34, 35, 36, 37, 38, 39, 41, 42, 43, 45, 46, 48, 49, 51, 53, 54, 56, 58 },
				//PerfectClearPotentialCalc = new[] { 18, 17, 17, 16, 16, 15, 15, 14, 13, 13, 12, 11, 11, 10, 9, 8, 8, 7, 6, 5, 5, 4 },
				//SingleEmpties = new[] { -3, -11, -11, -71, -71, -96 },
				SingleGroupBonus = new[] { 9, 11, 15, 31 },
				Groups = new[] { 19, 14, -1, -15, -46, -76 },
				Points = new ParamCurve(-0.00527209080755721, 2.13974841006509, 33),
				Combo = new ParamCurve(-0.401376549713314, 1.44075465565547, -15),
				HolesReachable = new ParamCurve(-0.439305378403515, 0.475891428906471, -23),
				HolesUnreachable = new ParamCurve(-1.31360944956541, 0.704850000329316, -36),
				UnreachableRows = new ParamCurve(0.190067554451525, 0.8809476534836, 45),
				UnreachableMultiples = new ParamCurve(-0.341357146203518, 1.42143538882956, 15),
				UnreachableColumns = new ParamCurve(0.693784930743277, 1.04889999758452, 7),
				I0 = new ParamCurve(-0.034807674586773, 1.29015202485025, -22),
				I1 = new ParamCurve(0.0791877958923578, 1.35039153089747, -30),
				I2 = new ParamCurve(0.0923286641947925, 1.44570297449827, -22),
				I3 = new ParamCurve(-0.949902494251728, 0.946074808202685, -5),
				I4 = new ParamCurve(0.360119940992445, 0.170563150942326, -2),
				TSpinSingle0Potential = new ParamCurve(0.551001233328134, 1.08935498986393, 31),
				TSpinSingle1Potential = new ParamCurve(-0.39851909186691, 0.797773856483401, 0),
				TSpinDoublePotential = new ParamCurve(1.24938215846196, 1.6243531239219, -14),
				TDoubleClearPotential = new ParamCurve(-0.0606713782064617, 1.50287064630538, 17),
				EmptyRows = new ParamCurve(176.677684466537, 0.357741549890489, -12),
				Skips = new ParamCurve(0.291853723861277, 1.46044738786295, 31),
				PerfectClearPotential = new ParamCurve(-0.276742051076144, 1.27656808737665, 18),
			};
			return pars.Calc();
		}
	}
}
