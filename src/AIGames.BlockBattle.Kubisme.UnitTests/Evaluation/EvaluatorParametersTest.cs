using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Evaluation
{
	[TestFixture]
	public class EvaluatorParametersTest
	{
		[Test]
		public void Calc_ComboPotential_SomeChecks()
		{
			var pars = new EvaluatorParameters()
			{
				Combos = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
			};
			pars.Calc();

			Assert.AreEqual(0, pars.ComboPotential[12, 0], "[12, 0]");
			Assert.AreEqual(3 + 4 + 5, pars.ComboPotential[1, 3], "[1, 3]");
		}
	}
}
