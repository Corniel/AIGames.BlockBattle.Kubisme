﻿namespace AIGames.BlockBattle.Kubisme
{
	public class Evaluator
	{

		/// <summary>A mask for a match on the 1st and the 8th column of a row.</summary>
		public const ushort Mask1st8thColomn = 0x102;

		// XX........
		public const ushort PerfectClearOneRow0 = 0x003;
		// ....XX....
		public const ushort PerfectClearOneRow1 = 0x030;
		// ........XX
		public const ushort PerfectClearOneRow2 = 0x300;


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

			#region A block on the 1st and 8th will disallow the clearance of a row one turn earlier.

			var firstFilled = field.FirstFilled;
			if (firstFilled > 0 && firstFilled < field.RowCount && (field[firstFilled] & Mask1st8thColomn) != 0)
			{
				firstFilled--;
			}
			// Evaluation for free space.
			score += pars.EmptyRowsCalc[firstFilled];

			#endregion

			// Points for static evaluation.
			score += field.Points * pars.PointsCalc[firstFilled];
			score += field.Combo * pars.ComboCalc[firstFilled];
			score += field.Skips * pars.SkipsCalc[firstFilled];

			var rowIndex = field.FirstFilled;
			var row0 = 0;
			var row1 = 0;
			var row2 = 0;
			var row3 = 0;
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
			var countRow2Group = 0;

			#region Reachable Area

			var rowIndexHoles = -1;
			for (; rowIndex < field.RowCount; rowIndex++)
			{
				// For holes use an index based on the first filled.
				rowIndexHoles++;
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
								if ((row0Mirror & 0x006) == 0x006) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// .X........
							case 0x002:
								//..XX......
								if ((row0Mirror & 0x00C) == 0x00C) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// ..X.......
							case 0x004:
								//XX........ OR ...XX.....
								if ((row0Mirror & 0x003) == 0x003 || (row0Mirror & 0x018) == 0x018) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// ...X......
							case 0x008:
								//.XX....... OR ....XX....
								if ((row0Mirror & 0x006) == 0x006 || (row0Mirror & 0x030) == 0x030) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// ....X.....
							case 0x010:
								//..XX...... OR .....XX...
								if ((row0Mirror & 0x00C) == 0x00C || (row0Mirror & 0x060) == 0x060) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// .....X....
							case 0x020:
								//...XX..... OR ......XX..
								if ((row0Mirror & 0x018) == 0x018 || (row0Mirror & 0x0C0) == 0x0C0) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// ......X...
							case 0x040:
								//....XX.... OR .......XX.
								if ((row0Mirror & 0x030) == 0x030 || (row0Mirror & 0x180) == 0x180) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// .......X..
							case 0x080:
								//.....XX... OR ........XX
								if ((row0Mirror & 0x060) == 0x060 || (row0Mirror & 0x300) == 0x300) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// ........X.
							case 0x100:
								//......XX..
								if ((row0Mirror & 0x0C0) == 0x0C0) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
							// .........X
							case 0x200:
								//.......XX.
								if ((row0Mirror & 0x180) == 0x180) { countHoleReachable += pars.HolesReachableCalc[rowIndexHoles]; }
								else { countHoleReachable += pars.HolesUnreachableCalc[rowIndexHoles]; }
								break;
						}
					}

					else
					{
						countHoleUnreachable += countRow0Holes * pars.HolesUnreachableCalc[rowIndexHoles];
					}

					#region Single T-spin upper

					if (countRow1 == 7 && countRow1Group == 1)
					{
						// .X.X...... (2 groups)
						if (Row.Groups[(row1 ^ Row.Filled) ^ row0Mirror] == 2)
						{
							score += pars.TSpinSingle1PotentialCalc[rowIndex];
						}
					}
				}
				#endregion

				#region T-spin potential

				#region Double T-spin
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
						var merged = Row.Count[row1 | row2];
						if (merged == 8)
						{
							score += pars.TSpinDoublePotentialCalc[rowIndex];
						}
						else if (merged == 7)
						{
							score += pars.TDoubleClearPotentialCalc[rowIndex];
						}
					}
				}
				#endregion

				#region Single T-spin lower
				// X...XXX??X
				// XX.XXXXXXX
				else if (countRow0 == 9 && countRow1 < 7)
				{
					// X...XXXXX?
					// XX.XXXXXXX
					// ----------
					// .X.X...... (1 group more than before)
					if (Row.Groups[row1 | row0Mirror] == Row.Groups[row1] + 1)
					{
						// the hole on row zero should stitch to 1 group, not to 2.
						var merged = row2 | row0Mirror;
						if (Row.Groups[merged] == Row.Groups[row2])
						{
							score += pars.TSpinSingle0PotentialCalc[rowIndex];
						}
					}
				}
				#endregion

				#endregion

				// With 7 in row1 see hole part.

				#endregion

				// Update history.
				row0Open &= row0Mirror;
				row0Closed |= row0;
				countRow1 = countRow0;
				countRow2Group = countRow1Group;
				countRow1Group = countRow0Group;
				row3 = row2;
				row2 = row1;
				row1 = row0;
			}
			#endregion
			
			#region Unreachable area

			var unreachbleGroups = 0;
		
			score += pars.UnreachableRowsCalc[field.RowCount - rowIndex];

			for (; rowIndex < field.RowCount; rowIndex++)
			{
				row0Mirror = field[rowIndex] ^ Row.Filled;
				unreachbleGroups = Row.Groups[row0Mirror];
				// Points for groups.
				score += pars.Groups[unreachbleGroups];
				// Points for holes.
				countRow0Holes = Row.Count[row0Mirror];
				countHoleUnreachable += countRow0Holes * pars.HolesUnreachableCalc[rowIndexHoles];
				
				rowIndexHoles++;
			}
			#endregion

			#region Perfect clear

			if (countHoleReachable == 0 && countHoleUnreachable == 0)
			{
				var filled = field.RowCount - field.FirstFilled;
				if (filled < 5)
				{
					var hasPerfectClearPotential = false;
					if (filled == 1)
					{
						hasPerfectClearPotential =
							row0 == PerfectClearOneRow0 ||
							row0 == PerfectClearOneRow1 ||
							row0 == PerfectClearOneRow2 ||
							// 4 empty cells as one group.
							(Row.Count[row0] == 6 && Row.Groups[row0Mirror] == 1);
					}
					else
					{
						var toFill = filled * 10 - field.Count;
						hasPerfectClearPotential =
							// Can be divided by 4.
							(toFill & 3) == 0 &&
							// maximum block left only.
							toFill <= 12 &&
							// Can be filled 'easily' because all space is connected.
							Row.Groups[row0Mirror] == 1 &&
							Row.Groups[row1 ^ Row.Filled] == 1 &&
							Row.Groups[row2 ^ Row.Filled] == 1 &&
							Row.Groups[row3 ^ Row.Filled] == 1;
					}

					if (hasPerfectClearPotential)
					{
						score += pars.PerfectClearPotentialCalc[field.RowCount];
					}
				}
			}
			#endregion

			score += countHoleReachable;
			score += countHoleUnreachable;

			return score;
#if !DEBUG
			} // end unchecked.
#endif
		}
	}
}

