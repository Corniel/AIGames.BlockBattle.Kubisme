using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.DecisionMaking
{
	public class SimpleMoveGenerator : IMoveGenerator
	{
		public IEnumerable<MoveCandiate> GetMoves(Field field, Block current, Position pos)
		{
			foreach (var block in current.Variations)
			{
				var minRow = field.FirstNoneEmptyRow - 4 + block.Bottom;
				var maxRow = field.RowCount - 3 + block.Bottom;

				var minCol = 0 - block.Left;
				var maxCol = 10 - 3 + block.Right;

				for (var col = minCol; col < maxCol; col++)
				{
					for (var row = minRow; row < maxRow; row++)
					{
						var test = field.Test(block, col, row);
						if (test == Field.TestResult.True)
						{
							var target = new Position(col, row);
							var applied = field.Apply(block, target);
							yield return new MoveCandiate(new MovePath(block.Rotation, target), applied);
						}
						if (test != Field.TestResult.Retry)
						{
							row = short.MaxValue;
						}
					}
				}

			}
		}
	}
}
