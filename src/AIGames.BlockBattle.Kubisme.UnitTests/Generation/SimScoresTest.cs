using AIGames.BlockBattle.Kubisme.Genetics;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Generation
{
	[TestFixture]
	public class SimScoresTest
	{
		[Test]
		public void Sort_()
		{
			var scores0 = new SimScores() { SimScore.Win(5, 8), SimScore.Win(5, 8), SimScore.Win(5, 8)};
			var scores1 = new SimScores() { SimScore.Win(5, 8), SimScore.Win(5, 8), SimScore.Win(8, 8)};
			var scores2 = new SimScores() { SimScore.Win(5, 8), SimScore.Win(5, 8), SimScore.Win(5, 8), SimScore.Lost(1, 0), SimScore.Lost(1, 0), SimScore.Lost(1, 0) };
			var scores3 = new SimScores() { SimScore.Win(10, 8), SimScore.Win(10, 8), SimScore.Win(10, 8), SimScore.Win(10, 8), SimScore.Lost(1, 0), SimScore.Lost(1, 0), SimScore.Lost(1, 0) };
			var scores4 = new SimScores() { SimScore.Lost(1, 0), SimScore.Lost(2, 0), SimScore.Lost(1, 0) };
			var scores5 = new SimScores() { SimScore.Lost(1, 0), SimScore.Lost(1, 0), SimScore.Lost(1, 0) };

			var exp = new SimScores[] { scores0, scores1, scores2, scores3, scores4, scores5 };
			var act = new List<SimScores>() {  scores5, scores3, scores1 , scores4, scores2, scores0 };
			act.Sort();

			var expected = exp.Select(e => e.ToUnitTestString()).ToArray();
			var actual = act.Select(e => e.ToUnitTestString()).ToArray();

			CollectionAssert.AreEqual(expected, actual);
		} 
	}
}
