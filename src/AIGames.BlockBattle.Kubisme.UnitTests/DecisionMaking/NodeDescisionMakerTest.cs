using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	public class NodeDescisionMakerTest
	{
		[Test]
		public void GetMove_BugOneToLeftToMuch_LeftDrop()
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
..........
..........
..........
..........
..........
..........
..........
X.......XX
XXX.....XX");
			var dm = new NodeDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					{
						FreeCellWeights = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 },
						Holes = 1,
						NeighborsVertical = 1,
						NeighborsHorizontal = 1,

					},
				},
				Points = new int[10],
				Generator = new MoveGenerator(),
				MaximumDepth = 1,
			};

			var act = dm.GetMove(field, Block.Z, Block.O, 1);
			var exp = BlockPath.Create(ActionType.Left, ActionType.Drop);

			var applied = dm.BestField.ToString();

			StringAssert.EndsWith("|..........|X.XX....XX|XXXXX...XX", applied);

			Assert.AreEqual(exp, act);
		}
	}
}
