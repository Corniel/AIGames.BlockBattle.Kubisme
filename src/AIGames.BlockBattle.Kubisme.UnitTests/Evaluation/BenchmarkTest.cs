﻿using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class BenchmarkTest
	{
		[Test, Category(Category.Benchmark)]
		public void Benchmark_Initial()
		{
			RunBenchmark(
				1, TimeSpan.FromSeconds(10), Block.J, Block.L,
				0, 0, 0, Field.Empty.ToString(),
				0, 0, 0, Field.Empty.ToString());
		}

		[Test, Category(Category.Benchmark)]
		public void Benchmark_FilledWith6EmptyRows()
		{
			RunBenchmark(
				1, TimeSpan.FromSeconds(0.5), Block.J, Block.L,
				0, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				.XXX..XX..
				.XXXXXXX..
				XXXXXXXX..",
				33, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				.........X
				.....XXXXX
				.....XXXXX
				.X..XXXXXX
				XX..XXXXXX
				XX.XXXXXXX
				XX.XXXXXXX
				XXXXXXXXX.");
		}

		[Test, Category(Category.Bug)]
		public void AlmostDieing_ChoosingToLose()
		{
			RunBenchmark(
				57, TimeSpan.FromSeconds(0.5), Block.J, Block.L,
				38, 0, 0, @"
				..........
				..........
				.XXX..XX..
				.XXXXXXX..
				XXXXXXXX..
				XX.XXXXX.X
				XXXXXXXX.X
				XXXXXXXX.X
				XXXXXXX.XX
				XXXXX.XXXX
				X.XXXXX.XX
				X.XXXXXXXX
				XXXXXX.X.X
				XXXXXXX.XX
				XX.XXXX.XX
				X.XXXXXXXX
				.XXXXXXX.X",
				33, 0, 0, @"
				..........
				..........
				..........
				..........
				.........X
				.....XXXXX
				.....XXXXX
				.X..XXXXXX
				XX..XXXXXX
				XX.XXXXXXX
				XX.XXXXXXX
				XXXXXXXXX.
				XXXXXXXX.X
				XXX.XX.XXX
				XXX.XXXXXX
				XXX.XXXX.X
				XX.XXXXXXX");
		}

		[Test, Category(Category.Bug)]
		public void MissingIDrop()
		{
			RunBenchmark(
				54, TimeSpan.FromSeconds(1.0), Block.I, Block.T,
				26, 0, 0, @"
				..........
				..........
				..........
				.........X
				XX......XX
				XX...XX.XX
				XX...XXXXX
				XX..XXXXXX
				XX..XXXXXX
				XXX.XXXXXX
				XXX.XXXXXX
				XXX..XXXXX
				XXX.XXXXXX
				XXX.XXXX.X
				XXX.XXXXXX
				XXX.XXX.XX
				XXXXXXX.XX",
				8, 1, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				.X...X....
				XX.XXXXXXX
				XX..XXXXXX
				XXXX.XXXXX");
		}

		[Test, Category(Category.Bug)]
		public void DivideByZeroException_MissingChildrenOnNode()
		{
			RunBenchmark(
				69, TimeSpan.FromSeconds(1.0), Block.T, Block.T,
				47, 1, 2, @"
				..........
				..........
				..........
				X.....XX..
				X.X...XXXX
				XXXX.XXXXX
				X.XXXXXXXX
				XXXX.XXXXX
				XXXXX..XXX
				XXXX.XXXXX
				XX.XX.XXXX
				XXXXXXX.XX
				XX.XXX.XXX
				X.XXXXXXXX
				.XXXXXX.XX
				XXXXXXX.XX",
				38, 0, 0, @"
				..........
				XXXXX.....
				.X.XXXXXXX
				XX.XXXXXXX
				XX.XXXXXXX
				XXXXXXXX.X
				XXXXXXXX.X
				..XXXXXXXX
				XXXXXXX.XX
				XXX.X.XXXX
				X.XXXXXXXX
				XXXXXX..XX
				XXXXXXX.XX
				X.XXXXXXX.
				XX.XXXXXXX
				XXXXX..XXX");
		}

		[Test, Category(Category.Bug)]
		public void CreatesABigHole()
		{
			RunBenchmark(
				17, TimeSpan.FromSeconds(1.0), Block.Z, Block.Z,
				13, 0, 1, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				........XX
				.......XXX
				......XXXX
				X.X...XXXX
				XXXX.XXXXX
				XXXXX.X.XX",
				3, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..X.......
				..XXXX....
				..XXXXX...
				..XXXXX...
				..XXXXXX..
				..XXXXXX..
				.XXXXXXX..
				XXXXXXXXX.
				XXXXXXX.XX
				XXXXXXX.XX
				XXXXXX.X.X
				XXX.XXXXXX");
		}

		[Test, Category(Category.Bug)]
		public void Skip_Instead_Of_Tetris()
		{
			RunBenchmark(
				58, TimeSpan.FromSeconds(1), Block.I, Block.J,
				47, 0, 2, @"
				..........
				..........
				...X......
				XXXXXXXXX.
				XXXXXXXXX.
				XXXXXXXXX.
				XXXXXXXXX.
				XX..XXXXXX
				XXXXXXXX.X
				XXX.XXXXX.
				.XXXXXXXXX
				XX.XXXXXX.
				X.XXXXXXXX
				XXXXX.XX.X
				XXXXXXXXX.
				XXXXXXXX..
				XXXXXXXXX.",
				36, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				....X.....
				XXXXX.....
				XXXXXXX.X.
				XXXXXXX.X.
				XXXXXX.XX.
				XXXXXXXX.X
				XXXX.XXX.X
				XXXXXXXX.X
				XXXXX.XX.X
				XXXXXXX.XX
				XX.XXXXX.X");
		}

		[Test, Category(Category.Bug)]
		public void Creates_Hole_Of_2_With_No_Clear_Reason()
		{
			var actual = RunBenchmark(
				7, TimeSpan.FromSeconds(1), Block.I, Block.J,
				0, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				...X..XXXX
				.XXXXXXXXX",
				0, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				......X...
				.....XXXXX
				XXXX.XXXXX
				XXXX.XXXXX");

			Assert.AreNotEqual("drop", actual.ToString());
		}

		[Test, Category(Category.Bug)]
		public void IndexOufOfArrayException_Evaluation_GetScore()
		{
			var actual = RunBenchmark(
				27, TimeSpan.FromSeconds(1), Block.J, Block.I,
				10, 0, 1, @"
				.......X.X
				XX....XXXX
				XX....XXXX
				XX....XXXX
				XX...XXXXX
				XX..XXXXXX
				XX..XX.XXX
				X...XXXXXX
				XXX.XXX.XX
				XXX.XXXXXX
				XXX.XXXXXX
				XXX.XXXXXX
				X.XXXXX.XX
				XXXXXX.XXX
				.XX.XXXXXX
				XXXX.XXXXX
				XXXXXXX..X
				XXX.XXXXXX
				.XXXXX.XXX",
				23, 1, 2, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				.X........
				.X........
				XXX.......
				XXXX......
				XXXXX..X..
				XXXXX.XXXX
				..XXXXXXXX
				XX.XXXXXXX
				X.XXX.XXXX");

			Assert.AreNotEqual("no_moves", actual.ToString());
		}

		[Test, Category(Category.Bug)]
		public void Does_not_play_a_clearing_move()
		{
			var actual = RunBenchmark(
				45, TimeSpan.FromSeconds(1), Block.I, Block.S,
				31, 0, 0, @"
				..........
				..........
				..X.......
				XXXX......
				XXXXX...XX
				XXXXX..XXX
				XXXXX..XXX
				XXXXXX.XXX
				XXXXXX.XXX
				X.XXX.XXXX
				.XXXXXXXXX
				XXXX.XXX.X
				XXXXXX.XXX
				XXXXX.XX.X
				.XXXXXXXXX
				XX.XX.XXXX
				XXXXXX.XXX
				X.XX.XXXXX",
				39, 1, 2, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				X.XX...XX.
				XXXX...XX.
				XXXXXX.XXX
				XXXXXX.XX.
				XXXXX.XXXX
				XXXXXX.X.X
				XXXXXX.XXX
				XXXXXX..XX
				XXX.XXXXXX");

			Assert.AreEqual("down,down,right,down,right,turnleft,drop", actual.ToString());
		}

		[Test, Category(Category.Bug)]
		public void Creates_a_hole_where_filling_a_gap_was_possible()
		{
			var actual = RunBenchmark(
				10, TimeSpan.FromSeconds(1), Block.L, Block.S,
				0, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				X.........
				X......X..
				X.XXX..XXX
				XXXXX...XX
				XXXXXX.XXX",
				0, 0, 0, @"
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				..........
				X.........
				X.........
				X.........
				X........X
				XXXX....XX
				XXXXXX...X
				XXXXXXX.XX
				XXXXXXX.XX");

			Assert.AreEqual("turnleft,turnleft,left,left,drop", actual.ToString());
		}


		private static MoveInstruction RunBenchmark(
			int round, TimeSpan duration, Block current, Block next,
			int pt0, int combo0, int skips0, string field0,
			int pt1, int combo1, int skips1, string field1)
		{

			var bot = new BenchmarkBot(current, next, round)
			{
				Field = Field.Create(pt0, combo0, skips0, field0),
				Opponent = Field.Create(pt1, combo1, skips1, field1),
			};
			var response = bot.GetResponse(duration);
			Console.WriteLine(response.Move);
			Console.WriteLine(response.Log);
			return response.Move;
		}
	}
	public class BenchmarkBot : KubismeBot
	{
		private readonly Block c;
		private readonly Block n;
		public BenchmarkBot(Block current, Block next, int round)
			: base(new MT19937Generator(round))
		{
			State = new GameState()
			{
				Round = round,
			};
			c = current;
			n = next;
		}
		public override Block Next { get { return n; } }
		public override Block Current { get { return c; } }

		protected override void SetDuration(TimeSpan time)
		{
			DecisionMaker.MinimumDuration = time;
			DecisionMaker.MaximumDuration = time;
		}
	}
}
