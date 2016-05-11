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
			// Elo: 1633, Runs:  1135 (55.2%, PT: 0.708, #: 47.0, T1: 3.99%, T2: 23.82%, I4: 1.71%, CL: 0.35%), ID: 6181, Parent: 6107, Gen: 80 (3 ply)
			{
				//PointsCalc = new[] { 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 53, 53, 53, 53, 53, 53 },
				//ComboCalc = new[] { -33, -32, -31, -31, -30, -29, -29, -28, -28, -27, -27, -26, -26, -25, -24, -24, -23, -23, -22, -22, -21, -21 },
				//I0Calc = new[] { -58, -59, -61, -63, -66, -68, -71, -74, -78, -81, -85, -89, -93, -97, -101, -105, -110, -115, -119, -124, -129, -135 },
				//I1Calc = new[] { -43, -43, -43, -43, -43, -42, -42, -42, -42, -42, -42, -41, -41, -41, -41, -41, -41, -41, -41, -40, -40, -40 },
				//I2Calc = new[] { -29, -29, -29, -29, -29, -29, -29, -29, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30 },
				//I3Calc = new[] { 48, 46, 44, 42, 40, 38, 36, 34, 32, 31, 29, 27, 25, 24, 22, 20, 19, 17, 15, 14, 12, 11 },
				//I4Calc = new[] { 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54, 54 },
				//HolesReachableCalc = new[] { -28, -29, -29, -30, -31, -32, -33, -33, -34, -35, -35, -36, -37, -38, -38, -39, -40, -40, -41, -42, -43, -43 },
				//HolesUnreachableCalc = new[] { -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41, -41 },
				//UnreachableRowsCalc = new[] { -22, -22, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24 },
				//TSpinSingle0PotentialCalc = new[] { 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5 },
				//TSpinSingle1PotentialCalc = new[] { -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14 },
				//TSpinDoublePotentialCalc = new[] { -17, -14, -9, -2, 7, 17, 29, 42, 57, 74, 92, 111, 132, 155, 178, 204, 231, 259, 289, 320, 352, 386 },
				//TDoubleClearPotentialCalc = new[] { 37, 38, 39, 40, 42, 43, 45, 47, 49, 51, 53, 55, 57, 60, 62, 64, 67, 69, 72, 75, 77, 80 },
				//EmptyRowsCalc = new[] { 91, 135, 164, 188, 208, 225, 240, 254, 267, 279, 290, 300, 310, 319, 328, 336, 344, 352, 359, 367, 373, 380 },
				//SingleEmptiesCalc = new[] { 0, -8, -22, -99, -320, -3355 },
				//SkipsCalc = new[] { 33, 34, 36, 37, 39, 42, 45, 47, 51, 54, 58, 62, 66, 71, 75, 80, 85, 91, 96, 102, 108, 115 },
				//PerfectClearPotentialCalc = new[] { 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7 },
				SingleEmpties = new[] { -5, -8, -11, -33, -80, -671 },
				SingleGroupBonus = new[] { 10, 30, 35, 22 },
				Groups = new[] { 11, 10, -16, -64, -127, -128 },
				Points = new ParamCurve(0.585631227027625, 0.334548579057749, 51),
				Combo = new ParamCurve(1.03772197989747, 0.819025602936748, -34),
				HolesReachable = new ParamCurve(-0.877052232995631, 0.945070122275503, -27),
				HolesUnreachable = new ParamCurve(-1.7250955962576, 0.0690137607976797, -39),
				UnreachableRows = new ParamCurve(-9.89641988324001, 0.0586229332722727, -12),
				I0 = new ParamCurve(-0.78754389565438, 1.48514510849491, -57),
				I1 = new ParamCurve(0.521252651326359, 0.636607061978432, -44),
				I2 = new ParamCurve(-0.0425219004042447, 1.14653443973511, -29),
				I3 = new ParamCurve(-2.80867381040007, 0.861577813327318, 51),
				I4 = new ParamCurve(0.201515816710889, -0.10599901471287, 54),
				TSpinSingle0Potential = new ParamCurve(0.375582553073764, 0.77252266984433, 1),
				TSpinSingle1Potential = new ParamCurve(-0.135067997127771, -0.33234183648601, -14),
				TSpinDoublePotential = new ParamCurve(1.19848575368524, 1.88302737222555, -18),
				TDoubleClearPotential = new ParamCurve(0.623814100865275, 1.37658286921575, 36),
				EmptyRows = new ParamCurve(179.455616640726, 0.310224717482924, -88),
				Skips = new ParamCurve(0.412685421388596, 1.71032570507378, 33),
				PerfectClearPotential = new ParamCurve(0.437007436528802, 0.689194873813542, 3),
			};
			return pars.Calc();
		}
	}
}
