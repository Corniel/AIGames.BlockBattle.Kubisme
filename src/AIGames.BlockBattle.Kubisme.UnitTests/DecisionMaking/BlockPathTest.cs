using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.DecisionMaking;
using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture]
	public class BlockPathTest
	{
		[Test]
		public void Create_5Items_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnLeft, ActionType.TurnRight };
			var path = BlockPath.Create(exp);

			Assert.AreEqual(5, path.Count, "Count");
			CollectionAssert.AreEqual(exp, path.Moves.ToArray());
			Assert.AreEqual("left,right,drop,turnleft,turnright", path.ToString());
		}
		
		[Test]
		public void Create_25Items_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Drop, ActionType.TurnRight, ActionType.TurnRight };
			var path = BlockPath.Create(exp);

			Assert.AreEqual(25, path.Count, "Count");
			CollectionAssert.AreEqual(exp, path.Moves.ToArray());
			Assert.AreEqual("left,right,drop,turnleft,turnright,left,right,drop,turnleft,turnright,left,right,drop,turnleft,turnright,left,right,drop,turnleft,turnright,left,right,drop,turnright,turnright", path.ToString());
		}
	}
}
