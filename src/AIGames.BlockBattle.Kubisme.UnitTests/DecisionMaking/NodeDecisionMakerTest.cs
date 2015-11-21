using AIGames.BlockBattle.Kubisme.UnitTests.Evaluation;
using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture, Category(Category.IntegrationTest)]
	public class NodeDecisionMakerTest
	{
		[Test]
		public void GetMove_With_LogResults()
		{
			var field = Field.Create(0, 0, 1, @"
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
.......XX.
X.......XX
XXX...X.XX
XXXX.XXXXX
XXXXX.XXXX
XXXX.XXXXX
X.XXXXXXXX
XXXXXXXX.X
XXXX.XXXXX");
			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator()
				{
					Pars = EvaluatorParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 7,
			};

			dm.GetMove(field, Block.T, Block.Z, 1);
		}

		[Test]
		public void GetMove_SmallBoard_LogResults()
		{
			var field = Field.Create(8, 1, 1, @"
..........
..........
..........
XXXXX.....");
			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator()
				{
					Pars = EvaluatorParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 10,
			};

			dm.GetMove(field, Block.O, Block.Z, 1);
		}

		[Test]
		public void GetMove_6LinesGarbage_LogResults()
		{
			var field = Field.Create(0, 0, 0, @"
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
XXXX.XXXXX
XXXXX.XXXX
XXXX.XXXXX
X.XXXXXXXX
XXXXXXXX.X
XXXX.XXXXX");
			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator()
				{
					Pars = EvaluatorParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 7,
			};

			dm.GetMove(field, Block.T, Block.T, 1);
		}

		[Test]
		public void GetMove_1LinesLeft_LogResults()
		{
			var field = Field.Create(0, 0, 0, @"
..........
XXXX.XXXXX
XXXXX.XXXX
XXXX.XXXXX
X.XXXXXXXX
XXXXXXXX.X
XXXX.XXXXX");
			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator()
				{
					Pars = EvaluatorParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 11,
			};

			dm.GetMove(field, Block.T, Block.T, 1);
		}

		[Test]
		public void GetMove_DontCreateHole_LogResults()
		{
			var field = Field.Create(0, 0, 0, @"
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
...XX..XX.
");
			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator()
				{
					Pars = EvaluatorParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 7,
			};

			dm.GetMove(field, Block.T, Block.T, 17);
		}
	}
}
