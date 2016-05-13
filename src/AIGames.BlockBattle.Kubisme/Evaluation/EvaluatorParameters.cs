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
			// Elo: 1620, Runs:  1844 (54.8%, PT: 0.654, #: 52.8, T1: 5.04%, T2: 19.85%, I4: 0.68%, CL: 0.16%), ID: 8731, Parent: 8703, Gen: 78
			{
				//PointsCalc = new[] { 33, 33, 33, 33, 33, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 35, 35, 35, 35, 35, 35, 35 },
				//ComboCalc = new[] { -19, -18, -18, -17, -17, -16, -16, -15, -15, -14, -14, -13, -13, -12, -12, -11, -11, -11, -10, -10, -9, -9 },
				//HolesReachableCalc = new[] { -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26 },
				//HolesUnreachableCalc = new[] { -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38, -38 },
				//UnreachableRowsCalc = new[] { -295, -296, -296, -296, -297, -297, -297, -297, -297, -297, -297, -297, -297, -297, -298, -298, -298, -298, -298, -298, -298, -298 },
				//TSpinSingle0PotentialCalc = new[] { 54, 53, 53, 52, 52, 52, 51, 51, 51, 50, 50, 49, 49, 49, 48, 48, 48, 47, 47, 47, 46, 46 },
				//TSpinSingle1PotentialCalc = new[] { -49, -49, -49, -49, -49, -49, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -47, -47, -47, -47, -47, -47 },
				//TSpinDoublePotentialCalc = new[] { -4, -1, 3, 8, 14, 22, 30, 40, 51, 63, 76, 89, 104, 120, 137, 154, 173, 192, 213, 234, 256, 279 },
				//TDoubleClearPotentialCalc = new[] { -2, -1, 1, 3, 6, 10, 14, 19, 24, 30, 37, 44, 52, 60, 69, 79, 89, 100, 111, 123, 136, 149 },
				//EmptyRowsCalc = new[] { 60, 91, 113, 131, 146, 160, 172, 183, 193, 202, 211, 220, 228, 235, 242, 249, 256, 263, 269, 275, 281, 286 },
				//SingleEmptiesCalc = new[] { 0, -1, -10, -51, -132, -750 },
				//SkipsCalc = new[] { 26, 27, 28, 28, 29, 29, 30, 31, 31, 32, 32, 33, 33, 34, 34, 35, 35, 36, 36, 37, 37, 38 },
				//PerfectClearPotentialCalc = new[] { 61, 61, 61, 61, 61, 61, 61, 61, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60 },
				SingleEmpties = new[] { -1, -1, -5, -17, -33, -150 },
				SingleGroupBonus = new[] { 9, 10, 18, 17 },
				Groups = new[] { 176, 9, -12, -39, -42, -50 },
				Points = new ParamCurve(0.0944685986265548, 0.99900740309998, 33),
				Combo = new ParamCurve(0.8414358782582, 0.836691961251202, -20),
				HolesReachable = new ParamCurve(0.0124674282968008, 0.659975231438876, -26),
				HolesUnreachable = new ParamCurve(-1.38468137569726, -0.0141344600357108, -37),
				UnreachableRows = new ParamCurve(-10.2020491494797, 0.0755151487886917, -285),
				TSpinSingle0Potential = new ParamCurve(-0.410325448215007, 0.965253904554995, 54),
				TSpinSingle1Potential = new ParamCurve(0.503174321260303, 0.567334262095394, -50),
				TSpinDoublePotential = new ParamCurve(1.0355431213975, 1.81606172358361, -5),
				TDoubleClearPotential = new ParamCurve(0.341211244370789, 1.97129311514329, -2),
				EmptyRows = new ParamCurve(107.86466049695, 0.365828333422542, -48),
				Skips = new ParamCurve(1.04878548439592, 0.812320481054486, 25),
				PerfectClearPotential = new ParamCurve(-0.163741579931229, 0.514722767565401, 61),
			};
			return pars.Calc();
		}
	}
}
