using System.Collections.Generic;
using System.Linq;

namespace AIGames.BlockBattle.Kubisme
{
	public class MoveGenerator : IMoveGenerator
	{
		public IEnumerable<Field> GetFields(Field field, Block current,  bool searchNoDrops)
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
			}
			if (searchNoDrops)
			{
				//foreach (var candidate in GetReachableHoles(field, current))
				//{
				//	yield return candidate.Field;
				//}
			}
		}

		public IEnumerable<MoveCandiate> GetMoves(Field field, Block current, bool searchNoDrops)
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
							yield return new MoveCandiate(block.InitialPath.AddShift(col - current.Start.Col).AddDrop(), applied);
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
				//foreach (var candidate in GetReachableHoles(field, current))
				//{
				//	yield return candidate;
				//}
			}
		}

		public static BlockPath GetPath(Field field, Block block, Position target)
		{
			var options = new Dictionary<Position, BlockPath>();
			var queue = new Queue<Position>();

			queue.Enqueue(block.Start);
			options[block.Start] = block.InitialPath;

			while (queue.Count > 0)
			{
				var position = queue.Dequeue();
				var path = options[position];

				if (position.Equals(target))
				{
					return path;
				}

				var test = field.Test(block, position);
				if (test == Field.TestResult.False) { continue; }

				if (position.Row < target.Row && test != Field.TestResult.True)
				{
					var down = position.Down;
					if (!options.ContainsKey(down))
					{
						queue.Enqueue(down);
						options[down] = path.AddDown();
					}
				}

				if (position.Col > block.ColumnMinimum)
				{
					var left = position.Left;
					if (!options.ContainsKey(left))
					{
						queue.Enqueue(left);
						options[left] = path.AddLeft();
					}
				}
				if (position.Col < block.ColumnMaximum)
				{
					var right = position.Right;
					if (!options.ContainsKey(right))
					{
						queue.Enqueue(right);
						options[right] = path.AddRight();
					}
				}
			}
			return BlockPath.None;
		}

		/// <summary>Gets a score only based on characters of the current state.</summary>
		public static IEnumerable<MoveCandiate> GetReachableHoles(Field field, Block current)
		{
			int filterTopColomns = 0;
			int open = Row.Filled;
			int prev = Row.Filled;

			// loop through the rows.
			for (var r = field.FirstFilled; r < field.RowCount; r++)
			{
				var row = field[r];
				var rowMirrored = Row.Filled ^ row;

				open &= rowMirrored;
				var openCount = Row.Count[open];
				// If only one hole, no access.
				if (openCount < 2)
				{
					break;
				}
				// If there are no free spots combined on a role there is no access.
				if (openCount > 6 && !Row.Row8BlockOneHole.Any(map => (map & open) != 0))
				{
					break;
				}

				var movement = prev & rowMirrored;
				var movementCount = Row.Count[movement];
				if (movementCount < 2)
				{
					break;
				}
				// there should be a way for a block to get from one row to another.
				if (movementCount > 6 && !Row.Row8BlockOneHole.Any(map => (map & movement) != 0))
				{
					break;
				}

				// is there a hole anyway?
				var holesMask = filterTopColomns & rowMirrored;
				if (holesMask != 0)
				{
					for (var i = 0; i < 10; i++)
					{
						if ((holesMask & Bits.FlagUInt16[i]) != 0)
						{
							foreach (var block in current.Variations)
							{
								foreach (var transfer in block.Touches)
								{
									var cc = i + transfer.Col;
									if (cc >= block.ColumnMinimum && cc <= block.ColumnMaximum)
									{
										var rr = r + transfer.Row;

										var minRow = rr - 4 + block.Bottom;
										var maxRow = rr - 3 + block.Bottom;

										if (minRow >= 0 && maxRow < field.RowCount)
										{
											var test = field.Test(block, cc, rr);
											if (test == Field.TestResult.True)
											{
												var target = new Position(cc, rr);
												var path = GetPath(field, block, target);
												if (!path.Equals(BlockPath.None))
												{
													var applied = field.Apply(block, target);
													yield return new MoveCandiate(path, applied);
												}
											}
										}
									}
								}
							}
						}
					}
				}

				filterTopColomns |= row;

				prev = rowMirrored;
			}
		}
	}
}
