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

			var act = evalator.Evaluate(field, Block.I);
			var exp = new OpponentEvaluation(6, 2, 13);

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

			var act = evalator.Evaluate(field, Block.O);
			var exp = new OpponentEvaluation(0, 0, 13);

			Assert.AreEqual(exp, act);
		}
	}
}
