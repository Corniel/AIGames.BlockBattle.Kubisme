using AIGames.BlockBattle.Kubisme.UnitTests.Evaluation;
using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTest
	{
		[Test, Category(Category.IntegrationTest)]
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

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}

		[Test, Category(Category.IntegrationTest)]
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

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_6LinesGarbage_LogResults()
		{
			var field = Field.Create(0, 0, 0,@"
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

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_1LinesLeft_LogResults()
		{
			var field = Field.Create(0, 0,0, @"
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

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}

		[Test, Category(Category.IntegrationTest)]
		public void GetMove_DontCreateHole_LogResults()
		{
			// Round 17, Points: 06
			//
			// 72.18 0.000s (1 depth) 0.0kN (82.2kN/s): left,left,left,drop
			// 95.85 0.003s (2 depth) 0.7kN (205.8kN/s): left,left,left,drop
			// 145.21 0.011s (3 depth) 10.1kN (882.7kN/s): left,drop
			// 212.88 0.142s (4 depth) 79.7kN (561.2kN/s): left,drop
			// 217.67 0.466s (5 depth) 331.9kN (711.7kN/s): left,drop
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

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}

	}
}
