using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture, Category(Category.Evaluation)]
	public class EvaluatorTest
	{
		private EvaluatorParameters GetHolePars()
		{
			var pars = new EvaluatorParameters()
			{
				HolesReachable = 1,
				HolesUnreachable = 100,
			};
			return pars;
		}
		
		[Test]
		public void Holes_SingleReachable_1()
		{
			Test(@"
			..........
			...XX.....
			...X...X..", GetHolePars(), 1);
		}

		[Test]
		public void Holes_SingleUnreachable_100()
		{
			Test(@"
			..........
			...XX.....
			...X..X...", GetHolePars(), 100);
		}

		[Test]
		public void SingleGroupBonus_AllKindOfRows_1111()
		{
			var field = @"
..........
.........X
........XX
.......XXX
......XXXX
.....XXXXX
....XXXXXX
...XXXXXXX
..XXXXXXXX
.XXXXXXXXX
XXXXXXXX..";
			var pars = new EvaluatorParameters()
			{
				SingleGroupBonus = new int[] { 1, 10, 100, 1000 },
			};
			var expected = 1111;

			Test(field, pars, expected);
		}

		[Test]
		public void TetrisPotential_TunnelOf5WithHickup_2()
		{
			var field = @"
..........
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XX.XXXX.XX
XX.XX.XXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 2);
		}
		[Test]
		public void TetrisPotential_TunnelOf3_2()
		{
			var field = @"
..........
..........
XX.XXX.XXX
XX.XX.XXXX
XX.XXXXXXX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 2);
		}

		[Test]
		public void TetrisPotential_TunnelOf5WithHickup_3()
		{
			var field = @"
..........
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XX.XXXX.XX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 3);
		}
		[Test]
		public void TetrisPotential_TunnelOf3_3()
		{
			var field = @"
..........
..........
XX.XXX.XXX
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 3);
		}


		[Test]
		public void TetrisPotential_TunnelOf4WithHickup_2()
		{
			var field = @"
..........
XX..XXXXXX
XX.XXXXXXX
XX.XX.XXXX
XX.XXXX.XX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 2);
		}

		[Test]
		public void TetrisPotential_TunnelOf5WithHickup_0()
		{
			var field = @"
..........
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XX.XXXX.XX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[]{0, 1, 2, 3, 4},
			};
			Test(field, pars, 3);
		}
		[Test]
		public void TetrisPotential_TunnelOf4_1()
		{
			var field = @"
..........
..........
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XX.XXXXXXX
XXX..X.XX.";
			var pars = new EvaluatorParameters()
			{
				TetrisPotential = new int[] { 0, 1, 2, 3, 4 },
			};
			Test(field, pars, 4);
		}

		[Test]
		public void TSpinPontential_TFitWithHole_0()
		{
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			Test(@"
			..........
			.X........
			X...XXXXXX
			XX.XXXXXX.", pars, 0);
		}
		[Test]
		public void TSpinPontential_TFitWithToBlocades_0()
		{
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			Test(@"
				..........
				.X.X......
				X...XXXXXX
				XX.XXXXXXX", pars, 0);
		}
		[Test]
		public void TSpinPontential_TFitWithCenterBlocades_0()
		{
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			Test(@"
			..........
			.XX.......
			X...XXXXXX
			XX.XXXXXXX", pars, 0);
		}
		[Test]
		public void TSpinPontential_TFit_1()
		{
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			Test(@"
			..........
			...X......
			X...XXXXXX
			XX.XXXXXXX", pars, 1);
		}
		
		[Test]
		public void TSpinPontential_ClearWithT_0()
		{
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			Test(@"
			..........
			..........
			..........
			X.........
			X..XX.....
			XXXXXX...X
			XXXXXX..XX
			XXXXXX.XXX", pars, 0);
		}


		[Test]
		public void SingleEmpties_TwoColomns_2x2()
		{
			var pars = new EvaluatorParameters()
			{
				SingleEmpties = new int[] { 0, 0, 1, 0, 0, 0 }
			};
			Test(@"
			..........
			X......X..
			X...XXXXX.
			XX.XXX.XX.
			XX..XXXXX.", pars, 2*2);
		}
		[Test]
		public void SingleEmpties_3Colomns_3x2()
		{
			var field = @"
				..........
				X......X..
				X...X..X..
				XX.XXX.XX.
				XX..XX.XX.";

			var pars = new EvaluatorParameters()
			{
				SingleEmpties = new int[] { 0, 0, 0, 1, 0, 0 }
			};
			var expected = 3 * 2;

			Test(field, pars, expected);
		}

		private static void Test(string str, EvaluatorParameters pars, int expected)
		{
			var field = Field.Create(0, 0, 0, str);
			var evaluator = new Evaluator();
			var actual = evaluator.GetScore(field, 0, pars.Calc());
			Assert.AreEqual(expected, actual);
		}
	}
}
