using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture, Category(Category.Benchmark)]
	public class BenchmarkTest
	{
		[OneTimeSetUp]
		public void SetUp()
		{
			RunBenchmark(
				1, TimeSpan.FromSeconds(0), Block.O, Block.O,
				0, 0, 0, Field.Empty.ToString(),
				0, 0, 0, Field.Empty.ToString());
		}

		[Test]
		public void Initial()
		{
			RunBenchmark(
				1, TimeSpan.FromSeconds(10), Block.J, Block.L,
				0, 0, 0, Field.Empty.ToString(),
				0, 0, 0, Field.Empty.ToString());
		}

		[Test]
		public void FilledWith6EmptyRows()
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

		[Test]
		public void FieldWithComplexPaths()
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

		private static void RunBenchmark(
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
		}
	}
	public class BenchmarkBot : KubismeBot
	{
		private Block c;
		private Block n;
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
