using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Communication
{
	[TestFixture]
	public class InstructionTest
	{
		#region Action

		[Test]
		public void Parse_RequestMoveInstruction_12345ms()
		{
			var act = Instruction.Parse("action moves 12345");
			var exp = new RequestMoveInstruction(TimeSpan.FromMilliseconds(12345));

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Settings

		[Test]
		public void Parse_YourBotInstruction_Player2()
		{
			var act = Instruction.Parse("settings your_bot player2");
			var exp = new YourBotInstruction(PlayerName.Player2);

			Assert.AreEqual(exp, act);
		}

		#endregion

		#region Update game

		[Test]
		public void Parse_ThisPieceInstruction_L()
		{
			var act = Instruction.Parse("update game this_piece_type L");
			var exp = new ThisPieceInstruction(PieceType.L);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Parse_NextPieceInstruction_I()
		{
			var act = Instruction.Parse("update game next_piece_type I");
			var exp = new NextPieceInstruction(PieceType.I);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Parse_ThisPiecePositionInstruction_Row4ColMin1()
		{
			var act = Instruction.Parse("update game this_piece_position 4,-1");
			var exp = new ThisPiecePositionInstruction(new Position(4, -1));

			Assert.AreEqual(exp, act);
		}
		#endregion

		#region Update player

		[Test]
		public void Parse_RowPointsInstruction_Player1Points17()
		{
			var act = Instruction.Parse("update player1 row_points 17");
			var exp = new RowPointsInstruction(PlayerName.Player1, 17);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Parse_ComboInstruction_Player2Points3()
		{
			var act = Instruction.Parse("update player2 combo 3");
			var exp = new ComboInstruction(PlayerName.Player2, 3);

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Parse_FieldInstruction_Player2Array()
		{
			var act = Instruction.Parse("update player2 field 0,1,2,2;0,0,0,2;0,0,2,2");
			var exp = new FieldInstruction(PlayerName.Player2, new int[,]
			{
				{0, 1, 2, 2},
				{0, 0, 0, 2},
				{0, 0, 2, 2},
			});

			Assert.AreEqual(exp.ToString(), act.ToString());
		}

		#endregion
	}
}
