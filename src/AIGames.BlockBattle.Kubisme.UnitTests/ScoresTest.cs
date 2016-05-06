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

		[Test]
		public void Formatted_Zero_Formatted()
		{
			var score = 0;
			var act = Scores.GetFormatted(score);
			var exp = "=0.00";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Formatted_Minus1234_Formatted()
		{
			var score = -1234;
			var act = Scores.GetFormatted(score);
			var exp = "-12.34";
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Formatted_Plus1234_Formatted()
		{
			var score = 1234;
			var act = Scores.GetFormatted(score);
			var exp = "+12.34";
			Assert.AreEqual(exp, act);
		}
	}
}
