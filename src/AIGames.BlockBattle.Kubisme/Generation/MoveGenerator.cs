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
				var minRow = field.FirstFilled - 4 + block.Bottom;
				var maxRow = field.RowCount - 3 + block.Bottom;

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
			}
			if (searchNoDrops)
			{
			}
		}


		public IEnumerable<MoveCandiate> GetMoves(Field field, Block current, Position pos, bool searchNoDrops)
		{
			foreach (var block in current.Variations)
			{
				var minRow = field.FirstFilled - 4 + block.Bottom;
				var maxRow = field.RowCount - 3 + block.Bottom;

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
				/// <summary>Handles get moves recursive.</summary>
		private IEnumerable<MoveCandiate> GetMoves(Field field, Block block, BlockPath path, Position pos, ActionType last, bool hasHoles)
		{
			var minCol = 0 - block.Left;
			var maxCol = 7 + block.Right;

			if (pos.Col >= minCol && pos.Col < maxCol)
			{
				var result = field.Test(block, pos);

				// we have result, so return that.
				if (result == Field.TestResult.True)
				{
					yield return new MoveCandiate(path, field.Apply(block, pos));
				}
				// we go to apply a drop.
				else if (result == Field.TestResult.Retry)
				{
					foreach (var candidate in GetMoves(field, block, path.AddDown(), pos.Drop, ActionType.Down, hasHoles))
					{
						yield return candidate;
					}
				}

				// if not false, check for left and right.
				if (result != Field.TestResult.False)
				{
					var isInitialRow = pos.Row == -1;

					var posLeft = pos.Left;
					var posRight = pos.Right;

					var applyLeft = (isInitialRow && (last == ActionType.None || last == ActionType.Left)) || (hasHoles && last != ActionType.Right && TestLeftRight(field, block, posLeft));
					var applyRight = (isInitialRow && (last == ActionType.None || last == ActionType.Right)) || (hasHoles && last != ActionType.Left && TestLeftRight(field, block, posRight));

					if (applyLeft)
					{
						foreach (var candidate in GetMoves(field, block, path.AddLeft(), posLeft, ActionType.Left, hasHoles))
						{
							yield return candidate;
						}
					}
					if (applyRight)
					{
						foreach (var candidate in GetMoves(field, block, path.AddRight(), posRight, ActionType.Right, hasHoles))
						{
							yield return candidate;
						}
					}
				}
			}
		}

		private bool TestLeftRight(Field field, Block block, Position pos)
		{
			if (pos.Row >= block.Top && pos.Col >= -1 && pos.Col < 6 + block.Right)
			{
				var lineMax = 4 - block.Bottom;

				for (var l = block.Top; l < lineMax; l++)
				{
					if (l < 1) { continue; }
					var line = block[l, pos.Col];
					// There is overlap, so moving left was not possible before.
					if ((field[l - 1].row & line) != 0)
					{
						return true;
					}
				}
			}
			return false;
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
