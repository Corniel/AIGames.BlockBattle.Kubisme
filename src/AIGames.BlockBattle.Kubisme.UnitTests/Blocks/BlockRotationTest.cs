using NUnit.Framework;
using System;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockRotationTest
	{
		[Test]
		public void TurnLeft_O_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........");
			var exp = new string[]
			{
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
			};
			TurnLeft(exp, Block.O, position, field);
		}
		[Test]
		public void TurnRight_O_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........");
			var exp = new string[]
			{
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
				"XX........|XX........",
			};
			TurnRight(exp, Block.O, position, field);
		}


		[Test]
		public void TurnLeft_I_AreEqual()
		{
			var position = new Position(0, 1);
			var field = Field.Create(0, 0, 0, "...........|...........|...........|...........");
			var exp = new string[]
			{
				"..........|XXXX......|..........|..........",
				".X........|.X........|.X........|.X........",
				"..........|..........|XXXX......|..........",
				"..X.......|..X.......|..X.......|..X.......",
				"..........|XXXX......|..........|..........",
			};
			TurnLeft(exp, Block.I, position, field);
		}
		[Test]
		public void TurnRight_I_AreEqual()
		{
			var position = new Position(0, 1);
			var field = Field.Create(0, 0, 0, "...........|...........|...........|...........");
			var exp = new string[]
			{
				"..........|XXXX......|..........|..........",
				"..X.......|..X.......|..X.......|..X.......",
				"..........|..........|XXXX......|..........",
				".X........|.X........|.X........|.X........",
				"..........|XXXX......|..........|..........",
			};
			TurnRight(exp, Block.I, position, field);
		}


		[Test]
		public void TurnLeft_S_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				".XX.......|XX........|..........",
				"X.........|XX........|.X........",
				"..........|.XX.......|XX........",
				".X........|.XX.......|..X.......",
				".XX.......|XX........|..........",
			};
			TurnLeft(exp, Block.S, position, field);
		}
		[Test]
		public void TurnRight_S_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				".XX.......|XX........|..........",
				".X........|.XX.......|..X.......",
				"..........|.XX.......|XX........",
				"X.........|XX........|.X........",
				".XX.......|XX........|..........",
			};
			TurnRight(exp, Block.S, position, field);
		}

		[Test]
		public void TurnLeft_Z_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
			"XX........|.XX.......|..........",
			".X........|XX........|X.........",
			"..........|XX........|.XX.......",
			"..X.......|.XX.......|.X........",
			"XX........|.XX.......|..........",
			};
			TurnLeft(exp, Block.Z, position, field);
		}
		[Test]
		public void TurnRight_Z_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
			"XX........|.XX.......|..........",
			"..X.......|.XX.......|.X........",
			"..........|XX........|.XX.......",
			".X........|XX........|X.........",
			"XX........|.XX.......|..........",

			};
			TurnRight(exp, Block.Z, position, field);
		}


		[Test]
		public void TurnLeft_J_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				"X.........|XXX.......|..........",
				".X........|.X........|XX........",
				"..........|XXX.......|..X.......",
				".XX.......|.X........|.X........",
				"X.........|XXX.......|..........",
			};
			TurnLeft(exp, Block.J, position, field);
		}
		[Test]
		public void TurnRight_J_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				"X.........|XXX.......|..........",
				".XX.......|.X........|.X........",
				"..........|XXX.......|..X.......",
				".X........|.X........|XX........",
				"X.........|XXX.......|..........",
			};
			TurnRight(exp, Block.J, position, field);
		}

		[Test]
		public void TurnLeft_L_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				"..X.......|XXX.......|..........",
				"XX........|.X........|.X........",
				"..........|XXX.......|X.........",
				".X........|.X........|.XX.......",
				"..X.......|XXX.......|..........",
			};
			TurnLeft(exp, Block.L, position, field);
		}
		[Test]
		public void TurnRight_L_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				"..X.......|XXX.......|..........",
				".X........|.X........|.XX.......",
				"..........|XXX.......|X.........",
				"XX........|.X........|.X........",
				"..X.......|XXX.......|..........",
			};
			TurnRight(exp, Block.L, position, field);
		}

		[Test]
		public void TurnLeft_T_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				".X........|XXX.......|..........",
				".X........|XX........|.X........",
				"..........|XXX.......|.X........",
				".X........|.XX.......|.X........",
				".X........|XXX.......|..........",
			};
			TurnLeft(exp, Block.T, position, field);
		}
		[Test]
		public void TurnRight_T_AreEqual()
		{
			var position = new Position(0, 0);
			var field = Field.Create(0, 0, 0, "...........|...........|...........");
			var exp = new string[]
			{
				".X........|XXX.......|..........",
				".X........|.XX.......|.X........",
				"..........|XXX.......|.X........",
				".X........|XX........|.X........",
				".X........|XXX.......|..........",
			};
			TurnRight(exp, Block.T, position, field);
		}

		private static void TurnLeft(string[] exp, Block block, Position position, Field field)
		{
			var act = new string[5];
			act[0] = field.Apply(block, position).ToString();

			for (var i = 1; i < 5; i++)
			{
				position = block.TurnLeft(position);
				block = block.TurnLeft();
				var apply = field.Apply(block, position).ToString();
				act[i] = apply;

				Console.WriteLine(apply.Replace("|", Environment.NewLine));
				Console.WriteLine();
			}

			foreach (var f in act)
			{
				Console.WriteLine('"' + f + '"' + ',');
			}
			CollectionAssert.AreEqual(exp, act);
		}
		private static void TurnRight(string[] exp, Block block, Position position, Field field)
		{
			var act = new string[5];
			act[0] = field.Apply(block, position).ToString();

			for (var i = 1; i < 5; i++)
			{
				position = block.TurnRight(position);
				block = block.TurnRight();
				var apply = field.Apply(block, position).ToString();
				act[i] = apply;

				Console.WriteLine(apply.Replace("|", Environment.NewLine));
				Console.WriteLine();
			}

			foreach (var f in act)
			{
				Console.WriteLine('"' + f + '"' + ',');
			}
			CollectionAssert.AreEqual(exp, act);
		}
	}
}
