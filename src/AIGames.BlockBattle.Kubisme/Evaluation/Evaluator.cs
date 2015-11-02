﻿namespace AIGames.BlockBattle.Kubisme
{
	public class Evaluator
	{
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
				// Groups of empty cells.
				var groups = Row.Groups[rowMirror];

				// Add points for empty reachable cells.
				var emptyCellCount = Row.Count[reachbleEmptyCells];
				score += emptyCellCount * Pars.EmptyCells[rowNr];

				// Add points for grouping
				score += Pars.Groups[groups];

				// Count holes.
				holes += Row.Count[maskColumnClosed & rowMirror];

				// Detect Tetris-score.
				// It should end with at least 4 reachable nine-rows.
				if (rowCount == 9) { tetrisCount++; }
				else { tetrisCount = 0; }

				// apply row to closed and open columns.
				maskColumnClosed |= row;
				maskColumnOpen = maskColumnClosed ^ Row.Filled;
				rowNr++;
			}
			var unreachables = field.RowCount - rowNr;

			// If we have an unreachable track the max distance to its holes.
			if (unreachables > 0)
			{
				var unreachableHoles = field[rowNr] ^ Row.Filled;
				int maxRowNr = rowNr;
				for (var i = rowNr - 1; i >= field.FirstFilled; i--)
				{
					// we have a hit, update.
					if ((field[i] & unreachableHoles) != 0)
					{
						maxRowNr = i;
					}
				}
				score += (rowNr - maxRowNr) * Pars.UnreachableDistance;
			}

			// Evaluation for unreachable.
			score += Pars.UnreachableRowsCalc[unreachables];

			// Add scores based on counters.
			score += holes * Pars.Holes;

			if (tetrisCount > 3) { score += Pars.TetrisPotential; }


			return score;
		}
	}
}