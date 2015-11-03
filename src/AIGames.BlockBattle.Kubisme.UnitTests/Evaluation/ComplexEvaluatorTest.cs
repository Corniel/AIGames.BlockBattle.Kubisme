using NUnit.Framework;
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
		public void TetrisPotential_TunnleOf5WithHickup_0()
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
		public void TetrisPotential_TunnleOf4_1()
		{
			var field = @"
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
