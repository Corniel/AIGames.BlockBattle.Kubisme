using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class DecisionMakerTest
	{
		[Test]
		public void GetMove_EmptyBoard_()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = SimpleParameters.GetDefault(),
				},
				Generator = new MoveGenerator(),
			};

			var field = Field.Create(0, 0, @"
..........
..........
..........");
			var path = dm.GetMove(field, new Position(4, -1), Block.O, Block.L);

			var act = path.ToString();
			var exp = "right,right,right,right,drop";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_BoardWithRowToClear_()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = SimpleParameters.GetDefault(),
				},
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

			var act = path.ToString();
			var exp = "turnleft,left,drop";

			Assert.AreEqual(exp, act);
		}
	}
}
