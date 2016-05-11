using NUnit.Framework;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture]
	public class BlockNodeTest
	{
		[Test]
		public void Apply_Move1Depth1_ShouldNotApplyALock()
		{
			var field = Field.Create(0, 0, 0, @"
				..........
				..X.......");

			var actual = BlockNode.Apply(field, 1, new ApplyParameters(new MT19937Generator()) { Round = 1 });

			FieldAssert.AreEqual(@"
				..........
				..X.......", 0, 0, 0, actual);
		}

		[Test]
		public void Apply_Move45Depth1_ShouldApplyALock()
		{
			var field = Field.Create(0, 0, 0, @"
				..........
				..X.......");

			var actual = BlockNode.Apply(field, 1, new ApplyParameters(new MT19937Generator()) { Round = 45 });

			FieldAssert.AreEqual(@"
				..X.......", 0, 0, 0, actual);
		}
	}
}
