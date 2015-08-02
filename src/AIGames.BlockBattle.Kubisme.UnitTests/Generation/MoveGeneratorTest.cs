using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Generation
{
	[TestFixture]
	public class MoveGeneratorTest
	{
		private static readonly Field Small = Field.Create(0, 0, @"
..........
..........
..........
..........
..........");

		private static readonly Field SmallFilled = Field.Create(0, 0, @"
..........
..........
..........
..........
....X.....
....X.....
...XX.....");

		[Test]
		public void GetMoves_SmallO_9candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.O, Position.Start, true).ToList();

			var expPath = new string[]
			{
				"left,left,left,left,drop",
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",
			};
			var expField = new string[]
			{
				"..........|..........|..........|XX........|XX........",
				"..........|..........|..........|.XX.......|.XX.......",
				"..........|..........|..........|..XX......|..XX......",
				"..........|..........|..........|...XX.....|...XX.....",
				"..........|..........|..........|....XX....|....XX....",
				"..........|..........|..........|.....XX...|.....XX...",
				"..........|..........|..........|......XX..|......XX..",
				"..........|..........|..........|.......XX.|.......XX.",
				"..........|..........|..........|........XX|........XX",
			};

			var actPath = candiates.Select(c => c.Path.ToString()).ToArray();
			var actField = candiates.Select(c => c.Field.ToString()).ToArray();

			CollectionAssert.AreEqual(expPath, actPath);
			CollectionAssert.AreEqual(expField, actField);
		}

		[Test]
		public void GetMoves_SmallI_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.I, Position.Start, true).ToList();

			Assert.AreEqual(10 + 7, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallS_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.S, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallZ_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.Z, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);

			var expPath = new string[]
			{
				"left,left,left,left,drop",
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"turnleft,left,left,left,left,drop",
				"turnleft,left,left,left,drop",
				"turnleft,left,left,drop",
				"turnleft,left,drop",
				"turnleft,drop",
				"turnleft,right,drop",
				"turnleft,right,right,drop",
				"turnleft,right,right,right,drop",
				"turnleft,right,right,right,right,drop",
			};
			var expField = new string[]
			{
				"..........|..........|..........|XX........|.XX.......",
				"..........|..........|..........|.XX.......|..XX......",
				"..........|..........|..........|..XX......|...XX.....",
				"..........|..........|..........|...XX.....|....XX....",
				"..........|..........|..........|....XX....|.....XX...",
				"..........|..........|..........|.....XX...|......XX..",
				"..........|..........|..........|......XX..|.......XX.",
				"..........|..........|..........|.......XX.|........XX",
				"..........|..........|.X........|XX........|X.........",
				"..........|..........|..X.......|.XX.......|.X........",
				"..........|..........|...X......|..XX......|..X.......",
				"..........|..........|....X.....|...XX.....|...X......",
				"..........|..........|.....X....|....XX....|....X.....",
				"..........|..........|......X...|.....XX...|.....X....",
				"..........|..........|.......X..|......XX..|......X...",
				"..........|..........|........X.|.......XX.|.......X..",
				"..........|..........|.........X|........XX|........X.",
			};

			var actPath = candiates.Select(c => c.Path.ToString()).ToArray();
			var actField = candiates.Select(c => c.Field.ToString()).ToArray();

			CollectionAssert.AreEqual(expPath, actPath);
			CollectionAssert.AreEqual(expField, actField);
		}

		[Test]
		public void GetMoves_SmallJ_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.J, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallL_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.L, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallT_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.T, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledO_9candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.O, Position.Start, true).ToList();

			Assert.AreEqual(9, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledI_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.I, Position.Start, true).ToList();

			Assert.AreEqual(10 + 7, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledS_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.S, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledZ_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.Z, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledJ_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.J, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledL_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.L, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledT_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.T, Position.Start, true).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_AlmostFilledField_21candidates()
		{
			var field = Field.Create(0, 0, @"
..........
..........
XXXX.XX...
.XXXXXXXXX
XXXX..XX.X
XXX.XXXXXX
XXX.XXXXXX
XXX.XXXXXX
XXX.XXXXXX
XXX.XXXXXX
XXX.XXXXXX
XXX.XXXXXX
XXXXX.XXXX
.XXXX.XXXX");
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(field, Block.J, Position.Start, true).ToList();

			Assert.AreEqual(8 + 8 + 3 + 2, candiates.Count);
		}

		[Test]
		public void GetMoves_WithMoveUnder_21candidates()
		{
			var field = Field.Create(0, 0, @"
..XX......
.XX.......
");
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(field, Block.S, Position.Start, true).ToList();

			var act = candiates.Select(c => c.ToString()).ToArray();
			var exp = new string[]
			{
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"down,left",
			};

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetMoves_()
		{
			var field = Field.Create(0, 0, @"
.......XXX
.........X
.........X
X........X
XX.......X
XX.......X
XX......XX
XXX.....XX
XXXXXX..XX
XXXXXX.XXX
XXXXXX.XXX
XXX.XX.XXX
XXXXXX.XXX
XXXXXX.XXX
X.XXXXXXXX
XXXXXX.XXX");
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(field, Block.I, Position.Start, true).ToList();
		}

		[Test]
		public void GetPath_HolesReachable_4()
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
			var target =new Position(5,3);
			var act = MoveGenerator.GetPath(field, Block.T[Block.RotationType.Left], Position.Start, target);
			var exp = "turnleft,down,right,down,right,down,down,left";
			
			var applied = field.Apply(Block.T[Block.RotationType.Left], target);
			Console.WriteLine(applied);
			
			Assert.AreEqual(exp, act.ToString());
		}


		[Test]
		public void GetReachableHoles_HolesNotReachable_False()
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
			var act = MoveGenerator.GetReachableHoles(field, Block.I, Position.Start).ToList();
			var exp = new List<MoveCandiate>();
			CollectionAssert.AreEqual(exp, act);
		}


		[Test]
		public void GetReachableHoles_HolesReachable_4()
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
			var act = MoveGenerator.GetReachableHoles(field, Block.T, Position.Start).Select(c => c.Path.ToString()).ToArray();
			var exp = new string[]
			{
				"turnleft,down,down,right,down,left"
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetReachableHoles_HolesNotReachableBecauseTurn_False()
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
			var act = MoveGenerator.GetReachableHoles(field, Block.O, Position.Start).ToList();
			var exp = new List<MoveCandiate>();
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
