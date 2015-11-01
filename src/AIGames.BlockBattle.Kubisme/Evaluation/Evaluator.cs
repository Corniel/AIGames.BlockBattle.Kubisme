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
			var tetrisCount = 0;

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

				var rowCount = Row.Count[row];
				var groups = Row.Groups[row];

				// Add points for empty reachable cells.
				var emptyCellCount = Row.Count[reachbleEmptyCells];
				score += emptyCellCount * Pars.EmptyCells[rowNr];

				// Add points for grouping
				score += Pars.Groups[groups];

				// Count holes.
				holes += Row.Count[maskColumnClosed & rowMirror];

				// Count walls.
				if ((rowMirror & MaskWallLeft) == 0) { walls++; }
				if ((rowMirror & MaskWallRight) == 0) { walls++; }

				// Detect Tetris-score.
				// It should end with at least 4 reachable nine-rows.
				if (rowCount == 9) { tetrisCount++; }
				else { tetrisCount = 0; }

				// apply row to closed and open columns.
				maskColumnClosed |= row;
				maskColumnOpen = maskColumnClosed ^ Row.Filled;
				rowNr++;
			}

			// Evaluation for unreachable.
			score += Pars.UnreachableRowsCalc[field.RowCount - rowNr];

			// Add scores based on counters.
			score += holes * Pars.Holes;
			score += walls * Pars.Walls;

			if (tetrisCount > 3) { score += Pars.TetrisPotential; }


			return score;
		}
	}
}
