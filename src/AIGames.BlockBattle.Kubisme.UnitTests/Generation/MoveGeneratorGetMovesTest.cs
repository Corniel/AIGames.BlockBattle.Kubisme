using NUnit.Framework;
using System;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Generation
{
	[TestFixture]
	public class MoveGeneratorGetMovesTest
	{
		[Test]
		public void GetMoves_O_9candidates()
		{
			var exp = new string[]
			{ 
				"left,left,left,left,drop",
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",
							};
			AssertGetMoves(exp, Block.O);
		}

		[Test]
		public void GetMoves_I_17candidates()
		{
			var exp = new string[]
			{ 
				"left,left,left,drop",				"left,left,drop",				"left,drop",				"drop",				"right,drop",				"right,right,drop",				"right,right,right,drop",				"left,left,left,turnleft,left,drop",				"left,left,left,turnleft,drop",				"left,left,turnleft,drop",				"left,turnleft,drop",				"turnleft,drop",				"turnright,drop",				"right,turnright,drop",				"right,right,turnright,drop",				"right,right,right,turnright,drop",				"right,right,right,turnright,right,drop",
			};
			AssertGetMoves(exp, Block.I);
		}

		[Test]
		public void GetMoves_S_17candidates()
		{
			var exp = new string[]
			{ 
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",
				
				"left,left,left,turnleft,drop",
				"left,left,turnleft,drop",
				"left,turnleft,drop",
				"turnleft,drop",
				"turnright,drop",
				"right,turnright,drop",
				"right,right,turnright,drop",
				"right,right,right,turnright,drop",
				"right,right,right,right,turnright,drop",
			};
			AssertGetMoves(exp, Block.S);
		}
		[Test]
		public void GetMoves_Z_17candidates()
		{
			var exp = new string[]
			{
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",
				
				"left,left,left,turnleft,drop",
				"left,left,turnleft,drop",
				"left,turnleft,drop",
				"turnleft,drop",
				"turnright,drop",
				"right,turnright,drop",
				"right,right,turnright,drop",
				"right,right,right,turnright,drop",
				"right,right,right,right,turnright,drop",
			};
			AssertGetMoves(exp, Block.Z);
		}

		[Test]
		public void GetMoves_J_34candidates()
		{
			var exp = new string[]
			{
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",

				"left,left,left,turnleft,drop",
				"left,left,turnleft,drop",
				"left,turnleft,drop",
				"turnleft,drop",
				"right,turnleft,drop",
				"right,right,turnleft,drop",
				"right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,right,drop",

				"turnleft,turnleft,left,left,left,drop",
				"turnleft,turnleft,left,left,drop",
				"turnleft,turnleft,left,drop",
				"turnleft,turnleft,drop",
				"turnleft,turnleft,right,drop",
				"turnleft,turnleft,right,right,drop",
				"turnleft,turnleft,right,right,right,drop",
				"turnleft,turnleft,right,right,right,right,drop",
				
				"left,left,left,turnright,left,drop",
				"left,left,left,turnright,drop",
				"left,left,turnright,drop",
				"left,turnright,drop",
				"turnright,drop",
				"right,turnright,drop",
				"right,right,turnright,drop",
				"right,right,right,turnright,drop",
				"right,right,right,right,turnright,drop",
			};
			AssertGetMoves(exp, Block.J);
		}
		[Test]
		public void GetMoves_L_34candidates()
		{
			var exp = new string[]
			{
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",

				"left,left,left,turnleft,drop",
				"left,left,turnleft,drop",
				"left,turnleft,drop",
				"turnleft,drop",
				"right,turnleft,drop",
				"right,right,turnleft,drop",
				"right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,right,drop",

				"turnleft,turnleft,left,left,left,drop",
				"turnleft,turnleft,left,left,drop",
				"turnleft,turnleft,left,drop",
				"turnleft,turnleft,drop",
				"turnleft,turnleft,right,drop",
				"turnleft,turnleft,right,right,drop",
				"turnleft,turnleft,right,right,right,drop",
				"turnleft,turnleft,right,right,right,right,drop",

				"left,left,left,turnright,left,drop",
				"left,left,left,turnright,drop",
				"left,left,turnright,drop",
				"left,turnright,drop",
				"turnright,drop",
				"right,turnright,drop",
				"right,right,turnright,drop",
				"right,right,right,turnright,drop",
				"right,right,right,right,turnright,drop",

			};
			AssertGetMoves(exp, Block.L);
		}

		[Test]
		public void GetMoves_T_34candidates()
		{
			var exp = new string[]
			{
				"left,left,left,drop",
				"left,left,drop",
				"left,drop",
				"drop",
				"right,drop",
				"right,right,drop",
				"right,right,right,drop",
				"right,right,right,right,drop",

				"left,left,left,turnleft,drop",
				"left,left,turnleft,drop",
				"left,turnleft,drop",
				"turnleft,drop",
				"right,turnleft,drop",
				"right,right,turnleft,drop",
				"right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,drop",
				"right,right,right,right,turnleft,right,drop",

				"turnleft,turnleft,left,left,left,drop",
				"turnleft,turnleft,left,left,drop",
				"turnleft,turnleft,left,drop",
				"turnleft,turnleft,drop",
				"turnleft,turnleft,right,drop",
				"turnleft,turnleft,right,right,drop",
				"turnleft,turnleft,right,right,right,drop",
				"turnleft,turnleft,right,right,right,right,drop",

				"left,left,left,turnright,left,drop",
				"left,left,left,turnright,drop",
				"left,left,turnright,drop",
				"left,turnright,drop",
				"turnright,drop",
				"right,turnright,drop",
				"right,right,turnright,drop",
				"right,right,right,turnright,drop",
				"right,right,right,right,turnright,drop",
			};
			AssertGetMoves(exp, Block.T);
		}

		private static void AssertGetMoves(string[] exp, Block block)
		{
			var generator = new MoveGenerator();
			var moves = generator.GetMoves(TestData.Small, block, false).Select(candidate => candidate.Path.ToString()).ToArray();

			foreach (var move in moves)
			{
				Console.WriteLine('"' + move + '"' + ',');
			}

			Assert.AreEqual(block.ChildCount, moves.Length, "GetMoves for {0} has the wrong number of answers.", block.Name);
			CollectionAssert.AreEqual(exp, moves);
		}
	}
}
