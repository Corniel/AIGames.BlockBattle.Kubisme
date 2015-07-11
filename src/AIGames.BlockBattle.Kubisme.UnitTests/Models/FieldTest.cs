using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class FieldTest
	{
		[Test]
		public void Create_WithLockedRow_RowCount4()
		{
			var act = Field.Create(0, 0, @"
..........
..........
..........
.......XX.
##########
");
			var exp = 4;
			Assert.AreEqual(exp, act.RowCount);
		}

		[Test]
		public void Apply_S_Added()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
.......XX.
");
			var act = field.Apply(Block.S, new Position(5, 2));
			var exp = "..........|..........|......XX..|.....XXXX.";
			Assert.AreEqual(exp, act.ToString());
		}

		[Test]
		public void Apply_Ir_Added()
		{
			var field = Field.Create(12, 0, @"
..........
..........
..........
XXXXXX.XXX
XXXXX..XX.
XXXXXX.XXX
XXXXXX.XX.
");
			var act = field.Apply(Block.I.Variations[1], new Position(5, 3));
			var exp = "..........|..........|..........|..........|..........|XXXXX.XXX.|XXXXXXXXX.";
			Assert.AreEqual(exp, act.ToString());
			Assert.AreEqual(14, act.Points);
			Assert.AreEqual(1, act.Combo);
		}
	}
}
