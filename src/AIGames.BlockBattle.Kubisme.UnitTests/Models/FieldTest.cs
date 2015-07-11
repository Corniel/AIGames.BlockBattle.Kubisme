using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;
using System;

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
		public void LockRows_2_FieldWithLessRows()
		{
			var field = Field.Create(0, 0, @"
..........
..........
........X.
.......XX.
");
			var act = field.LockRows(2);
			var exp = Field.Create(0, 0, @"
........X.
.......XX.
"); 
			Assert.AreEqual(exp.ToString(), act.ToString());
		}

		[Test]
		public void LockRows_3_EndOfGame()
		{
			var field = Field.Create(12, 3, @"
..........
..........
........X.
.......XX.
");
			var act = field.LockRows(3);
			
			Assert.AreEqual(0, act.RowCount, "RowCount");
			Assert.AreEqual(12, act.Points, "Points");
			Assert.AreEqual(0, act.Combo, "Combo");
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


		[Test]
		public void Apply_WithNegativeColumnPosition_Successful()
		{
			var field = Field.Create(0, 0, @"
..........
..........
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var block = Block.T[Block.RotationType.Right];
			var act = field.Apply(block, new Position(-1, 1));
			var exp = Field.Create(1, 1, @"
..........
..........
X.........
XX....X..X
XX.XXXXXXX");
			Console.WriteLine(act);
			Assert.AreEqual(exp.ToString(), act.ToString());

		}
	}
}
