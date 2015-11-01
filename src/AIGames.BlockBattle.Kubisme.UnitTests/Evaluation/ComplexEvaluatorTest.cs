using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture, Category(Category.Evaluation)]
	public class ComplexEvaluatorTest
	{
		[Test]
		public void FreeFields_FieldWitholes_10c9c2c1c1c1()
		{
			var field = @"
..........
.....X.....
.X.XXXXXXX
.XX...X.XX
...XXX.XXX
XXX.......";
			var pars = new EvaluatorParameters()
			{
				EmptyCells = new int[] 
				{
					1000000, 
					100000,
					10000,
					1000,
					100,
					10,
					1,
				},
			};
			var expected = 10921110;

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
			var pars = new EvaluatorParameters() { Holes = 1 };
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void Holes_SimpleField_1()
		{
			var field = @"
.XX.......
.X.X......";
			var pars = new EvaluatorParameters() { Holes = 1 };
			var expected = 1;

			Test(field, pars, expected);
		}

		[Test]
		public void Walls_1ReachbleRight_1()
		{
			var field = @"
..........
..X.......
.X.XXXXXXX
XXX...X.XX
X..XXX.XXX
XXX......X";
			var pars = new EvaluatorParameters()
			{
				Walls = 1,
			};
			var expected = 1;

			Test(field, pars, expected);
		}
		[Test]
		public void Walls_2ReachbleLeft_2()
		{
			var field = @"
..........
X.X.......
XX.XXXXXX.
XXX...X.XX
X..XXX.XXX
XXX......X";
			var pars = new EvaluatorParameters()
			{
				Walls = 1,
			};
			var expected =2;

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
				UnreachableStaffle = 1,
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
				UnreachableStaffle = 1,
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
				UnreachableStaffle = 1,
			};
			var expected = 2;

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
