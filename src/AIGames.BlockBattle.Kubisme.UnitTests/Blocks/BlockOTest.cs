using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockOTest
	{
		[Test]
		public void GetColumns_FieldWith1Row_NoColumns()
		{
			var field = Field.Create(0, 0, 0, "...........");
			var act = Block.O.GetColumns(field).ToArray();
			var exp = new int[0];
			CollectionAssert.AreEqual(exp, act);

		}

		[Test]
		public void GetColumns_FieldWith2Rows_9Columns()
		{
			var field = Field.Create(0, 0, 0, "...........|...........");
			var act = Block.O.GetColumns(field).ToArray();
			var exp = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetColumns_FieldWith2RowsWithBlockades_2Columns()
		{
			var field = Field.Create(0, 0, 0, "..X...XX...|.XX.X..XXXX");
			var act = Block.O.GetColumns(field).ToArray();
			var exp = new int[] { 3, 4 };
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessibles_None_RightBitMask()
		{
			var act = BlockO.accessiblesO.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXX....",
				".XXXXX....",
				"..XXXX....",
				"...XXX....",
				
				"....XX....",

				"....XXX...",
				"....XXXX..",
				"....XXXXX.",
				"....XXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
