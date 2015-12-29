using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class FieldTest
	{
		[Test]
		public void Create_Field_InitializedField()
		{
			var act = Field.Create(0, 0, 17, @"
..........
.......XX.
");
			FieldAssert.AreEqual("..........|.......XX.", 0, 0, 17, act);
		}
		[Test]
		public void Create_FromState_InitializedField()
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
			FieldAssert.AreEqual("..........|........X.|.......XX.", 10, 12, 0, act);
		}

		[Test]
		public void Count_None_24FilledCells()
		{
			var field = Field.Create(0, 0, 17, @"
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
			var field = Field.Create(10, 3, 17, @"
..........
........X.
.......XX.");
			var act = field.LockRow();
			FieldAssert.AreEqual("........X.|.......XX.", 10, 3, 17, act);
		}
		[Test]
		public void LockRow_FieldWithouEmptyRpws_NoneField()
		{
			var field = Field.Create(10, 3, 17, @"
........X.
.......XX.");
			var act = field.LockRow();
			FieldAssert.IsNone(act);
		}

		[Test]
		public void Apply_S_Added()
		{
			var field = Field.Create(0, 0, 17, @"
..........
..........
..........
.......XX.
");
			var act = field.Apply(Block.S, new Position(5, 2));
			var exp = "..........|..........|......XX..|.....XXXX.";
			FieldAssert.AreEqual(exp, 0, 0, 17, act);
		}
		[Test]
		public void Apply_I_Added()
		{
			var field = Field.Create(0, 0, 17, @"
..........
..........
..........
.......XX.
");
			var act = field.Apply(Block.I, new Position(0, 3));
			var exp = "..........|..........|..........|XXXX...XX.";
			FieldAssert.AreEqual(exp, 0, 0, 17, act);
		}
		[Test]
		public void Apply_Ir_Added()
		{
			var field = Field.Create(12, 0, 17, @"
..........
..........
..........
XXXXXX.XXX
XXXXX..XX.
XXXXXX.XXX
XXXXXX.XX.
");
			var act = field.Apply(Block.I.Variations[1], new Position(6, 3));
			var exp = "..........|..........|..........|..........|..........|XXXXX.XXX.|XXXXXXXXX.";
			FieldAssert.AreEqual(exp, 15, 1, 17, act);
		}
		[Test]
		public void Apply_IClearRow_Added()
		{
			var field = Field.Create(0, 0, 2, "..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|........XX|.XX.....XX|XX......XX|XXXX....XX");

			var act = field.Apply(Block.I, new Position(4, 19));
			var exp = "..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|..........|........XX|.XX.....XX|XX......XX";
			FieldAssert.AreEqual(exp, 0, 0, 2, act);
		}
		[Test]
		public void Apply_FullClear_Adds18Points()
		{
			var field = Field.Create(6, 0, 17, @"
..........
...XXXXXXX
XX.XXXXXXX");

			var act = field.Apply(Block.J[Block.RotationType.Uturn], new Position(0, 1));
			var exp = "..........|..........|..........";
			FieldAssert.AreEqual(exp, 24, 1, 17, act);
		}


		[Test]
		public void Apply_TDoubleClear_NoTSpin()
		{
			var field = Field.Create(0, 0, 0, @"
				X.........
				X..XX.....
				XXXXXX...X
				XXXXXX..XX
				XXXXXX.XXX");

			var act = field.Apply(Block.T[Block.RotationType.Right], new Position(6, 2));

			FieldAssert.AreEqual(@"
				..........
				..........
				X.........
				X..XX.....
				XXXXXXX..X", 3, 1, 0, act);
		}


		[Test]
		public void Apply_TSingleClear_NoTSpin()
		{
			var field = Field.Create(0, 0, 0, @"
				X.........
				X..XX.....
				XXXXXX...X
				XXXX.X..XX
				XXXXXX.XXX");

			var act = field.Apply(Block.T[Block.RotationType.Right], new Position(6, 2));

			FieldAssert.AreEqual(@"
				..........
				X.........
				X..XX.....
				XXXXXXX..X
				XXXX.XXXXX", 0, 0, 0, act);
		}

		[Test]
		public void Apply_SingleClearWithCombo1_Adds0Points()
		{
			var field = Field.Create(8, 1, 17, @"
..........
....XXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.L[Block.RotationType.Uturn], new Position(0, 1));
			var exp = "..........|..........|XXX.XXXXXX";
			FieldAssert.AreEqual(exp, 8, 1, 17, act);
		}
		[Test]
		public void Apply_DoubleClear0_Adds3Points()
		{
			var field = Field.Create(7, 0, 17, @"
.........X
...XXXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.L[Block.RotationType.Uturn], new Position(0, 1));
			var exp = "..........|..........|.........X";
			FieldAssert.AreEqual(exp, 10, 1, 17, act);
		}
		[Test]
		public void Apply_TripleClear_Adds6Points()
		{
			var field = Field.Create(4, 0, 17, @"
..........
.XXXXXXXXX
.XXXXXXXXX
.XXXXXXXXX");

			var act = field.Apply(Block.I[Block.RotationType.Left], new Position(0, 0));
			var exp = "..........|..........|..........|X.........";
			FieldAssert.AreEqual(exp, 10, 1, 17, act);
		}
		[Test]
		public void Apply_QuadrupleClear_Adds10Points()
		{
			var field = Field.Create(8, 0, 17, @"
XX.......X
XXXXX.XXXX
XXXXX.XXXX
XXXXX.XXXX
XXXXX.XXXX");

			var act = field.Apply(Block.I[Block.RotationType.Left], new Position(5, 1));
			var exp = "..........|..........|..........|..........|XX.......X";
			FieldAssert.AreEqual(exp, 18, 1, 18, act);
		}
		[Test]
		public void Apply_WithNegativeColumnPosition_Successful()
		{
			var field = Field.Create(0, 0, 17, @"
..........
..........
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var block = Block.T[Block.RotationType.Right];
			var act = field.Apply(block, new Position(0, 1));
			Console.WriteLine(act);
			var exp = "..........|..........|X.........|XX....X..X|XX.XXXXXXX";
			FieldAssert.AreEqual(exp, 0, 0, 17, act);
		}

		[Test]
		public void SkipBlock_None_SkipOf3()
		{
			var exp = "..X..XX...|XXX...XXXX|XXXX.XXXXX";
			var field = Field.Create(1, 2, 4, exp);

			var act = field.SkipBlock();
			FieldAssert.AreEqual(exp, 1, 0, 3, act);
		}

		[Test]
		public void Garbage_TwoRows_AppliedSuccessful()
		{
			var field = Field.Create(0, 0, 17, @"
..........
..........
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var act = field.Garbage(Row.Garbage[0], Row.Garbage[1]);
			var exp = "......X..X|.XXXXXXXXX|XX.XXXXXXX|.XXXXXXXXX|X.XXXXXXXX";
			FieldAssert.AreEqual(exp, 0, 0, 17, act);
		}
		[Test]
		public void Garage_OneRow_NoSpace()
		{
			var field = Field.Create(0, 0, 17, @"
......X..X
.XXXXXXXXX
XX.XXXXXXX");

			var act = field.Garbage(Row.Garbage[2]);
			FieldAssert.IsNone(act);
		}

		[Test]
		public void Test_NoSpace_False()
		{
			var field = Field.Create(0, 0, 17, @"
......X..X");

			var act = field.Test(Block.O, 0, 0);
			var exp = Field.TestResult.False;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Test_FitWithoutSolidGround_Retry()
		{
			var field = Field.Create(0, 0, 17, @"
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
			var field = Field.Create(0, 0, 17, @"
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
			var field = Field.Create(0, 0, 17, @"
.........X
......X..X
XX....XX.X");

			var act = field.Test(Block.O, 0, 1);
			var exp = Field.TestResult.False;
			Assert.AreEqual(exp, act);
		}

		
		
	}

	
}
