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
		public void LockRow_None_FieldWithLessRows()
		{
			var field = Field.Create(0, 0, @"
..........
........X.
.......XX.
");
			var act = field.LockRow();
			var exp = Field.Create(0, 0, @"
........X.
.......XX.
"); 
			Assert.AreEqual(exp.ToString(), act.ToString());
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
			AssertField(exp, 0, 0, 2, act);
		}

		[Test]
		public void Apply_I_Added()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
.......XX.
");
			var act = field.Apply(Block.I, new Position(0, 2));
			var exp = "..........|..........|..........|XXXX...XX.";
			AssertField(exp, 0, 0, 3, act);
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
			AssertField(exp, 15, 1, 5, act);
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
			AssertField(exp.ToString(), 1, 1, 2, act);
		}

		[Test]
		public void Garbage_TwoRows_AppliedSuccessful()
		{
			var field = Field.Create(0, 0, @"
..........
..........
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var act = field.Garbage(Row.Garbage[0], Row.Garbage[1]);

			AssertField("......X..X|.XXXXXXXXX|XX.XXXXXXX|.XXXXXXXXX|X.XXXXXXXX", 0, 0, 0, act);


		}

		private static void AssertField(string str, int points, int combo, int freeRows, Field act)
		{
			Assert.AreEqual(str, act.ToString());
			Assert.AreEqual(points, act.Points, "Points");
			Assert.AreEqual(combo, act.Combo, "Combo");
			Assert.AreEqual(freeRows, act.FirstFilled, "Free rows");
		}
	}
}
