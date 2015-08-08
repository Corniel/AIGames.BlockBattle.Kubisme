using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class SimpleEvaluatorTest
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
			var pars = new SimpleParameters()
			{
				Blockades = 1,
			};
			var expected = 21;

			Test(field, pars, expected);
		}

		[Test]
		public void Test_FreeCells_2()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
.XX...X.XX
...XXX.XXX
XXX.......";
			var pars = new SimpleParameters()
			{
				FreeCellWeights = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
			};
			var expected = 22;

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
			var pars = new SimpleParameters()
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
			var pars = new SimpleParameters()
			{
				Holes = 1,
			};
			var expected = 16;

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
			var pars = new SimpleParameters()
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
			var pars = new SimpleParameters()
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
			var pars = new SimpleParameters()
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
			var pars = new SimpleParameters()
			{
				WallsLeft = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		
		[Test]
		public void TestWallsRight_4()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX......X";
			var pars = new SimpleParameters()
			{
				WallsRight = 1,
			};
			var expected = 4;

			Test(field, pars, expected);
		}

		[Test]
		public void TestWallsRightNoneOnTheFloor_0()
		{
			var field = @"
.........X
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX.......";
			var pars = new SimpleParameters()
			{
				WallsRight = 1,
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void TestComboPotential_0()
		{
			var field = @"
.........X
..X.......
.X.XXXXXXX
XXX...XXXX
X..XXX.XXX
XXX.......";
			var pars = new SimpleParameters()
			{
				ComboPotential = new int[] {  },
			};
			var expected = 0;

			Test(field, pars, expected);
		}

		[Test]
		public void TestComboPotential_1()
		{
			var field = @"
.........X
..X.......
XXX..XXXXX
XXX.X.XXXX
X..XXX.XXX
XXX.......";
			var pars = new SimpleParameters()
			{
				ComboPotential = new int[] { 1 },
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void TestComboPotential_2()
		{
			var field = @"
.........X
..X.......
XXX..XXXXX
XXX...XXXX
X..XXX.XXX
XXX.......";
			var pars = new SimpleParameters()
			{
				ComboPotential = new int[] { 0, 1 },
			};
			var expected = 2;

			Test(field, pars, expected);
		}
		[Test]
		public void TestComboPotential_3()
		{
			var field = @"
.........X
........XX
XXX..XXXXX
XXX...XXXX
...XXXXXXX
XXX.......";
			var pars = new SimpleParameters()
			{
				ComboPotential = new int[] { 0, 0, 1 },
			};
			var expected = 3;

			Test(field, pars, expected);
		}

		[Test]
		public void LostScore_None_Negative()
		{
			var evaluator = new SimpleEvaluator();
			var act = evaluator.LostScore;
			var exp = short.MinValue;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void WinScore_None_Postive()
		{
			var evaluator = new SimpleEvaluator();
			var act = evaluator.WinScore;
			var exp = short.MaxValue;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void DrawScore_None_RoundZero()
		{
			var evaluator = new SimpleEvaluator();
			var act = evaluator.DrawScore;
			var exp = 0;
			Assert.AreEqual(exp, act);
		}

		private static void Test(string str, SimpleParameters pars, int expected)
		{
			var field = Field.Create(0, 0, str);
			var evaluator = new SimpleEvaluator() { Parameters = pars };
			var actual = evaluator.GetScore(field);
			Assert.AreEqual(expected, actual);
		}
	}
}
