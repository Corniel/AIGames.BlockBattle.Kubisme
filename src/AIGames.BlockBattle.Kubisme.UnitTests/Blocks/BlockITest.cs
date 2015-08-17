using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockITest
	{
		[Test]
		public void GetColumns_FieldWith1Row_7Columns()
		{
			var field = Field.Create(0, 0, "...........");
			var act = Block.I.GetColumns(field).ToArray();
			var exp = new int[]{ 0, 1, 2, 3, 4, 5, 6 };
			CollectionAssert.AreEqual(exp, act);

		}

		[Test]
		public void GetColumns_FieldWith1RowsWithBlockades_3Columns()
		{
			var field = Field.Create(0, 0, "..X......X");
			var act = Block.I.GetColumns(field).ToArray();
			var exp = new int[] { 3, 4, 5 };
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
