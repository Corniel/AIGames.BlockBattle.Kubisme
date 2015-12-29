using AIGames.BlockBattle.Kubisme.Communication;
using AIGames.BlockBattle.Kubisme.UnitTests.DecisionMaking;
using NUnit.Framework;

namespace AIGames.BlockBattle.Kubisme.UnitTests.Blocks
{
	[TestFixture]
	public class BlockTTest
	{
		[Test]
		public void TSpinTopMask_None_MasksMatch()
		{
			var act = BitsTest.Select(BlockT.TSpinTopMask);
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
		public void TSpinRow1Mask_All_Matches()
		{
			var act = BitsTest.Select(BlockT.TSpinRow1Mask);
			var exp = new string[] 
			{ 
				"XXX.......",
				".XXX......",
				"..XXX.....",
				"...XXX....",
				"....XXX...",
				".....XXX..",
				"......XXX.",
				".......XXX",
				
			};
			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void TSpinRow2Mask_All_Matches()
		{
			var act = BitsTest.Select(BlockT.TSpinRow2Mask);
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
			var field = Field.Create(0, 0, 0, @"
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
			var field = Field.Create(0, 0, 0, @"
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
			var field = Field.Create(0, 0, 0, @"
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
			var field = Field.Create(0, 0, 0, @"
				..X..XX...
				XX....XXXX
				XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			FieldAssert.AreEqual( @"
				..........
				..X..XX...
				XX.XXXXXXX", 5, 1, 0 , act);
		}

		[Test]
		public void Apply_Row1_WithDoubleTSpin()
		{
			var field = Field.Create(0, 0, 0, @"
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
			var field = Field.Create(0, 0, 0, @"
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
			var field = Field.Create(0, 0, 0, @"
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
		public void Apply_SingleTSpinLeft_AddsPointsAndSkips()
		{
			var field = Field.Create(0, 0, 17, @"
				..X..XX...
				XXX...XXXX
				XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Left], new Position(3, 0));

			var exp = @"
				..........
				..X.XXX...
				XXXXX.XXXX";
			FieldAssert.AreEqual(exp, 5, 1, 17, act);
		}

		[Test]
		public void Apply_SingleTSpinRight_AddsPointsAndSkips()
		{
			var field = Field.Create(0, 0, 17, @"
				.....X....
				XXX...XXXX
				XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Right], new Position(4, 0));

			var exp = @"
				..........
				....XX....
				XXX.XXXXXX";
			FieldAssert.AreEqual(exp, 5, 1, 17, act);
		}
		[Test]
		public void Apply_SingleTSpinNone_AddsPointsAndSkips()
		{
			var field = Field.Create(0, 0, 17, @"
				.....X....
				XXX...XXXX
				XXXX.XXXXX");

			var act = field.Apply(Block.T, new Position(3, 0));

			var exp = @"
				..........
				....XX....
				XXXX.XXXXX";
			FieldAssert.AreEqual(exp, 5, 1, 17, act);
		}

		[Test]
		public void Apply_DoubleTSpin_AddsPointsAndSkips()
		{
			var field = Field.Create(0, 0, 17, @"
..X..XX...
XXX...XXXX
XXXX.XXXXX");

			var act = field.Apply(Block.T[Block.RotationType.Uturn], new Position(3, 1));

			var exp = "..........|..........|..X..XX...";
			FieldAssert.AreEqual(exp, 10, 1, 18, act);
		}

		[Test, Category(Category.Evaluation)]
		public void GetMove_TForTSpin_ClearedField()
		{
			var field = Field.Create(0, 2, 0, @"
..........
..........
..X..XX...
XXX...XXXX
XXXX.XXXXX");

			var pars = new  EvaluatorParameters()
			{
				Points = 100,
			}.Calc();

			var dm = new NodeDecisionMakerTester()
			{
				Evaluator = new Evaluator(),
				Generator = new MoveGenerator(),
				MaximumDepth = 3,
			};

			var act = dm.GetMove(field, Block.T, Block.Z, 1, pars);
			var actField = dm.BestField.ToString();

			var exp = BlockPath.Create(ActionType.Down, ActionType.TurnLeft, ActionType.Down, ActionType.Down, ActionType.TurnLeft);
			var expField = "..........|..........|..........|..........|..X..XX...";

			Assert.AreEqual(expField, actField);
			Assert.AreEqual(exp, act);
		}
	}
}
