namespace AIGames.BlockBattle.Kubisme
{
	public class Evaluator
	{
		public EvaluatorParameters Pars { get; set; }

		public Field Initial { get; set; }

		/// <summary>Gets the (static) score of a field.</summary>
		/// <remarks>
		/// Normally, this function should be split in several sub methods. But as 
		/// this is the most executed code of the all AI, speed is everything. The
		/// penalty for calling a method is small, but here, we don't want to spoil it.
		/// </remarks>
		public int GetScore(Field field, int depth)
		{
#if !DEBUG
			unchecked { // we trust this not to overflow.
#endif
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
			var comboPotential = 0;
			var hasComboPotential = true;

			// masks.
			var maskColumnOpen = (int)Row.Filled;
			var maskColumnClosed = 0;
			var maskColumnClosedPrev = 0;

			// row number.
			var rowNr = field.FirstFilled;

			// Loop through reachable rows.
			while (rowNr < field.RowCount)
			{
				var row = field[rowNr];
				var rowMirror = row ^ Row.Filled;

				var reachbleEmptyCells = rowMirror & maskColumnOpen;

				// No reachable cells left.
				if (reachbleEmptyCells == 0) { break; }

				var rowHoles = Row.Count[maskColumnClosed & rowMirror];
				var rowCount = Row.Count[row];
				// Groups of empty cells.
				var groups = Row.Groups[rowMirror];

				// Get bonuses for lines that can potentially be cleared by one block.
				if (groups == 1)
				{
					if (rowCount >= 6)
					{
						score += Pars.SingleGroupBonus[rowCount - 6];
					}
				}
				else
				{
					// Add score for single empties.
					var singleEmpties = Row.SingleEmpties[row | maskColumnClosed];
					score += Pars.SingleEmptiesCalc[singleEmpties];
				}

				// Add points for grouping
				score += Pars.Groups[groups];

				// Count holes.
				holes += rowHoles;
				
				if (hasComboPotential)
				{
					// If there is only one empty cell to go through, its over.
					if (rowCount == 9 || Row.Count[rowMirror & maskColumnOpen] < 2)
					{
						hasComboPotential = false;
					}
					else if (rowCount > 6 && groups == 1)
					{
						comboPotential++;
					}
				}

				// Detect T-spin and double clearance (T, L, J) potential.
				if (rowNr > 1)
				{
					// if 1 group, and 8, and the previous row is identical and no
					// holes are added (all reachable).
					if (rowCount == 8 && groups == 1 && rowHoles == 0 && row == field[rowNr - 1])
					{
						score += Pars.DoublePotentialO;
					}
					else if (rowCount == 9)
					{
						// Detect Tetris-score.
						// It should end with at least 4 reachable nine-rows.
						tetrisCount++;

						// If Tetris count is 2 of bigger, the previous one was 9.
						if (tetrisCount == 1)
						{
							var prev = field[rowNr - 1];
							var prevMirror = field[rowNr - 1] ^ Row.Filled;

							if (Row.Groups[prevMirror] == 1)
							{
								var prevMirrorCount = Row.Count[prevMirror];
								if (prevMirrorCount == 2)
								{
									if ((prevMirror & maskColumnClosedPrev) == 0)
									{
										score += Pars.DoublePotentialTSZ;
									}
								}
								else if (prevMirrorCount == 3)
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
						}
						// We'd like to detect a J/L triple.
						else if (tetrisCount > 1)
						{
							var prevMirror = field[rowNr - tetrisCount] ^ Row.Filled;
							// One hole, and 8 filled cells.
							if (Row.Groups[prevMirror] == 1 && Row.Count[prevMirror] == 2)
							{
								score += Pars.TriplePotentialJL;
							}
						}
					}
				}

				// reset Tetris count.
				if (rowCount != 9) { tetrisCount = 0; }

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
			score += Pars.ComboPotential[field.Combo, comboPotential];

			if (tetrisCount == 2) { score += Pars.DoublePotentialI; }
			else if (tetrisCount == 3) { score += Pars.TriplePotentialI; }
			else if (tetrisCount > 3) { score += Pars.TetrisPotential; }

			return score;
#if !DEBUG
			} // end unchecked.
#endif
		}
	}
}
