using AIGames.BlockBattle.Kubisme.DecisionMaking;
using AIGames.BlockBattle.Kubisme.Evaluation;
using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class DecisionMakerTest
	{
		[Test]
		public void GetMove_EmptyBoard_()
		{
			var dm = new DecisionMaker()
			{
				Evaluator = new SimpleEvaluator(),
				Generator = new MoveGenerator(),
			};

			var field = Field.Create(0, 0, @"
..........
..........
..........");
			var path = dm.GetMove(field, new Position(4, -1), Block.O, Block.L);
		}

		[Test]
		public void GetMove_BoardWithRowToClear_()
		{
			var dm = new DecisionMaker()
			{
				Evaluator = new SimpleEvaluator(),
				Generator = new MoveGenerator(),
			};

			var field = Field.Create(0, 0, @"
..........
..........
..........
..........
..........
XXX.XXXXXX");
			var path = dm.GetMove(field, new Position(4, -1), Block.Z, Block.O);

			Assert.AreEqual(Block.RotationType.Left, path.Option);
			Assert.AreEqual(new Position(3, 3), path.Target);
		}
	}
}
