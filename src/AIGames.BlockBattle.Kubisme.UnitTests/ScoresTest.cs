using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests
{
	[TestFixture]
	public class ScoresTest
	{
		[Test]
		public void Formatted_WinningValue_Plus4()
		{
			var score = Scores.Wins(4);
			var act = Scores.GetFormatted(score);
			var exp = "+oo 4";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void Formatted_LosingValue_Min3()
		{
			var score = Scores.Loses(3);
			var act = Scores.GetFormatted(score);
			var exp = "-oo 3";
			Assert.AreEqual(exp, act);
		}
	}
}
