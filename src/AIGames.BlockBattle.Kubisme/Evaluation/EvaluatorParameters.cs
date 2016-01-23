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
			// Elo: 1634, Runs: 22296 (53.9%, PT: 0.639, #: 55.9, T1: 4.89%, T2: 16.08%, I4: 1.13%, CL: 0.35%), ID: 4493, Parent: 4026, Gen: 40
			{
				//PointsCalc = new int[] { 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18 },
				//ComboCalc = new int[] { 19, 18, 17, 16, 16, 15, 14, 13, 13, 12, 11, 10, 10, 9, 8, 8, 7, 6, 6, 5, 4, 3 },
				//I0Calc = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
				//I1Calc = new int[] { -4, -4, -5, -6, -6, -7, -8, -8, -9, -10, -10, -11, -12, -13, -14, -14, -15, -16, -17, -18, -18, -19 },
				//I2Calc = new int[] { 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 8, 8, 8, 8 },
				//I3Calc = new int[] { 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 10, 10, 10, 10, 10, 10, 10, 10, 10, 11 },
				//I4Calc = new int[] { -16, -14, -12, -10, -8, -6, -4, -3, -1, 1, 3, 5, 7, 9, 10, 12, 14, 16, 18, 20, 21, 23 },
				//HolesReachableCalc = new int[] { -45, -41, -38, -35, -32, -30, -28, -25, -23, -21, -19, -17, -15, -13, -12, -10, -8, -6, -4, -3, -1, 1 },
				//HolesUnreachableCalc = new int[] { -44, -41, -40, -38, -36, -35, -33, -32, -31, -29, -28, -27, -26, -25, -23, -22, -21, -20, -19, -18, -17, -15 },
				//UnreachableRowsCalc = new int[] { 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25 },
				//TSpinSingle0PotentialCalc = new int[] { 19, 19, 18, 18, 18, 17, 17, 17, 16, 16, 15, 15, 15, 14, 14, 14, 13, 13, 13, 12, 12, 12 },
				//TSpinSingle1PotentialCalc = new int[] { 13, 13, 12, 12, 11, 11, 10, 10, 9, 9, 8, 8, 8, 7, 7, 6, 6, 5, 5, 5, 4, 4 },
				//TSpinDoublePotentialCalc = new int[] { 62, 62, 63, 63, 64, 64, 64, 65, 65, 66, 66, 66, 67, 67, 68, 68, 69, 69, 70, 70, 70, 71 },
				//TDoubleClearPotentialCalc = new int[] { 4, 6, 7, 9, 10, 12, 13, 15, 16, 17, 19, 20, 22, 23, 24, 26, 27, 28, 30, 31, 32, 34 },
				//EmptyRowsCalc = new int[] { -24, -21, -20, -18, -17, -15, -14, -13, -12, -11, -10, -9, -8, -7, -6, -5, -4, -3, -2, -2, -1, 0 },
				//SingleEmptiesCalc = new int[] { 0, -5, -12, -36, -176, -265 },
				//SkipsCalc = new int[] { 21, 19, 17, 16, 14, 12, 11, 9, 8, 6, 5, 3, 2, 1, -1, -2, -4, -5, -6, -8, -9, -11 },
				//PerfectClearPotentialCalc = new int[] { -8, -7, -7, -6, -6, -6, -5, -5, -5, -4, -4, -4, -3, -3, -3, -2, -2, -2, -1, -1, -1, 0 },
				//DeltaCalc = new int[] { 14, 13, 12, 11, 11, 10, 9, 9, 8, 7, 7, 6, 5, 5, 4, 3, 3, 2, 2, 1, 0, 0 },
				SingleEmpties = new int[] { -1, -5, -6, -12, -44, -53 },
				SingleGroupBonus = new int[] { 8, 4, 10, 16 },
				Groups = new int[] { 10, 5, -6, -18, -38, -48 },
				Points = new ParamCurve(18, 18, 1.014830118228, 0.838432989296965),
				Combo = new ParamCurve(19, 3, 0.936776996094886, 0.990490255936304),
				HolesReachable = new ParamCurve(-45, 33, 0.952007344546505, 0.816464351172821),
				HolesUnreachable = new ParamCurve(-44, -1, 0.931151341674711, 0.855106170998661),
				UnreachableRows = new ParamCurve(2, 28, 1.02262982189191, 0.954461493640961),
				I0 = new ParamCurve(-1, -1, 0.936257022676448, 0.992174425205204),
				I1 = new ParamCurve(-4, -15, 1.0432217357769, 1.10095604559849),
				I2 = new ParamCurve(4, 9, 1.03892328237388, 0.943827527492944),
				I3 = new ParamCurve(8, 11, 1.00925221277347, 0.951954131552442),
				I4 = new ParamCurve(-16, 18, 0.925708331242879, 1.05028004789725),
				TSpinSingle0Potential = new ParamCurve(19, 12, 0.999329595470956, 1.02101698958501),
				TSpinSingle1Potential = new ParamCurve(13, 3, 0.985073609738149, 0.972918544492897),
				TSpinDoublePotential = new ParamCurve(62, 71, 1.06265983231003, 0.995436844241155),
				TDoubleClearPotential = new ParamCurve(4, 43, 1.03414778660212, 0.914759317569463),
				EmptyRows = new ParamCurve(-24, 7, 0.808765703482153, 0.899237215128975),
				Skips = new ParamCurve(21, -29, 1.0507113227564, 0.855683510862029),
				PerfectClearPotential = new ParamCurve(-8, 1, 0.885743197049049, 0.940702834629212),
				Delta = new ParamCurve(14, -4, 0.951239115950989, 0.920210138598872),
			};
			return pars.Calc();
		}
	}
}
