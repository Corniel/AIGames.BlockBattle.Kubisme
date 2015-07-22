using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
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
				ComboPotential = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
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
				ComboPotential = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
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
				ComboPotential = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
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
				ComboPotential = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
			};
			var expected = 3;

			Test(field, pars, expected);
		}

		[Test]
		public void Row7ComboPotential_All_Matches()
		{
			var act = SimpleEvaluator.Row7ComboPotential.Select(r => new Row((ushort)r).ToString()).ToArray();
			var exp = new string[] 
			{ 
				"...XXXXXXX",
				"X...XXXXXX",
				"XX...XXXXX",
				"XXX...XXXX",
				"XXXX...XXX",
				"XXXXX...XX",
				"XXXXXX...X",
				"XXXXXXX...",
			};
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void Row8ComboPotential_All_Matches()
		{
			var act = SimpleEvaluator.Row8ComboPotential.Select(r => new Row((ushort)r).ToString()).ToArray();
			var exp = new string[] 
			{ 
				"..XXXXXXXX",
				"X..XXXXXXX",
				"XX..XXXXXX",
				"XXX..XXXXX",
				"XXXX..XXXX",
				"XXXXX..XXX",
				"XXXXXX..XX",
				"XXXXXXX..X",
				"XXXXXXXX..",
			};
			CollectionAssert.AreEqual(exp, act);
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
