using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var exp = "Points: 0, Combo: 0, Holes: 0, Blockades: 0, LastBlockades: 0, WallsLeft: 0, WallsRight: 0, Floor: 0, NeighborsHorizontal: 0, NeighborsVertical: 0, FreeCellWeights: {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, ComboPotential: {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}";
#else
			var exp = "AIGames.BlockBattle.Kubisme.SimpleParameters";
#endif
			Assert.AreEqual(exp, act);
		}
	}
}
