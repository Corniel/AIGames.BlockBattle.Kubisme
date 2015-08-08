using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class BlockTest
	{
		[Test]
		public void Name_O_O()
		{
			var act = Block.O.Name;
			var exp = "O";
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void BlockRotates_O_AllTheSame()
		{
			ValidateDefinition("XX..|XX..|....|....", Block.O[Block.RotationType.None]);
		}

		[Test]
		public void BlockRotates_I_IsValidDefinition()
		{
			ValidateDefinition("....|XXXX|....|....", Block.I[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_ILeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|.X..|.X..|.X..", Block.I[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_S_IsValidDefinition()
		{
			ValidateDefinition(".XX.|XX..|....|....", Block.S[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_SLeft_IsValidDefinition()
		{
			ValidateDefinition("X...|XX..|.X..|....", Block.S[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_Z_IsValidDefinition()
		{
			ValidateDefinition("XX..|.XX.|....|....", Block.Z[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_ZLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|XX..|X...|....", Block.Z[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_J_IsValidDefinition()
		{
			ValidateDefinition("X...|XXX.|....|....", Block.J[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_JLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|.X..|XX..|....", Block.J[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_J180_IsValidDefinition()
		{
			ValidateDefinition("....|XXX.|..X.|....", Block.J[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_JRight_IsValidDefinition()
		{
			ValidateDefinition(".XX.|.X..|.X..|....", Block.J[Block.RotationType.Right]);
		}

		[Test]
		public void BlockRotates_L_IsValidDefinition()
		{
			ValidateDefinition("..X.|XXX.|....|....", Block.L[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_LLeft_IsValidDefinition()
		{
			ValidateDefinition("XX..|.X..|.X..|....", Block.L[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_L180_IsValidDefinition()
		{
			ValidateDefinition("....|XXX.|X...|....", Block.L[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_LRight_IsValidDefinition()
		{
			ValidateDefinition(".X..|.X..|.XX.|....", Block.L[Block.RotationType.Right]);
		}

		[Test]
		public void BlockRotates_T_IsValidDefinition()
		{
			ValidateDefinition(".X..|XXX.|....|....", Block.T[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_TLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|XX..|.X..|....", Block.T[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_T180_IsValidDefinition()
		{
			ValidateDefinition("....|XXX.|.X..|....", Block.T[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_TRight_IsValidDefinition()
		{
			ValidateDefinition(".X..|.XX.|.X..|....", Block.T[Block.RotationType.Right]);
		}

		[Test]
		public void ThisIndex_Index0To5_Matches()
		{
			var block =Block.I[Block.RotationType.Left];
			var act = new byte[5];
			for (var i = 0; i < 5; i++)
			{
				act[i] = block[i];
			}
			var exp = new byte[]{2, 2, 2, 2, 0};

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void ThisLineCol_AllPossibleColumns_Matches()
		{
			var block = Block.I[Block.RotationType.Left];
			var act = new int[16];

			for (var i = 0; i < 16; i++)
			{
				act[i] = block[0, i - 1];
			}
			var exp = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 0, 0, 0, 0, 0, 0 };
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetMinRow_TOnFieldOf3_1()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........");

			var act = Block.T.GetMinRow(field);
			var exp = 1;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void GetMinRow_TOnFilledFieldOf3_0()
		{
			var field = Field.Create(0, 0, @"
.........X
........XX
.......XX.");

			var act = Block.T.GetMinRow(field);
			var exp = 0;
			Assert.AreEqual(exp, act);
		}
		[Test]
		public void GetMaxRow_TOnFieldOf3_2()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........");

			var act = Block.T.GetMaxRow(field);
			var exp = 2;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Select_All_Matches()
		{
			var tps = new PieceType[] { PieceType.I, PieceType.J, PieceType.L, PieceType.O, PieceType.S, PieceType.T, PieceType.Z };
			var act = tps.Select(tp => Block.Select(tp)).Select(block => block.Name).ToArray();
			var exp = new string[]{ "I", "J", "L", "O", "S", "T", "Z" };
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void Select_None_IsNull()
		{
			Assert.IsNull(Block.Select(PieceType.None));
		}

		protected void ValidateDefinition(string expected, Block org)
		{
			Assert.AreEqual(expected, org.ToString(), "ToString().");
			Assert.AreEqual(4, org.Count, "Count should be 4.");
		}
	}
}
