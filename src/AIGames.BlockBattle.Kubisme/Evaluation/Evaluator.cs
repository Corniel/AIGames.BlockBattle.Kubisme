namespace AIGames.BlockBattle.Kubisme
{
	public class Evaluator
	{

		/// <summary>A mask for a match on the 1st and the 8th column of a row.</summary>
		public const ushort Mask1st8thColomn = 0x0102;


		/// <summary>Gets the (static) score of a field.</summary>
		/// <remarks>
		/// Normally, this function should be split in several sub methods. But as 
		/// this is the most executed code of the all AI, speed is everything. The
		/// penalty for calling a method is small, but here, we don't want to spoil it.
		/// </remarks>
		public int GetScore(Field field, int depth, EvaluatorParameters pars)
		{
#if !DEBUG
			unchecked { // we trust this not to overflow.
#endif
			var score = 0;

			// Points for static evaluation.
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;
			score += field.Skips * pars.Skips;

			#region A block on the 1st and 8th will disallow the clearance of a row one turn earlier.

			var firstFilled = field.FirstFilled;
			if (firstFilled > 0 && firstFilled < field.RowCount && (field[firstFilled] & Mask1st8thColomn) != 0)
			{
				firstFilled--;
			}
			// Evaluation for free space.
			score += pars.EmptyRowsCalc[firstFilled];

			#endregion

			var rowIndex = field.FirstFilled;
			var row0 = 0;
			var row1 = 0;
			var row2 = 0;
			var row0Mirror = 0;
			var row0Holes = 0;

			int row0Open = Row.Filled;
			int row0Closed = 0;

			var countHoleReachable = 0;
			var countHoleUnreachable = 0;

			var countRow0 = 0;
			var countRow0Holes = 0;
			var countRow0Group = 0;
			var countRow1 = 0;
			var countRow1Group = 0;
			
			#region Reachable Area
			
			for (; rowIndex < field.RowCount; rowIndex++)
			{
				row0 = field[rowIndex];
				row0Mirror = row0 ^ Row.Filled;

				// Not reachable, redo as unreachable.
				if ((row0Open & row0Mirror) == 0)
				{
					rowIndex--;
					break;
				}

				row0Holes = row0Mirror & row0Closed;
				countRow0 = Row.Count[row0];
				countRow0Group = Row.Groups[row0Mirror];

				#region  Add points for grouping
				score += pars.Groups[countRow0Group];

				if (countRow0Group == 1)
				{
					// Get bonuses for lines that can potentially be cleared by one block.
					if (countRow0 >= 6)
					{
						score += pars.SingleGroupBonus[countRow0 - 6];
					}
				}
				else
				{
					// Add score for single empties.
					var singleEmpties = Row.SingleEmpties[row0 | row0Closed];
					score += pars.SingleEmptiesCalc[singleEmpties];
				}
				#endregion

				#region Handle holes

				if (row0Holes != 0)
				{
					countRow0Holes = Row.Count[row0Holes];

					if (countRow0Holes == 1)
					{
						switch (row0Holes)
						{
							// X.........
							case 0x001:
								//.XX.......
								if ((row0Mirror & 0x006) == 0x006) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// .X........
							case 0x002:
								//..XX......
								if ((row0Mirror & 0x00C) == 0x00C) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// ..X.......
							case 0x004:
								//XX........ OR ...XX.....
								if ((row0Mirror & 0x003) == 0x003 || (row0Mirror & 0x018) == 0x018) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// ...X......
							case 0x008:
								//.XX....... OR ....XX....
								if ((row0Mirror & 0x006) == 0x006 || (row0Mirror & 0x030) == 0x030) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// ....X.....
							case 0x010:
								//..XX...... OR .....XX...
								if ((row0Mirror & 0x00C) == 0x00C || (row0Mirror & 0x060) == 0x060) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// .....X....
							case 0x020:
								//...XX..... OR ......XX..
								if ((row0Mirror & 0x018) == 0x018 || (row0Mirror & 0x0C0) == 0x0C0) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// ......X...
							case 0x040:
								//....XX.... OR .......XX.
								if ((row0Mirror & 0x030) == 0x030 || (row0Mirror & 0x180) == 0x180) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// .......X..
							case 0x080:
								//.....XX... OR ........XX
								if ((row0Mirror & 0x060) == 0x060 || (row0Mirror & 0x300) == 0x300) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// ........X.
							case 0x100:
								//......XX..
								if ((row0Mirror & 0x0C0) == 0x0C0) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
							// .........X
							case 0x200:
								//.......XX.
								if ((row0Mirror & 0x180) == 0x180) { countHoleReachable++; }
								else { countHoleUnreachable++; }
								break;
						}
					}

					else
					{
						countHoleUnreachable += countRow0Holes;
					}
				}
				#endregion

				#region T-spin potential

				// X...XXXXXX
				// XX.XXXXXXX
				else if (countRow0 == 9 && countRow1 == 7 && countRow1Group == 1)
				{
					// X...XXXXXX
					// XX.XXXXXXX
					// ----------
					// .X.X...... (2 groups)
					if (Row.Groups[(row1 ^ Row.Filled) ^ row0Mirror] == 2)
					{
						// ...X......
						// X...XXXXXX
						// ----------
						// X..XXXXXXX (count 8)
						if (Row.Count[row1 | row2] == 8)
						{
							score += pars.TSpinPontential;
						}
					}
				}

				#endregion

				// Update history.
				row0Open &= row0Mirror;
				row0Closed |= row0;
				countRow1 = countRow0;
				countRow1Group = countRow0Group;
				row2 = row1;
				row1 = row0;
			}
				#endregion

			#region Vertical I potential

			if (rowIndex >= 4)
			{
				var cearenceI = 0;
				// If we're on the floor, -1, -4, instead 0, -3.
				if (Row.Count[field[rowIndex - (rowIndex == field.RowCount ? 4 : 0)]] == 9) { cearenceI++; }
				if (Row.Count[field[rowIndex - 1]] == 9) { cearenceI++; }
				if (Row.Count[field[rowIndex - 2]] == 9) { cearenceI++; }
				if (Row.Count[field[rowIndex - 3]] == 9) { cearenceI++; }
				score += pars.TetrisPotential[cearenceI];
			}
			#endregion

			#region Unreachable area

			for (; rowIndex < field.RowCount; rowIndex++)
			{
				row0Mirror = field[rowIndex] ^ Row.Filled;
				// Points for groups.
				score += pars.Groups[Row.Groups[row0Mirror]];
				// Points for holes.
				countHoleUnreachable += Row.Count[row0Mirror];
			}
			#endregion

			score += countHoleReachable * pars.HolesReachable;
			score += countHoleUnreachable * pars.HolesUnreachable;

			return score;
#if !DEBUG
			} // end unchecked.
#endif
		}
	}
}
