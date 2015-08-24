using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockTUturnTest
	{
		[Test]
		public void TSpinTopMask_None_MasksMatch()
		{
			var act = BlockTUturn.TSpinTopMask.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"X.X.......",
				".X.X......",
				"..X.X.....",
				"...X.X....",

				"....X.X...",
				".....X.X..",
				"......X.X.",
				".......X.X",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Apply_Top_NoTSpin()
		{
			var field = Field.Create(0, 0,@"
XXX...XXXX
.XXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 0));

			var expField = "..........|.XXXXXXXXX";
			var expPt = 1;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1_NoTSpin()
		{
			var field = Field.Create(0, 0, @"
..X...X...
XXX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..........|..X...X...";
			var expPt = 3;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XXX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..........|..X..XX...";
			var expPt = 12;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1Clear1_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
.XX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|.XXXXXXXXX";
			var expPt = 6;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1Clear2_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XXX...XXXX
.XXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|.XXXXXXXXX";
			var expPt = 6;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}
	}
}
