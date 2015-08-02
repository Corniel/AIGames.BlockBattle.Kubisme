﻿using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class DecisionMakerTest
	{
		[Test]
		public void GetMove_EmptyBoard_LeftLeftLeftLeftDrop()
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
			var path = dm.GetMove(field, Block.O, Block.L);

			var act = path;
			var exp = BlockPath.Create(ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Left, ActionType.Drop);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void GetMove_BoardWithRowToClear_()
		{
			var dm = new SimpleDecisionMaker()
			{
				Evaluator = new SimpleEvaluator()
				{
					Parameters = new SimpleParameters()
					{
						Points = 100
					},
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
			var path = dm.GetMove(field, Block.Z, Block.O);

			var act = path;
			var exp = BlockPath.Create(ActionType.TurnLeft, ActionType.Drop);

			Assert.AreEqual(exp, act);
			Assert.AreEqual(1, dm.BestField.Points);
		}
	}
}
