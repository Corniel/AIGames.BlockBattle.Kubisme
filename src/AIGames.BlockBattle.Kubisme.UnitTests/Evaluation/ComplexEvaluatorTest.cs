﻿using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class ComplexEvaluatorTest
	{
		[Test]
		public void Test_Blockades_21()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Blockades = 1,
			};
			var expected = 21;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_LastBlockades_1()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				LastBlockades = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_LastBlockadesComplex_3()
		{
			var field = @"
..X.......
..XX......
.X..XXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				LastBlockades = 1,
			};
			var expected = 3;

			Test(field, pars, expected);
		}


		[Test]
		public void Test_OwnFreeRows_1()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
.XX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Free = 1,
				OwnFreeRows = new int[] { 0, 1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}


		[Test]
		public void Test_Floor_3()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Floor = 1,
			};
			var expected = 3;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_Holes_16()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Holes = 1,
			};
			var expected = 16;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_HoleWithDiagonalAccess_1()
		{
			var field = @"
..........
.XX.......
.X.X......";
			var pars = new ComplexParameters()
			{
				Holes = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_NeighborsHorizontal_6()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				NeighborsHorizontal = 1,
			};
			var expected = 6;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_NeighborsVertical_15()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				NeighborsVertical = 1,
			};
			var expected = 15;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_WallsLeft_3()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX......X";
			var pars = new ComplexParameters()
			{
				WallsLeft = 1,
			};
			var expected = 3;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_WallsLeftWithInterception_1()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
...XXX.XXX
XXX......X";
			var pars = new ComplexParameters()
			{
				WallsLeft = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		
		[Test]
		public void Test_WallsRight_4()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX......X";
			var pars = new ComplexParameters()
			{
				WallsRight = 1,
			};
			var expected = 4;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_WallsRightNoneOnTheFloor_0()
		{
			var field = @"
.........X
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				WallsRight = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_ComboPotential_0()
		{
			var field = @"
.........X
..X.......
.X.XXXXXXX
XXX...XXXX
X..XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				ComboPotential = new int[0],
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_ComboPotential_1()
		{
			var field = @"
.........X
..X.......
XXX..XXXXX
XXX.X.XXXX
X..XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				ComboPotential = new int[] { 1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void Test_ComboPotential_2()
		{
			var field = @"
.........X
..X.......
XXX..XXXXX
XXX...XXXX
X..XXX.XXX
XXX.......";
			var pars = new ComplexParameters()
			{
				ComboPotential = new int[] { 0, 1 },
			};
			var expected = 2;

			Test(field, pars, expected);
		}
		[Test]
		public void Test_ComboPotential_3()
		{
			var field = @"
.........X
........XX
XXX..XXXXX
XXX...XXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				ComboPotential = new int[] { 0, 0, 1 },
			};
			var expected = 3;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_TSpinPotential_0()
		{
			var field = @"
.........X
........XX
XXX...XXXX
XXXXX.XXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				TSpinPotential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_TSpinPotential_1()
		{
			var field = @"
.........X
........XX
XXX...XXXX
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				TSpinPotential = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_TSpinPotential_unreachable()
		{
			var field = @"
.........X
...XXX..XX
XXX...XXXX
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				TSpinPotential = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_2Unreachable_2()
		{
			var field = @"
.........X
........XX
XXX...XXXX
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Unreachables = new int[] { 0, 1, 2 },
			};
			var expected = 2;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_5UnreachablesWithHole_5()
		{
			var field = @"
....X....X
.....XX.XX
XXXX..XXXX
XX...XXXXX
XXX.XXXXXX
XXX.XXXXXX
XXXXXX.XXX";
			var pars = new ComplexParameters()
			{
				Unreachables = new int[] { 0, 1, 2, 3, 4, 5 },
			};
			var expected = 5;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_Reachables_4()
		{
			var field = @"
.........X
........XX
XXX...XXXX
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				Reachables = new int[] { 0, 1, 2, 3, 4 },
			};
			var expected = 4;

			Test(field, pars, expected);
		}


		[Test]
		public void LostScore_None_Negative()
		{
			var evaluator = new ComplexEvaluator();
			var act = evaluator.LostScore;
			var exp = short.MinValue;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void WinScore_None_Postive()
		{
			var evaluator = new ComplexEvaluator();
			var act = evaluator.WinScore;
			var exp = short.MaxValue;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DrawScore_None_RoundZero()
		{
			var evaluator = new ComplexEvaluator();
			var act = evaluator.DrawScore;
			var exp = 0;
			Assert.AreEqual(exp, act);
		}

		private static void Test(string str, ComplexParameters pars, int expected)
		{
			var field = Field.Create(0, 0, str);
			var evaluator = new ComplexEvaluator()
			{
				Parameters = pars,
				Opponent = new OpponentStub(),
			};
			var actual = evaluator.GetScore(field, 0);
			Assert.AreEqual(expected, actual);
		}
	}
}