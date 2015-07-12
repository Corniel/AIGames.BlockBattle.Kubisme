using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
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
			var candiates = generator.GetMoves(Small, Block.O, Position.Start).ToList();

			Assert.AreEqual(9, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallI_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.I, Position.Start).ToList();

			Assert.AreEqual(10 + 7, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallS_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.S, Position.Start).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallZ_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.Z, Position.Start).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallJ_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.J, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallL_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.L, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallT_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(Small, Block.T, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledO_9candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.O, Position.Start).ToList();

			Assert.AreEqual(9, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledI_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.I, Position.Start).ToList();

			Assert.AreEqual(10 + 7, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledS_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.S, Position.Start).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledZ_17candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.Z, Position.Start).ToList();

			Assert.AreEqual(9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledJ_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.J, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_SmallFilledL_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.L, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}
		[Test]
		public void GetMoves_SmallFilledT_34candidates()
		{
			var generator = new MoveGenerator();
			var candiates = generator.GetMoves(SmallFilled, Block.T, Position.Start).ToList();

			Assert.AreEqual(9 + 8 + 9 + 8, candiates.Count);
		}

		[Test]
		public void GetMoves_AlmostFilledField_17candidates()
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
			var candiates = generator.GetMoves(field, Block.J, Position.Start).ToList();

			Assert.AreEqual(8 + 8, candiates.Count);
		}	
	}
}
