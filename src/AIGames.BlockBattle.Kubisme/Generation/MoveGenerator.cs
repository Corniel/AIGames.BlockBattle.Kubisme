using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class MoveGenerator : IMoveGenerator
	{
		public IEnumerable<Field> GetFields(Field field, Block current, bool searchNoDrops)
		{
			if (searchNoDrops)
			{
				foreach (var candidate in GetReachableHoles(field, current))
				{
					yield return candidate.Field;
				}
			}
			foreach (var block in current.Variations)
			{
				int minRow = block.GetMinRow(field);
				int maxRow = block.GetMaxRow(field);

				foreach (var col in block.GetColumns(field))
				{
					for (var row = minRow; row <= maxRow; row++)
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
			if (field.Skips > 0)
			{
				yield return field.SkipBlock();
			}
		}

		public IEnumerable<MoveCandiate> GetMoves(Field field, Block current, bool searchNoDrops)
		{
			if (searchNoDrops)
			{
				foreach (var candidate in GetReachableHoles(field, current))
				{
					yield return candidate;
				}
			}
			foreach (var block in current.Variations)
			{
				int minRow = block.GetMinRow(field);
				int maxRow = block.GetMaxRow(field);

				foreach (var col in block.GetColumns(field))
				{
					for (var row = minRow; row <= maxRow; row++)
					{
						var test = field.Test(block, col, row);
						if (test == Field.TestResult.True)
						{
							var target = new Position(col, row);
							var applied = field.Apply(block, target);
							yield return new MoveCandiate(block.GetPath(field, col), applied);
						}
						if (test != Field.TestResult.Retry)
						{
							row = short.MaxValue;
						}
					}
				}
			}
			if (field.Skips > 0)
			{
				yield return new MoveCandiate(BlockPath.Skip, field.SkipBlock());
			}
		}

		/// <summary>Gets a score only based on characters of the current state.</summary>
		public static IEnumerable<MoveCandiate> GetReachableHoles(Field field, Block current)
		{
			int filterTopColomns = 0;
			int prevMirrored = Row.Filled;

			var targets = new int[field.RowCount];
			var found = false;
			var maxRow = 0;

			// loop through the rows.
			for (var r = field.FirstFilled; r < field.RowCount; r++)
			{
				var row = field[r];
				var rowMirrored = Row.Filled ^ row;

				// We can not enter any empty spot of this row, so stop searching.
				if (!current.IsReachable(rowMirrored, prevMirrored))
				{
					break;
				}
				prevMirrored = rowMirrored;
				filterTopColomns |= row;

				var holesMask = filterTopColomns & rowMirrored;
				if (holesMask != 0)
				{
					found = true;
					targets[r] = holesMask;
					maxRow = r;
				}
			}
			if (!found) { yield break; }
			foreach (var path in GetPaths(field, current, targets, maxRow))
			{
				yield return path;
			}
		}

		public static IEnumerable<MoveCandiate> GetPaths(Field field, Block current, int[] targets, int maxRow)
		{
			var dones = new bool[current.RotationVariations.Length, field.RowCount + 1, 10];

			TempPath first = current.GetFirstTempPath(field.FirstFilled);
			dones[0, first.Position.Row + 1, first.Position.Col] = true;

			var stack = new Stack<TempPath>();
			stack.Push(first);

			while (stack.Count != 0)
			{
				var temp = stack.Pop();

				if (temp.Position.Row > maxRow) { continue; }

				var test = field.Test(temp.Block, temp.Position);

				// No reason the investigate
				if (test == Field.TestResult.False) { continue; }

				var tr = temp.TurnRight();
				if (tr.Position.Col >= 0 && tr.Position.Col <= tr.Block.ColumnMaximum &&
					!dones[(int)tr.Block.Rotation, tr.Position.Row + 1, tr.Position.Col])
				{
					dones[(int)tr.Block.Rotation, tr.Position.Row + 1, tr.Position.Col] = true;
					stack.Push(tr);
				}

				var tl = temp.TurnLeft();
				if (tl.Position.Col >= 0 && tl.Position.Col <= tl.Block.ColumnMaximum &&
					!dones[(int)tl.Block.Rotation, tl.Position.Row + 1, tl.Position.Col])
				{
					dones[(int)tl.Block.Rotation, tl.Position.Row + 1, tl.Position.Col] = true;
					stack.Push(tl);
				}

				if (temp.Position.Col < temp.Block.ColumnMaximum && !dones[(int)temp.Block.Rotation, temp.Position.Row + 1, temp.Position.Col + 1])
				{
					dones[(int)temp.Block.Rotation, temp.Position.Row + 1, temp.Position.Col + 1] = true;
					stack.Push(temp.Right());
				}

				if (temp.Position.Col > 0 && !dones[(int)temp.Block.Rotation, temp.Position.Row + 1, temp.Position.Col - 1])
				{
					dones[(int)temp.Block.Rotation, temp.Position.Row + 1, temp.Position.Col - 1] = true;
					stack.Push(temp.Left());
				}

				// we have a fit.
				if (test == Field.TestResult.True)
				{
					if (temp.Block.TouchPosition(temp.Position, targets))
					{
						var unique = temp.Block.Variations.Length == temp.Block.RotationVariations.Length;

						// if the block is rotation only, first check if counter part did not arrive already.
						if (!unique)
						{
							var rot_org = (int)temp.Block.Rotation & 1;
							var rot_acc = rot_org | 2;

							unique =
								dones[rot_org, temp.Position.Row + 1, temp.Position.Col] ^
								dones[rot_acc, temp.Position.Row + 1, temp.Position.Col];
						}
						if (unique)
						{
							var apply = field.Apply(temp.Block, temp.Position);
							yield return new MoveCandiate(temp.Path, apply);
						}
					}
				}
				else
				{
					if (!dones[(int)temp.Block.Rotation, temp.Position.Row + 2, temp.Position.Col])
					{
						dones[(int)temp.Block.Rotation, temp.Position.Row + 2, temp.Position.Col] = true;
						stack.Push(temp.Down());
					}
				}
			}
		}
	}
}
