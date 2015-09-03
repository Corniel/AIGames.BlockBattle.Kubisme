using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class SimpleParametersTest
	{
		[Test]
		public void ToString_SimpleInstance_()
		{
			var pars = new SimpleParameters();
			var act = pars.ToString();

#if DEBUG
			var exp = "Points: 0, Combo: 0, Holes: 0";
#else
			var exp = "AIGames.BlockBattle.Kubisme.SimpleParameters";
#endif
			StringAssert.StartsWith(exp, act);
		}
	}
}
