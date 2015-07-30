using AIGames.BlockBattle.Kubisme.Communication;
using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class MoveGenerator : IMoveGenerator
	{
		public IEnumerable<Field> GetFields(Field field, Block current, Position pos, bool searchNoDrops)
		{
			foreach (var block in current.Variations)
			{
				int minRow = block.GetMinRow(field);
				int maxRow = block.GetMaxRow(field);

				foreach (var col in block.Columns)
				{
					for (var row = minRow; row < maxRow; row++)
					{
						var test = field.Test(block, col, row);
						if (test == Field.TestResult.True)
						{
							var target = new Position(col, row);
							var applied = field.Apply(block, target);
							yield return applied;
						}
						if (test != Field.TestResult.Retry)
						{
							row = short.MaxValue;
						}
					}
				}
				if (searchNoDrops)
				{
				}
			}
		}

		public IEnumerable<MoveCandiate> GetMoves(Field field, Block current, Position pos, bool searchNoDrops)
		{
			foreach (var block in current.Variations)
			{
				int minRow = block.GetMinRow(field);
				int maxRow = block.GetMaxRow(field);

				foreach(var col in block.Columns)
				{
					for (var row = minRow; row < maxRow; row++)
					{
						var test = field.Test(block, col, row);
						if (test == Field.TestResult.True)
						{
							var target = new Position(col, row);
							var applied = field.Apply(block, target);
							yield return new MoveCandiate(block.InitialPath.AddShift(col - pos.Col).AddDrop(), applied);
						}
						if (test != Field.TestResult.Retry)
						{
							row = short.MaxValue;
						}
					}
				}
				if (searchNoDrops)
				{
				}
			}
		}

		/// <summary>Gets a score only based on characters of the current state.</summary>
		public static bool HasReachableHoles(Field field)
		{
			int filterTopColomns = 0;
			int open = Row.Filled;
			int prev = Row.Filled;

			// loop through the rows.
			for (var r = field.FirstFilled; r < field.RowCount; r++)
			{
				var row = field[r].row;
				var rowMirrored = Row.Filled ^ row;

				open &= rowMirrored;
				var openCount =Row.Count[open];
				// If only one hole, no access.
				if (openCount < 2)
				{
					return false;
				}
				// If there are no free spots combined on a role there is no access.
				if (openCount > 6 && !Row.Row8BlockOneHole.Any(map => (map & open) != 0))
				{
					return false;
				}

				var movement = prev & rowMirrored;
				var movementCount = Row.Count[movement];
				if (movementCount < 2)
				{
					return false;
				}
				// there should be a way for a block to get from one row to another.
				if (movementCount > 6 && !Row.Row8BlockOneHole.Any(map => (map & movement) != 0))
				{
					return false;
				}

				// is there a hole anyway?
				var holesMask = filterTopColomns & rowMirrored;
				if (holesMask != 0)
				{
					return true;
				}

				filterTopColomns |= row;

				prev = rowMirrored;
			}
			return false;

		}
	}
}
