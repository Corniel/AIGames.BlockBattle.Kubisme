using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Generation
{
	[TestFixture]
	public class MoveGeneratorTest
	{
		[Test]
		public void HasReachableHoles_HolesNotReachable_False()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
XXXXXX.XXX
XXXXX..XX.
XXXXXX.XXX
XXXXXX.XX.
");
			var act = MoveGenerator.HasReachableHoles(field);

			Assert.IsFalse(act);
		}


		[Test]
		public void HasReachableHoles_HolesReachable_True()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
XXXXXX..XX
XXXXX...X.
XXXXXX..XX
XXXXXX..X.
");
			var act = MoveGenerator.HasReachableHoles(field);

			Assert.IsTrue(act);
		}

		[Test]
		public void HasReachableHoles_HolesNotReachableBecauseTurn_False()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
XXXXXX...X
XXXX...XXX
XXXXXX..XX
XXXXXX..X.
");
			var act = MoveGenerator.HasReachableHoles(field);

			Assert.IsFalse(act);
		}
	}
}
