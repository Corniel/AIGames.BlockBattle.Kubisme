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
			// Elo: 1619, Runs:  3089 (53.3%, PT: 0.639, #: 50.4, T1: 4.51%, T2: 15.79%, I4: 1.31%, CL: 0.74%), ID: 1508, Parent: 1369, Gen: 36
			{
				//PointsCalc = new[] { 63, 63, 63, 64, 64, 64, 64, 64, 65, 65, 65, 65, 65, 65, 66, 66, 66, 66, 66, 67, 67, 67 },
				//ComboCalc = new[] { 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 9, 10, 11, 11, 12, 13, 13, 14, 15, 15, 16, 17 },
				//I0Calc = new[] { -44, -44, -44, -44, -43, -43, -43, -43, -43, -43, -43, -42, -42, -42, -42, -42, -42, -42, -41, -41, -41, -41 },
				//I1Calc = new[] { -48, -47, -47, -46, -45, -45, -44, -44, -43, -42, -42, -41, -40, -40, -39, -39, -38, -37, -37, -36, -36, -35 },
				//I2Calc = new[] { -21, -21, -20, -20, -19, -18, -18, -17, -16, -15, -14, -13, -11, -10, -9, -7, -6, -4, -3, -1, 1, 2 },
				//I3Calc = new[] { 20, 17, 14, 10, 6, 3, -2, -6, -10, -14, -18, -23, -27, -32, -36, -41, -46, -50, -55, -60, -65, -70 },
				//I4Calc = new[] { 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25 },
				//HolesReachableCalc = new[] { -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40, -40 },
				//HolesUnreachableCalc = new[] { -56, -56, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57, -57 },
				//UnreachableRowsCalc = new[] { -38, -40, -41, -43, -44, -44, -45, -46, -47, -47, -48, -48, -49, -49, -50, -50, -51, -51, -51, -52, -52, -52 },
				//TSpinSingle0PotentialCalc = new[] { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60 },
				//TSpinSingle1PotentialCalc = new[] { -1, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2 },
				//TSpinDoublePotentialCalc = new[] { -34, -30, -25, -16, -6, 7, 22, 40, 60, 82, 107, 134, 163, 195, 230, 266, 305, 347, 390, 437, 485, 536 },
				//TDoubleClearPotentialCalc = new[] { 37, 38, 40, 42, 45, 48, 51, 55, 59, 63, 67, 72, 77, 82, 87, 93, 99, 105, 111, 117, 124, 131 },
				//EmptyRowsCalc = new[] { 42, 79, 106, 128, 146, 162, 177, 190, 202, 214, 225, 235, 244, 254, 262, 271, 279, 287, 294, 301, 308, 315 },
				//SingleEmptiesCalc = new[] { 0, -10, -22, -102, -688, -1030 },
				//SkipsCalc = new[] { -25, -23, -21, -19, -16, -13, -10, -7, -4, 0, 4, 7, 11, 15, 20, 24, 28, 33, 37, 42, 47, 51 },
				//PerfectClearPotentialCalc = new[] { 36, 36, 36, 36, 36, 36, 36, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35 },
				SingleEmpties = new[] { -2, -10, -11, -34, -172, -206 },
				SingleGroupBonus = new[] { 14, 24, 36, 34 },
				Groups = new[] { 41, 14, -3, -29, -30, -42 },
				Points = new ParamCurve(0.143608534056694, 1.07878590486392, 63),
				Combo = new ParamCurve(0.724214836023748, 0.971217547729613, 2),
				HolesReachable = new ParamCurve(-0.805072444956752, 0.0422822702676064, -39),
				HolesUnreachable = new ParamCurve(-1.27927398113534, 0.168173085618765, -55),
				UnreachableRows = new ParamCurve(-9.55754565764219, 0.303653192240746, -28),
				I0 = new ParamCurve(0.102988159097731, 1.08626936264336, -44),
				I1 = new ParamCurve(0.790443618688733, 0.929363574646415, -49),
				I2 = new ParamCurve(0.126962998695672, 1.68755730520934, -21),
				I3 = new ParamCurve(-2.55701059568674, 1.16132939746604, 23),
				I4 = new ParamCurve(0.361637411732227, -0.0352843928150827, 25),
				TSpinSingle0Potential = new ParamCurve(-0.135393512807786, 0.330154416337611, 60),
				TSpinSingle1Potential = new ParamCurve(-0.425548685435206, 0.232812980841847, -1),
				TSpinDoublePotential = new ParamCurve(1.14277370208875, 2.01046245816184, -35),
				TDoubleClearPotential = new ParamCurve(0.680131085496396, 1.59725646151125, 36),
				EmptyRows = new ParamCurve(129.571144443036, 0.367229484114796, -88),
				Skips = new ParamCurve(1.06433900203556, 1.3868205701001, -26),
				PerfectClearPotential = new ParamCurve(-0.119112591817975, 0.706858475506307, 36),
			};
			return pars.Calc();
		}
	}
}
