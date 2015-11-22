using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class OpponentEvaluatorTest
	{
		[Test]
		public void Evaluate_SomeField_GetValues()
		{
			var field = Field.Create(13, 1, 0, @"
				..........
				..........
				..........
				XX.XXXXXXX
				XX.XXXXXXX
				XX.XXXXXXX
				XX.XXXX.XX
				XX.XXXXXXX
				XXX..X.XX.");

			var evalator = new OpponentEvaluator() { Generator = new MoveGenerator() };

			var act = evalator.Evaluate(field, Block.I, Block.O);
			var exp = new OpponentEvaluation(13, 20, 20, 6, 4);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Evaluate_FieldWithFirstFilled0_GetValues()
		{
			var field = Field.Create(13, 1, 0, @"
				.........X
				.........X
				.......XX.
				XX.XXXXXXX");

			var evalator = new OpponentEvaluator() { Generator = new MoveGenerator() };

			var act = evalator.Evaluate(field, Block.O, Block.I);
			var exp = new OpponentEvaluation(13, 13, 13, 0, 1);

			Assert.AreEqual(exp, act);
		}
	}
}
