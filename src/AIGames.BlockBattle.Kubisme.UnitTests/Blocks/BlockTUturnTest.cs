using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking;
using AIGames.BlockBattle.Kubisme.UnitTests.Evaluation;
using NUnit.Framework;
using Troschuetz.Random.Generators;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockTUturnTest
	{
		[Test]
		public void TSpinTopMask_None_MasksMatch()
		{
			var act = BitsTest.Select(BlockTUturn.TSpinTopMask);
			var exp = new string[]
			{
				"X.X.......",
				".X.X......",
				"..X.X.....",
				"...X.X....",

				"....X.X...",
				".....X.X..",
				"......X.X.",
				".......X.X",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TSpinTopBorderMask_None_MasksMatch()
		{
			var act = BitsTest.Select(BlockTUturn.TSpinTopBorderMask);
			var exp = new string[]
			{
				"...X......",
				"X...X.....",
				".X...X....",
				"..X...X...",

				"...X...X..",
				"....X...X.",
				".....X...X",
				"......X...",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TSpinRow1Mask_All_Matches()
		{
			var act = BitsTest.Select(BlockTUturn.TSpinRow1Mask);
			var exp = new string[] 
			{ 
				"...XXXXXXX",
				"X...XXXXXX",
				"XX...XXXXX",
				"XXX...XXXX",
				"XXXX...XXX",
				"XXXXX...XX",
				"XXXXXX...X",
				"XXXXXXX...",
				
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TSpinRow2Mask_All_Matches()
		{
			var act = BitsTest.Select(BlockTUturn.TSpinRow2Mask);
			var exp = new string[] 
			{ 
				"X.XXXXXXXX",
				"XX.XXXXXXX",
				"XXX.XXXXXX",
				"XXXX.XXXXX",
				"XXXXX.XXXX",
				"XXXXXX.XXX",
				"XXXXXXX.XX",
				"XXXXXXXX.X",
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Apply_Top_NoTSpin()
		{
			var field = Field.Create(0, 0, @"
XXX...XXXX
.XXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 0));

			var expField = "..........|.XXXXXXXXX";
			var expPt = 0;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1_NoTSpin()
		{
			var field = Field.Create(0, 0, @"
..X...X...
XXX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..........|..X...X...";
			var expPt = 3;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1ToMuchSpaceOnTail_NoTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XXX...XXXX
XXX..XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|XXX.XXXXXX";
			var expPt = 0;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1ToMuchSpaceToMuchSpaceOnHead_NoTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XX....XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|XX.XXXXXXX";
			var expPt = 0;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XXX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..........|..X..XX...";
			var expPt = 10;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1Clear1_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
.XX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|.XXXXXXXXX";
			var expPt = 5;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void Apply_Row1Clear2_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, @"
..X..XX...
XXX...XXXX
.XXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var expField = "..........|..X..XX...|.XXXXXXXXX";
			var expPt = 5;

			Assert.AreEqual(expField, act.ToString());
			Assert.AreEqual(expPt, act.Points);
		}

		[Test]
		public void GetMove_TForTSpin_ClearedField()
		{
			var field = Field.Create(0, 2, @"
..........
..........
..X..XX...
XXX...XXXX
XXXX.XXXXX");

			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new ComplexEvaluator()
				{
					Parameters = new  ComplexParameters()
					{
						Points = 100,
						Holes = -100,
					}.Calculate(),
					Opponent = new OpponentStub(),
				},
				Generator = new MoveGenerator(),
				MaximumDepth = 3,
			};

			var act = dm.GetMove(field, Block.T, Block.Z, 1);
			var actField = dm.BestField.ToString();

			var exp = BlockPath.Create(ActionType.Down, ActionType.TurnLeft, ActionType.Down, ActionType.Down, ActionType.TurnLeft);
			var expField = "..........|..........|..........|..........|..X..XX...";

			Assert.AreEqual(expField, actField);
			Assert.AreEqual(exp, act);
		}
	}
}
