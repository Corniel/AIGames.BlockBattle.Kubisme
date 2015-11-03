﻿using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture, Category(Category.Evaluation)]
	public class ComplexEvaluatorTest
	{
		[Test]
		public void FreeFields_FieldWitholes_10c9c2c1c1()
		{
			var field = @"
..........
.....X....
.X.XXXXXXX
.XX...X.XX
...XXX.XXX
XXX.......";
			var pars = new EvaluatorParameters()
			{
				EmptyRowCount = 10,
				EmptyCells = new int[] 
				{
					100000,
					10000,
					1000,
					100,
					10,
					1,
				},
			};
			var expected = 1092110;

			Test(field, pars, expected);
		}

		[Test]
		public void Holes_FieldWithUnreachableRow_1()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX";
			var pars = new EvaluatorParameters()
			{
				Holes = 1,
				Unreachables = new int[] { -1, -1, -1, -1, -1, -1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void Holes_SimpleField_1()
		{
			var field = @"
.XX.......
.X.X......";
			var pars = new EvaluatorParameters()
			{
				Holes = 1,
				Unreachables = new int[] { -1, -1, -1, -1, -1, -1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void Unreachables_0Rows_0()
		{
			var field = @"
..........
...X.XXXXX
XXX.......";
			var pars = new EvaluatorParameters()
			{
				Holes = 0,
				Unreachables = new int[] { 1, 1, 1, 1, 1, 1 },
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void Unreachables_1Rows_1()
		{
			var field = @"
..........
...X.XXXXX
XXX.XXX...";
			var pars = new EvaluatorParameters()
			{
				Holes = 0,
				Unreachables = new int[] { 1, 1, 1, 1, 1, 1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void Unreachables_2Rows_2()
		{
			var field = @"
..........
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new EvaluatorParameters()
			{
				Holes = 0,
				Unreachables = new int[] { 1, 1, 1, 1, 1, 1 },
			};
			var expected = 2;

			Test(field, pars, expected);
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
		public void DoublePotentialJLT_JFitWithHole_0()
		{
			var field = @"
..........
...XXXXXXX
XX.XXXXXX.
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialJLT_JFit_1()
		{
			var field = @"
..........
...XXXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialJLT_LFitWithHole_0()
		{
			var field = @"
..........
XX...XXXXX
XX.XXXXXX.
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialJLT_LFit_1()
		{
			var field = @"
..........
XX...XXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialJLT_TFitWithHole_0()
		{
			var field = @"
..........
X...XXXXXX
XX.XXXXXX.
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialJLT_TFit_1()
		{
			var field = @"
..........
X...XXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialJLT = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}


		[Test]
		public void DoublePotentialO_OWithHole_0()
		{
			var field = @"
..........
X..XXXXXXX
X..XXXXXX.
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialO = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialO_OWithBlocade_0()
		{
			var field = @"
.X........
X..XXXXXXX
X..XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialO = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialO_OFit_1()
		{
			var field = @"
..........
X..XXXXXXX
X..XXXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				DoublePotentialO = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}


		[Test]
		public void DoublePotentialI_TunnelOf5WithHickup_0()
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
				DoublePotentialI = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void DoublePotentialI_TunnelOf3_1()
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
				DoublePotentialI = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void TripplePotentialI_TunnelOf5WithHickup_0()
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
				TripplePotentialI = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void TripplePotentialI_TunnelOf3_1()
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
				TripplePotentialI = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
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
				TetrisPotential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
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
				TetrisPotential = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void TSpinPontential_TFitWithHole_0()
		{
			var field = @"
..........
.X........
X...XXXXXX
XX.XXXXXX.
";
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void TSpinPontential_TFitWithToBlocades_0()
		{
			var field = @"
..........
.X.X......
X...XXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void TSpinPontential_TFitWithCenterBlocades_0()
		{
			var field = @"
..........
.XX.......
X...XXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void TSpinPontential_TFit_1()
		{
			var field = @"
..........
...X......
X...XXXXXX
XX.XXXXXXX
";
			var pars = new EvaluatorParameters()
			{
				TSpinPontential = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		private static void Test(string str, EvaluatorParameters pars, int expected)
		{
			var field = Field.Create(0, 0, 0, str);
			var evaluator = new Evaluator()
			{
				Pars = pars.Calc(),
			};
			var actual = evaluator.GetScore(field, 0);
			Assert.AreEqual(expected, actual);
		}
	}
}
