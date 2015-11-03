namespace AIGames.BlockBattle.Kubisme
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
			var maskColumnClosedPrev = 0;
			var rowNr = field.FirstFilled;

			// Loop through reachable rows.
			while (rowNr < field.RowCount)
			{
				var row = field[rowNr];
				var rowMirror = row ^ Row.Filled;

				var reachbleEmptyCells = rowMirror & maskColumnOpen;
				
				// No reachable cells left.
				if (reachbleEmptyCells== 0) { break; }

				var rowHoles =  Row.Count[maskColumnClosed & rowMirror];
				var rowCount = Row.Count[row];
				// Groups of empty cells.
				var groups = Row.Groups[rowMirror];

				// Get bonuses for lines that can potentially be cleared by one block.
				if (groups == 1 && rowCount >= 6)
				{
					score += Pars.SingleGroupBonus[rowCount - 6];
				}

				// Add points for empty reachable cells.
				var emptyCellCount = Row.Count[reachbleEmptyCells];
				score += emptyCellCount * Pars.EmptyCells[rowNr];

				// Add points for grouping
				score += Pars.Groups[groups];

				// Count holes.
				holes += rowHoles;

				// Detect T-spin and double clearance (T, L, J) potential.
				if (rowCount == 9 && rowNr > 1)
				{
					var prev = field[rowNr - 1];
					var prevMirror = field[rowNr - 1] ^ Row.Filled;
					if (Row.Count[prevMirror] == 3 && Row.Groups[prevMirror] == 1)
					{
						if ((prevMirror & maskColumnClosedPrev) == 0)
						{
							score += Pars.DoublePotentialJLT;
						}
						// Potential T-spin.
						else if (rowNr > 2 && field.FirstFilled < rowNr - 1)
						{
							for (var col = 0; col < 8; col++)
							{
								if (BlockTUturn.TSpinRow2Mask[col] == row)
								{
									if (BlockTUturn.TSpinRow1Mask[col] == prev)
									{
										var top = field[rowNr - 2];
										var maskTSpinTop = BlockTUturn.TSpinTopMask[col];
										var match = top & maskTSpinTop;
										// Note, we don't have to check for the center blockade.
										// If that was the case, the current line would be unreachable.
										if (match != 0 && match != maskTSpinTop)
										{
											score += Pars.TSpinPontential;
										}
									}
									break;
								}
							}
						}
					}
				}

				// Detect Tetris-score.
				// It should end with at least 4 reachable nine-rows.
				if (rowCount == 9) { tetrisCount++; }
				else { tetrisCount = 0; }

				// apply row to closed and open columns.
				maskColumnClosedPrev = maskColumnClosed;
				maskColumnClosed |= row;
				maskColumnOpen = maskColumnClosed ^ Row.Filled;
				rowNr++;
			}
			var unreachables = field.RowCount - rowNr;

			// Evaluation for unreachable.
			score += Pars.UnreachableRowsCalc[unreachables];

			// Add scores based on counters.
			score += holes * Pars.Holes;

			if (tetrisCount > 3) { score += Pars.TetrisPotential; }


			return score;
		}
	}
}
