﻿using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Generation
{
	[TestFixture]
	public class MoveGeneratorTest
	{
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
			var candiates = generator.GetMoves(field, Block.J,  true).ToList();

			Assert.AreEqual(8 + 8 + 3 + 2, candiates.Count);
		}

		[Test]
		public void GetMoves_WithMoveUnder_6candidates()
		{
			var field = Field.Create(0, 0, @"
.XX.......
XX........
");
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(field, Block.S,  true).ToList();

			var act = candiates.Select(c => c.ToString()).ToArray();
			var exp = new string[]
			{
				"down,left",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",
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
			var candiates = generator.GetMoves(field, Block.I,  true).ToList();
		}

		[Test]
		public void GetPath_HolesReachable_PathEndingWithDownLeft()
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
			var act = MoveGenerator.GetReachableHoles(field, Block.I).ToList();
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
			var act = MoveGenerator.GetReachableHoles(field, Block.T).Select(c => c.Path.ToString()).ToArray();
			var exp = new string[]
			{
				"down,down,right,right,right,turnleft,down,down,left",
				"down,down,right,right,right,turnleft,down,down,left,turnleft",
				"down,down,right,right,right,turnleft,down,down,left,turnright"
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
			var act = MoveGenerator.GetReachableHoles(field, Block.O).ToList();
			var exp = new List<MoveCandiate>();
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetPaths_ToTwoHoles_Found()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........
...XXX....
....X.....
");
			var targets = new int[] { 0, 0, 0, 0, 0x28 };
			var candidates = new List<MoveCandiate>();
			foreach (var candidate in MoveGenerator.GetPaths(field, Block.J, targets, 4))
			{
				candidates.Add(candidate);

				Console.WriteLine(candidate.Field.ToString().Replace("|", Environment.NewLine));
				Console.WriteLine(candidate.Path);
				Console.WriteLine();
			}

			var act = candidates.Select(c => c.Field.ToString()).ToArray();

			var exp = new string[]
			{
				"..........|..........|..........|.X.XXX....|.XXXX.....",
				"..........|..........|......X...|...XXXX...|....XXX...",
			};
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
