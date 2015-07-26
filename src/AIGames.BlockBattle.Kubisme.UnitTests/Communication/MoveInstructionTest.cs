using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication
{
	[TestFixture]
	public class MoveInstructionTest
	{
		[Test]
		public void Ctor_None_NoMoves()
		{
			var instruction = new MoveInstruction();
			var act = instruction.ToString();
			var exp = "no_moves";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Ctor_LeftDownDrop_LeftDrop()
		{
			var instruction = new MoveInstruction(ActionType.Left, ActionType.Down, ActionType.Drop);
			var act = instruction.ToString();
			var exp = "left,drop";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Ctor_LeftDown_LeftDrop()
		{
			var instruction = new MoveInstruction(ActionType.Left, ActionType.Down);
			var act = instruction.ToString();
			var exp = "left,drop";

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Ctor_TurnRightLeftDownDownDownDown_TurnRightLeftDrop()
		{
			var instruction = new MoveInstruction(ActionType.TurnRight, ActionType.Left, ActionType.Down, ActionType.Down, ActionType.Down, ActionType.Down, ActionType.Down, ActionType.Down, ActionType.Down, ActionType.Down);
			var act = instruction.ToString();
			var exp = "turnright,left,drop";

			Assert.AreEqual(exp, act);
		}
	}
}
