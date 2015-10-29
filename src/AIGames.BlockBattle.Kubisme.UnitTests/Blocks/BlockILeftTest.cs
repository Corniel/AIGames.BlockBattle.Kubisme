using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockILeftTest
	{
		[Test]
		public void GetColumns_FieldWith3Rows_0Columns()
		{
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var act = Block.I[Block.RotationType.Left].GetColumns(field).ToArray();
			var exp = new int[0];
			CollectionAssert.AreEqual(exp, act);

		}

		[Test]
		public void GetColumns_FieldWith5Rows_10Columns()
		{
			var field = Field.Create(0, 0, 0, "...........|...........|...........|...........|.X.....XX..");
			var act = Block.I[Block.RotationType.Left].GetColumns(field).ToArray();
			var exp = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			CollectionAssert.AreEqual(exp, act);

		}

		[Test]
		public void GetColumns_FieldWith5RowsWithBlockades_6Columns()
		{
			var field = Field.Create(0, 0, 0, "...........|...........|...........|.X.X...X...|.X.....XX..");
			var act = Block.I[Block.RotationType.Left].GetColumns(field).ToArray();
			var exp = new int[] { 2, 4, 5, 6, 8, 9 };
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessibles0_None_RightBitMask()
		{
			var act = BlockILeft.accessibles0.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXXX...",
				"XXXXXXX...",
				".XXXXXX...",
				"..XXXXX...",
				
				"...XXXX...",
				"...XXXX...",

				"...XXXXX..",
				"...XXXXXX.",
				"...XXXXXXX",
				"...XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessibles1_None_RightBitMask()
		{
			var act = BlockILeft.accessibles1.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XX........",
				".X........",
				"..X.......",
				"...X......",
				"....X.....",
				".....X....",
				"......X...",
				".......X..",
				"........X.",
				"........XX",
			};
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
