using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDecisionMakerTest
	{
		[Test, Category(Category.IntegrationTest)]
		public void GetMove_None_LogResults()
		{
			var field = Field.Create(0, 0, @"
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
				Evaluator = new SimpleEvaluator()
				{
					Parameters = SimpleParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 10,
			};

			dm.GetMove(field, Block.T, Block.Z, 1);

			foreach (var log in dm.Logs)
			{
				Console.WriteLine(log);
			}
		}
	}
}
