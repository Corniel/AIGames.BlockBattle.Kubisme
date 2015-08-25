using AIGames.BlockBattle.Kubisme.Communication;
using NUnit.Framework;
using System;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockTest
	{
		[Test]
		public void BlockRotates_O_AllTheSame()
		{
			ValidateDefinition("XX..|XX..", 1, 9, Block.O[Block.RotationType.None]);
		}

		[Test]
		public void BlockRotates_I_IsValidDefinition()
		{
			ValidateDefinition("XXXX", 2, 17, Block.I[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_ILeft_IsValidDefinition()
		{
			ValidateDefinition("X...|X...|X...|X...", 2, 17, Block.I[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_S_IsValidDefinition()
		{
			ValidateDefinition(".XX.|XX..", 2, 17, Block.S[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_SLeft_IsValidDefinition()
		{
			ValidateDefinition("X...|XX..|.X..", 2, 17, Block.S[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_Z_IsValidDefinition()
		{
			ValidateDefinition("XX..|.XX.", 2, 17, Block.Z[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_ZLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|XX..|X...", 2, 17, Block.Z[Block.RotationType.Left]);
		}

		[Test]
		public void BlockRotates_J_IsValidDefinition()
		{
			ValidateDefinition("X...|XXX.", 4, 34, Block.J[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_JLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|.X..|XX..", 4, 34, Block.J[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_J180_IsValidDefinition()
		{
			ValidateDefinition("XXX.|..X.", 4, 34, Block.J[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_JRight_IsValidDefinition()
		{
			ValidateDefinition("XX..|X...|X...", 4, 34, Block.J[Block.RotationType.Right]);
		}

		[Test]
		public void BlockRotates_L_IsValidDefinition()
		{
			ValidateDefinition("..X.|XXX.", 4, 34, Block.L[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_LLeft_IsValidDefinition()
		{
			ValidateDefinition("XX..|.X..|.X..", 4, 34, Block.L[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_L180_IsValidDefinition()
		{
			ValidateDefinition("XXX.|X...", 4, 34, Block.L[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_LRight_IsValidDefinition()
		{
			ValidateDefinition("X...|X...|XX..", 4, 34, Block.L[Block.RotationType.Right]);
		}

		[Test]
		public void BlockRotates_T_IsValidDefinition()
		{
			ValidateDefinition(".X..|XXX.", 4, 34, Block.T[Block.RotationType.None]);
		}
		[Test]
		public void BlockRotates_TLeft_IsValidDefinition()
		{
			ValidateDefinition(".X..|XX..|.X..", 4, 34, Block.T[Block.RotationType.Left]);
		}
		[Test]
		public void BlockRotates_T180_IsValidDefinition()
		{
			ValidateDefinition("XXX.|.X..", 4, 34, Block.T[Block.RotationType.Uturn]);
		}
		[Test]
		public void BlockRotates_TRight_IsValidDefinition()
		{
			ValidateDefinition("X...|XX..|X...", 4, 34, Block.T[Block.RotationType.Right]);
		}

		[Test]
		public void ThisLineCol_AllPossibleColumns_Matches()
		{
			var block = Block.I[Block.RotationType.Left];
			var act = new int[16];

			for (var i = 0; i < 16; i++)
			{
				act[i] = block[0, i];
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
		public void GetMaxRow_TOnFieldOf3_1()
		{
			var field = Field.Create(0, 0, @"
..........
..........
..........");

			var act = Block.T.GetMaxRow(field);
			var exp = 1;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Select_All_Matches()
		{
			var tps = new PieceType[] { PieceType.I, PieceType.J, PieceType.L, PieceType.O, PieceType.S, PieceType.T, PieceType.Z };
			var act = tps.Select(tp => Block.Select(tp)).Select(block => block.Name).ToArray();
			var exp = new string[] { "I", "J", "L", "O", "S", "T", "Z" };
			CollectionAssert.AreEqual(exp, act);
		}
		[Test]
		public void Select_None_IsNull()
		{
			Assert.IsNull(Block.Select(PieceType.None));
		}

		[Test]
		public void accessibles_None_RightBitMask()
		{
			var act = Block.accessibles.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXX....",
				".XXXXX....",
				"..XXXX....",
				"...XXX....",
				
				"...XXXX...",
				"...XXXXX..",
				"...XXXXXX.",
				"...XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessiblesSZLeft_None_RightBitMask()
		{
			var act = Block.accessiblesSZLeft.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXX....",
				".XXXXX....",
				"..XXXX....",
				
				"...XXX....",
				"...XXX....",
				
				"...XXXX...",
				"...XXXXX..",
				"...XXXXXX.",
				"...XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessiblesJLTLeft_None_RightBitMask()
		{
			var act = Block.accessiblesJLTLeft.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXX....",
				".XXXXX....",
				"..XXXX....",
				
				"...XXX....",
				
				"...XXXX...",
				"...XXXXX..",
				"...XXXXXX.",
				"...XXXXXXX",

				"...XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void accessiblesJLTRight_None_RightBitMask()
		{
			var act = Block.accessiblesJLTRight.Select(e => Row.ToString(e)).ToArray();
			var exp = new string[]
			{
				"XXXXXX....",

				"XXXXXX....",
				".XXXXX....",
				"..XXXX....",
				
				"...XXX....",
				
				"...XXXX...",
				"...XXXXX..",
				"...XXXXXX.",
				"...XXXXXXX",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		protected void ValidateDefinition(string expStr, int expVariations, int expChildCount, Block org)
		{
			var tp = org.GetType().Name;
			var name = tp.Substring(5, 1);
			var rot = tp.Substring(6);
			Block.RotationType rotation = Block.RotationType.None;

			if (!String.IsNullOrEmpty(rot) && !Enum.TryParse<Block.RotationType>(rot, out rotation))
			{
				Assert.Fail("Type '{0}', name does not fit into contract.", tp);
			}

			Assert.AreEqual(name, org.Name, "Name");
			Assert.AreEqual(rotation, org.Rotation, "Rotation");

			Assert.AreEqual(expStr, org.ToString(), "ToString().");
			Assert.AreEqual(4, org.Count, "Count should be 4.");
			Assert.AreEqual(expVariations, org.Variations.Length, "Variations.Length");
			Assert.AreEqual(expChildCount, org.ChildCount, "ChildCount");
		}
	}
}
