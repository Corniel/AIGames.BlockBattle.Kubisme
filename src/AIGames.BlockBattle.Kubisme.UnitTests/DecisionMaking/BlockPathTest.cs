using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking
{
	[TestFixture]
	public class BlockPathTest
	{
		[Test]
		public void Skips_1Item_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Skips };
			var act = BlockPath.Skips;

			Assert.AreEqual(1, act.Count, "Count");
			CollectionAssert.AreEqual(exp, act.Moves.ToArray());
			Assert.AreEqual("skips", act.ToString());
		}

		[Test]
		public void Create_5Items_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight };
			var act = BlockPath.Create(exp);

			Assert.AreEqual(5, act.Count, "Count");
			CollectionAssert.AreEqual(exp, act.Moves.ToArray());
			Assert.AreEqual("left,right,down,turnleft,turnright", act.ToString());
		}
		
		[Test]
		public void Create_25Items_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnRight, ActionType.TurnRight };
			var act = BlockPath.Create(exp);

			Assert.AreEqual(25, act.Count, "Count");
			CollectionAssert.AreEqual(exp, act.Moves.ToArray());
			Assert.AreEqual("left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnright,turnright", act.ToString());
		}

		[Test]
		public void Add_TurnLeftM0_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.TurnLeft };
			var path = BlockPath.Create(exp.Take(5).ToArray());
			var act = path.Add(ActionType.TurnLeft);

			Assert.AreEqual(6, act.Count, "Count");
			CollectionAssert.AreEqual(exp, act.Moves.ToArray());
			Assert.AreEqual("left,right,down,turnleft,turnright,turnleft", act.ToString());
		}

		[Test]
		public void Add_TurnRightM1_AreEqual()
		{
			var exp = new ActionType[] { ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnLeft, ActionType.TurnRight, ActionType.Left, ActionType.Right, ActionType.Down, ActionType.TurnRight, ActionType.TurnRight };
			var path = BlockPath.Create(exp.Take(24).ToArray());
			var act = path.Add(ActionType.TurnRight);

			Assert.AreEqual(25, act.Count, "Count");
			CollectionAssert.AreEqual(exp, act.Moves.ToArray());
			Assert.AreEqual("left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnleft,turnright,left,right,down,turnright,turnright", act.ToString());
		}
	}
}
