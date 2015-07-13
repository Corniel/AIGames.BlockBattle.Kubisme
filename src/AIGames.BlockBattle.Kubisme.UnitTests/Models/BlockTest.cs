using AIGames.BlockBattle.Kubisme.Models;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Models
{
	[TestFixture]
	public class BlockTest
	{
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

		protected void ValidateDefinition(string expected, Block org)
		{
			Assert.AreEqual(expected, org.ToString(), "ToString().");
			Assert.AreEqual(4, org.Count, "Count should be 4.");
		}
	}
}
