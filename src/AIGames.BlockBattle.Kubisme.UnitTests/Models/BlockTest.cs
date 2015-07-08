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
			BlockRotates("XX..|XX..|....|....", Block.O.Variations[0]);
		}

		[Test]
		public void BlockRotates_I_Rotates()
		{
			BlockRotates("....|XXXX|....|....", Block.I.Variations[0]);
		}
		[Test]
		public void BlockRotates_Ir_Rotates()
		{
			BlockRotates(".X..|.X..|.X..|.X..", Block.I.Variations[1]);
		}

		[Test]
		public void BlockRotates_S_Rotates()
		{
			BlockRotates(".XX.|XX..|....|....", Block.S.Variations[0]);
		}
		[Test]
		public void BlockRotates_Sr_Rotates()
		{
			BlockRotates("X...|XX..|.X..|....", Block.S.Variations[1]);
		}

		[Test]
		public void BlockRotates_Z_Rotates()
		{
			BlockRotates("XX..|.XX.|....|....", Block.Z.Variations[0]);
		}
		[Test]
		public void BlockRotates_Zr_Rotates()
		{
			BlockRotates(".X..|XX..|X...|....", Block.Z.Variations[1]);
		}

		[Test]
		public void BlockRotates_J_Rotates()
		{
			BlockRotates("X...|XXX.|....|....", Block.J.Variations[0]);
		}
		[Test]
		public void BlockRotates_Jr_Rotates()
		{
			BlockRotates(".X..|.X..|XX..|....", Block.J.Variations[1]);
		}
		[Test]
		public void BlockRotates_Ju_Rotates()
		{
			BlockRotates("....|XXX.|..X.|....", Block.J.Variations[2]);
		}
		[Test]
		public void BlockRotates_Jl_Rotates()
		{
			BlockRotates(".XX.|.X..|.X..|....", Block.J.Variations[3]);
		}

		[Test]
		public void BlockRotates_L_Rotates()
		{
			BlockRotates("..X.|XXX.|....|....", Block.L.Variations[0]);
		}
		[Test]
		public void BlockRotates_Lr_Rotates()
		{
			BlockRotates(".X..|.X..|.XX.|....", Block.L.Variations[1]);
		}
		[Test]
		public void BlockRotates_Lu_Rotates()
		{
			BlockRotates("....|XXX.|X...|....", Block.L.Variations[2]);
		}
		[Test]
		public void BlockRotates_Ll_Rotates()
		{
			BlockRotates("XX..|.X..|.X..|....", Block.L.Variations[3]);
		}

		[Test]
		public void BlockRotates_T_Rotates()
		{
			BlockRotates(".X..|XXX.|....|....", Block.T.Variations[0]);
		}
		[Test]
		public void BlockRotates_Tr_Rotates()
		{
			BlockRotates(".X..|XX..|.X..|....", Block.T.Variations[1]);
		}
		[Test]
		public void BlockRotates_Tu_Rotates()
		{
			BlockRotates("....|XXX.|.X..|....", Block.T.Variations[2]);
		}
		[Test]
		public void BlockRotates_Tl_Rotates()
		{
			BlockRotates(".X..|.XX.|.X..|....", Block.T.Variations[3]);
		}

		protected void BlockRotates(string expected, Block org)
		{
			Assert.AreEqual(expected, org.ToString(), "ToString().");
			Assert.AreEqual(4, org.Count, "Count should be 4.");
		}
	}
}
