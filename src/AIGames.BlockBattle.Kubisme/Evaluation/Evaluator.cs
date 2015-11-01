namespace AIGames.BlockBattle.Kubisme
{
	public class Evaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		public EvaluatorParameters Pars { get; set; }

		public Field Initial { get; set; }

		public int GetScore(Field field, int depth)
		{
			var score = 0;

			// Points for static evaluation.
			score += field.Points * Pars.Points;
			score += field.Combo * Pars.Combo;
			score += field.Skips * Pars.Skips;

			// Evaluation for free space.
			score += Pars.EmptyRowsCalc[field.FirstFilled];

			// counters
			var holes = 0;
			var walls = 0;

			var maskColumnOpen = (int)Row.Filled;
			var maskColumnClosed = 0;
			var rowNr = field.FirstFilled;

			// Loop through reachable rows.
			while (rowNr < field.RowCount)
			{
				var row = field[rowNr];
				var rowMirror = row ^ Row.Filled;

				var reachbleEmptyCells = rowMirror & maskColumnOpen;
				
				// No reachable cells left.
				if (reachbleEmptyCells== 0) { break; }

				// Add points for empty reachable cells.
				var emptyCellCount = Row.Count[reachbleEmptyCells];
				score += emptyCellCount * Pars.EmptyCells[rowNr];

				// Add points for grouping
				var groups = Row.Groups[row];
				score += Pars.Groups[groups];

				// Count holes.
				holes += Row.Count[maskColumnClosed & rowMirror];

				// Count walls.
				if ((rowMirror & MaskWallLeft) == 0) { walls++; }
				if ((rowMirror & MaskWallRight) == 0) { walls++; }

				// apply row to closed and open columns.
				maskColumnClosed |= row;
				maskColumnOpen = maskColumnClosed ^ Row.Filled;
				rowNr++;
			}

			// Evaluation for unreachable.
			score += Pars.UnreachableRowsCalc[field.RowCount - rowNr];

			// Add counters.
			score += holes * Pars.Holes;
			score += walls * Pars.Walls;

			return score;
		}
	}
}
