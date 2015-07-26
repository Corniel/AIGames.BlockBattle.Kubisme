using NUnit.Framework;
using System;
using System.Diagnostics;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Opponent
{
	[TestFixture]
	public class PointsPredictorTest
	{
		[Test]
		public void GetPoints_EmptyField_0c0c0c1c3c6ect()
		{
			var predictor = new PointsPredictor();

			var sw = Stopwatch.StartNew();
			var act = predictor.GetPoints(Field.Empty, Block.Z, Block.S);
			Console.WriteLine(sw.Elapsed.TotalMilliseconds);

			var exp = new int[] { 0, 0, 0, 1, 3, 6, 10, 15, 21, 28 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetPoints_FieldWithCombos_0c0c0c1c3c6ect()
		{
			var field = Field.Create(10, 3, @"
..........
..........
..........
..........
.XXXXXXXXX
.XXXXXXXXX
.XXXXXXXXX
.XXXXXXXXX
XXXXX.XXXX");

			var predictor = new PointsPredictor();
			
			var sw = Stopwatch.StartNew();
			var act = predictor.GetPoints(field, Block.I, Block.T);
			Console.WriteLine(sw.Elapsed.TotalMilliseconds);
			
			var exp = new int[] { 10, 21, 26, 32, 39, 47, 56, 66, 77, 89 };

			CollectionAssert.AreEqual(exp, act);
		}
	}
}
