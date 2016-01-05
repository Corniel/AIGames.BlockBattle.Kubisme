using System;
using System.Xml.Serialization;

namespace AIGames.BlockBattle.Kubisme
{
	[Serializable]
	public class EvaluatorParameters
	{
		public EvaluatorParameters()
		{
			TetrisPotential = new int[5];
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

		public int[] HolesReachableCalc { get { return m_HolesReachable; } }
		private int[] m_HolesReachable;

		public int[] HolesUnreachableCalc { get { return m_HolesUnreachable; } }
		private int[] m_HolesUnreachable;

		public int[] SingleEmptiesCalc { get { return m_SingleEmpties; } }
		private int[] m_SingleEmpties;

		public int[] SkipsCalc { get { return m_SkipsCalc; } }
		private int[] m_SkipsCalc;

		public int[] PerfectClearPotentialCalc { get { return m_PerfectClearPotential; } }
		private int[] m_PerfectClearPotential;
		/// <summary>Factor for current combo's.</summary>
		public int Combo { get; set; }
		
		[ParameterType(ParameterType.Positive)]
		public int Points { get; set; }

		

		/// <summary>Points for a potential Tetris, triple, double and single.</summary>
		[ParameterType(ParameterType.Ascending)]
		public int[] TetrisPotential { get; set; }

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
			// Elo: 1626, Runs:  1150 (52.8%, PT: 0.676, #: 50.5, T1: 4.81%, T2: 21.04%, I4: 1.07%, CL: 0.52%), ID: 4703, Parent: 4697, Gen: 98
			{
				//TSpinSingle0PotentialCalc = new int[] { -9, 15, 27, 38, 47, 55, 63, 70, 77, 83, 89, 95, 101, 106, 112, 117, 122, 127, 132, 137, 141, 146 },
				//TSpinSingle1PotentialCalc = new int[] { 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -10, -11, -12, -13, -14 },
				//TSpinDoublePotentialCalc = new int[] { 205, 245, 280, 315, 348, 381, 413, 445, 477, 509, 540, 571, 601, 632, 662, 693, 723, 752, 782, 812, 842, 871 },
				//TDoubleClearPotentialCalc = new int[] { 74, 110, 133, 153, 172, 190, 206, 222, 237, 252, 266, 280, 294, 307, 320, 333, 345, 357, 370, 382, 393, 405 },
				//EmptyRowsCalc = new int[] { 118, 286, 360, 418, 466, 510, 549, 585, 619, 651, 682, 711, 738, 765, 791, 815, 839, 863, 885, 907, 929, 950 },
				//HolesReachableCalc = new int[] { -214, -191, -176, -164, -153, -142, -132, -123, -114, -105, -96, -88, -80, -72, -65, -57, -50, -43, -36, -29, -22, -15 },
				//HolesUnreachableCalc = new int[] { -226, -190, -178, -168, -160, -154, -148, -142, -137, -133, -128, -124, -120, -116, -113, -109, -106, -103, -100, -97, -94, -91 },
				//SingleEmptiesCalc = new int[] { 0, -25, -52, -276, -376, -1275 },
				//SkipsCalc = new int[] { -18, 17, 42, 64, 85, 105, 124, 143, 160, 178, 195, 211, 227, 243, 259, 274, 290, 305, 319, 334, 349, 363 },
				Combo = -6,
				Points = 199,
				PerfectClearPotential = new ParamCurve(739),
				TetrisPotential = new int[] { -142, -90, -48, -17, 166 },
				SingleEmpties = new int[] { -1, -25, -26, -92, -94, -255 },
				SingleGroupBonus = new int[] { 23, 10, 53, 66 },
				Groups = new int[] { 58, 36, -21, -107, -130, -157 },
				HolesReachable = new ParamCurve(-214, -15, 0.709493574002696),
				HolesUnreachable = new ParamCurve(-226, -91, 0.436054888096868),
				TSpinSingle0Potential = new ParamCurve(-9, 146, 0.615300814240982),
				TSpinSingle1Potential = new ParamCurve(6, -14, 0.918633534642757),
				TSpinDoublePotential = new ParamCurve(205, 871, 0.927206740185875),
				TDoubleClearPotential = new ParamCurve(74, 405, 0.733161952127343),
				EmptyRows = new ParamCurve(118, 950, 0.524794427281711),
				Skips = new ParamCurve(-18, 363, 0.78646622322278),
			};
			return pars.Calc();
		}
	}
}
