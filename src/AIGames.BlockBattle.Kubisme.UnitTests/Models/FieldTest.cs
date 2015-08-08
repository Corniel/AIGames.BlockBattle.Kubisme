using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class FieldTest
	{
		[Test]
		public void Create_WithLockedRow_InitializedField()
		{
			var act = Field.Create(0, 0, @"
..........
.......XX.
##########
");
			AssertField("..........|.......XX.", 0, 0, 1, act);
		}
		[Test]
		public void Create_FromStateWithLockedRow_InitializedField()
		{
			var state = new GameState()
			{
				Player2 = new GameState.Player()
				{
					Combo = 12,
					Points = 10,
					Field = new int[,] 
					{ 
						{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
						{ 0, 0, 0, 0, 0, 0, 0, 0, 2, 0 },
						{ 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 },
						{ 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 },
					},
				},
			};
			var act = Field.Create(state, PlayerName.Player2);
			AssertField("..........|........X.|.......XX.", 10, 12, 1, act);
		}

		[Test]
		public void Count_None_24FilledCells()
		{
			var field = Field.Create(0, 0, @"
..........
.........X
.XXX..XXXX
...XXXXXXX
.XXXXXXXXX");

			Assert.AreEqual(24, field.Count);
		}

		[Test]
		public void LockRow_None_FieldWithLessRows()
		{
			var field = Field.Create(10, 3, @"
..........
........X.
.......XX.");
			var act = field.LockRow();
			AssertField("........X.|.......XX.", 10, 3, 0, act);
		}
		[Test]
		public void LockRow_FieldWithouEmptyRpws_NoneField()
		{
			var field = Field.Create(10, 3, @"
........X.
.......XX.");
			var act = field.LockRow();
			AssertFieldisNone(act);
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
		public void Apply_IClearRow_Added()
		{
			var field = Field.Create(0, 0, "..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|........XX|.XX.....XX|XX......XX|XXXX....XX");

			var act = field.Apply(Block.I, new Position(4, 18));
			AssertField("..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|........XX|.XX.....XX|XX......XX", 1, 1, 17, act);
		}
		[Test]
		public void Apply_FullClear_Adds24Points()
		{
			var field = Field.Create(6, 0, @"
..........
...XXXXXXX
XX.XXXXXXX");

			var act = field.Apply(Block.J[Block.RotationType.Uturn], new Position(0, 0));
			AssertField("..........|..........|..........", 30, 1, 3, act);
		}
		[Test]
		public void Apply_SingleClearWithCombo1_Adds2Points()
		{
			var field = Field.Create(8, 1, @"
..........
....XXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.L[Block.RotationType.Uturn], new Position(0, 0));
			AssertField("..........|..........|XXX.XXXXXX", 10, 2, 2, act);
		}
		[Test]
		public void Apply_DoubleClear0_Adds3Points()
		{
			var field = Field.Create(7, 0, @"
.........X
...XXXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.L[Block.RotationType.Uturn], new Position(0, 0));
			AssertField("..........|..........|.........X", 10, 1, 2, act);
		}
		[Test]
		public void Apply_TrippleClear_Adds6Points()
		{
			var field = Field.Create(4, 0, @"
..........
.XXXXXXXXX
.XXXXXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.I[Block.RotationType.Left], new Position(-1, 0));
			AssertField("..........|..........|..........|X.........", 10, 1, 3, act);
		}
		[Test]
		public void Apply_QuadrupleClear_Adds12Points()
		{
			var field = Field.Create(8, 0, @"
XX.......X
XXXXX.XXXX
XXXXX.XXXX
XXXXX.XXXX
XXXXX.XXXX");

			var act = field.Apply(Block.I[Block.RotationType.Left], new Position(4, 1));
			AssertField("..........|..........|..........|..........|XX.......X", 20, 1, 4, act);
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
		[Test]
		public void Garage_OneRow_NoSpace()
		{
			var field = Field.Create(0, 0, @"
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var act = field.Garbage(Row.Garbage[2]);
			AssertFieldisNone(act);
		}

		[Test]
		public void Test_NoSpace_False()
		{
			var field = Field.Create(0, 0, @"
......X..X");

			var act = field.Test(Block.O, 0, 0);
			var exp = Field.TestResult.False;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Test_FitWithoutSolidGround_Retry()
		{
			var field = Field.Create(0, 0, @"
.........X
......X..X
......XX.X");

			var act = field.Test(Block.T, 3, 0);
			var exp = Field.TestResult.Retry;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Test_FitOnBottom_True()
		{
			var field = Field.Create(0, 0, @"
.........X
......X..X
......XX.X");

			var act = field.Test(Block.O, 0, 1);
			var exp = Field.TestResult.True;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Test_WithOverlap_False()
		{
			var field = Field.Create(0, 0, @"
.........X
......X..X
XX....XX.X");

			var act = field.Test(Block.O, 0, 1);
			var exp = Field.TestResult.False;
			Assert.AreEqual(exp, act);
		}

		private static void AssertField(string str, int points, int combo, int freeRows, Field act)
		{
			Assert.AreEqual(str, act.ToString());
			Assert.AreEqual(points, act.Points, "Points");
			Assert.AreEqual(combo, act.Combo, "Combo");
			Assert.AreEqual(freeRows, act.FirstFilled, "Free rows");
		}
		private static void AssertFieldisNone(Field act)
		{
			AssertField("", -1, 0, 0, act);
		}
	}
}
