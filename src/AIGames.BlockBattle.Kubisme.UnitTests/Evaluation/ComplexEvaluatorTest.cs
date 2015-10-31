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
				FreeRows = new int[] { 0, 1 },
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
		public void Test_Unreachables_2Rows()
		{
			var field = @"
..........
XXXX.XXXXX
...XXXXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				UnreachableFactor = 1,
			};
			var expected = 2;

			Test(field, pars, expected);
		}
		[Test]
		public void Test_Unreachables_0Rows()
		{
			var field = @"
..........
XXXX.XXXXX
...X.XXXXX
XXX.......";
			var pars = new ComplexParameters()
			{
				UnreachableFactor = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void OBlockPlacement_OneRowFree_0Matches()
		{
			var field = @"
..........
XXXX.XXXXX
...X.XXXXX";
			var pars = new ComplexParameters()
			{
				OBlockPlacement = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void OBlockPlacement_NoSpots_0Matches()
		{
			var field = @"
..........
..........
X.XX...X..
...X.X.XX.";
			var pars = new ComplexParameters()
			{
				OBlockPlacement = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}
		[Test]
		public void OBlockPlacement_OneSpot_1Matches()
		{
			var field = @"
..........
X.XX...X..
...X.XXXX.";
			var pars = new ComplexParameters()
			{
				OBlockPlacement = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}

		private static void Test(string str, ComplexParameters pars, int expected)
		{
			var field = Field.Create(0, 0, 0, str);
			var evaluator = new ComplexEvaluator()
			{
				Parameters = pars,
			};
			var actual = evaluator.GetScore(field, 0);
			Assert.AreEqual(expected, actual);
		}
	}
}
