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
			// Elo: 1620, Runs:  1141 (52.1%, PT: 0.651, #: 50.5, T1: 5.68%, T2: 17.01%, I4: 1.41%, CL: 0.26%), ID: 2509, Parent: 2107, Gen: 27
			{
				//PointsCalc = new[] { 61, 61, 61, 61, 61, 61, 61, 61, 61, 61, 61, 61, 61, 62, 62, 62, 62, 62, 62, 62, 62, 62 },
				//ComboCalc = new[] { -13, -13, -12, -11, -11, -11, -10, -10, -9, -9, -8, -8, -8, -7, -7, -6, -6, -6, -5, -5, -5, -4 },
				//HolesReachableCalc = new[] { -34, -34, -34, -34, -34, -34, -34, -34, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35 },
				//HolesUnreachableCalc = new[] { -58, -58, -59, -59, -59, -59, -60, -60, -60, -60, -60, -61, -61, -61, -61, -61, -61, -62, -62, -62, -62, -62 },
				//UnreachableRowsCalc = new[] { -152, -154, -156, -157, -159, -160, -161, -161, -162, -163, -164, -164, -165, -165, -166, -166, -167, -167, -168, -168, -169, -169 },
				//TSpinSingle0PotentialCalc = new[] { 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59, 59 },
				//TSpinSingle1PotentialCalc = new[] { -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -42, -43, -43, -43, -43, -43, -43, -43, -43, -43 },
				//TSpinDoublePotentialCalc = new[] { -13, -9, -4, 3, 11, 21, 33, 46, 61, 77, 95, 114, 134, 156, 180, 205, 231, 258, 287, 317, 349, 381 },
				//TDoubleClearPotentialCalc = new[] { 12, 13, 14, 16, 19, 22, 25, 28, 32, 36, 41, 45, 50, 56, 61, 67, 73, 79, 86, 93, 100, 107 },
				//EmptyRowsCalc = new[] { 41, 87, 122, 152, 178, 202, 224, 244, 264, 282, 300, 316, 332, 348, 363, 377, 391, 405, 418, 431, 444, 456 },
				//SingleEmptiesCalc = new[] { 0, -6, -22, -123, -420, -1585 },
				//SkipsCalc = new[] { 32, 34, 36, 38, 41, 44, 47, 50, 54, 58, 61, 66, 70, 74, 79, 83, 88, 93, 98, 103, 109, 114 },
				//PerfectClearPotentialCalc = new[] { 42, 41, 41, 41, 41, 41, 41, 41, 41, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40 },
				SingleEmpties = new[] { -1, -6, -11, -41, -105, -317 },
				SingleGroupBonus = new[] { 2, 22, 20, 34 },
				Groups = new[] { 99, 23, -5, -19, -23, -43 },
				Points = new ParamCurve(0.0176347951404757, 1.28724797916978, 61),
				Combo = new ParamCurve(0.841147647332401, 0.792408423498275, -14),
				HolesReachable = new ParamCurve(-1.11523611033336, 0.139473473466933, -33),
				HolesUnreachable = new ParamCurve(-0.800448975991459, 0.602726194821299, -57),
				UnreachableRows = new ParamCurve(-9.85570864323527, 0.326263881567867, -142),
				TSpinSingle0Potential = new ParamCurve(0.116655692830682, 0.380376488808544, 59),
				TSpinSingle1Potential = new ParamCurve(-0.07150014936924, 0.741831650305541, -42),
				TSpinDoublePotential = new ParamCurve(1.24890470560641, 1.8627426718759, -14),
				TDoubleClearPotential = new ParamCurve(0.511895527411253, 1.69367088291689, 11),
				EmptyRows = new ParamCurve(105.441154983282, 0.516249707061798, -64),
				Skips = new ParamCurve(0.951946985442193, 1.44524544635788, 31),
				PerfectClearPotential = new ParamCurve(-0.427328338567167, 0.550637362338605, 42),
			};
			return pars.Calc();
		}
	}
}
