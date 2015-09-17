using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class ComplexParametersTest
	{
		[Test]
		public void ToString_SimpleInstance_StartsWithSomeParameters()
		{
			var pars = new ComplexParameters();
			var act = pars.ToString();

#if DEBUG
			var exp = "Free: 0, Points: 0, Combo: 0, Holes: 0, WallsLeft: 0, WallsRight: 0, NeighborsHorizontal: 0, NeighborsVertical: 0";
#else
			var exp = "AIGames.BlockBattle.Kubisme.ComplexParameters";
#endif
			StringAssert.StartsWith(exp, act);
		}
	}
}
