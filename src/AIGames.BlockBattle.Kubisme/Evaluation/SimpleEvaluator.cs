using AIGames.BlockBattle.Kubisme.Models;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public class SimpleEvaluator : IEvaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		private static readonly byte[] NeighborsVertical = new byte[Row.Locked + 1];
		static SimpleEvaluator()
		{
			for (ushort i = Row.Empty; i <= Row.Filled; i++)
			{
				int count = 0;
				int prev = i & 1;
				for (var c = 1; c < 10; c++)
				{
					var cur = i & Row.Flag[c];
					if (prev > 0 && cur > 0)
					{
						count++;
					}
					prev = cur;
				}
				NeighborsVertical[i] = (byte)count;
			}
		}
		public IParameters Parameters
		{
			get
			{
				return pars;
			}
			set
			{
				pars = value as SimpleParameters;
			}
		}
		/// <summary>A set of masks with 7 blocks and one hole.</summary>
		public static readonly HashSet<int> Row7ComboPotential = new HashSet<int>() { 0X03F8, 0X03F1, 0X03E3, 0X03C7, 0X038F, 0X031F, 0X023F, 0X007F, };
		/// <summary>A set of masks with 8 blocks and one hole.</summary>
		public static readonly HashSet<int> Row8ComboPotential = new HashSet<int>() { 0X03FC, 0X03F9, 0X03F3, 0X03E7, 0X03CF, 0X039F, 0X033F, 0X027F, 0X00FF, };

		protected SimpleParameters pars { get; set; }

		/// <summary>Gets a score only based on characters of the current state.</summary>
		public int GetScore(Field field)
		{
			var score = 0;
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;

			int filterTopColomns = 0;
			int filterBlockades = 0;
			int filterComboPotential = 0;
			var holes = 0;
			var wallLeft = 0;
			var wallRight = 0;
			var blockades = 0;
			var neighborsH = 0;
			var neighborsV = 0;
			var comboPotential = 0;
			ushort previous = 0;

			var hasComboPotential = true;

			// loop through the rows.
			for (var r = 0; r < field.RowCount; r++)
			{
				var rw = field[r];
				var row = rw.row;
				if (row == Row.Empty) { continue; }

				var rowCount = Row.Count[row];
				var rowMirrored = Row.Filled ^ row;
				var holesMask = filterTopColomns & rowMirrored;

				score += pars.RowCountWeights[rowCount] * pars.RowWeights[r];
				holes += Row.Count[holesMask];
				
				// Give points for blocks against the wall, that are not under an hole.
				if ((row & MaskWallLeft) != 0)
				{
					wallLeft++;
				}
				// Else reset.
				else
				{
					wallLeft = 0;
				}
				if ((row & MaskWallRight) != 0)
				{
					wallRight++;
				}
				else
				{
					wallRight = 0;
				}

				// check for rows who can be filled in a combo.
				if (hasComboPotential)
				{
					if (rowCount == 7)
					{
						if (Row7ComboPotential.Contains(row | filterComboPotential))
						{
							comboPotential++;
						}
						else
						{
							hasComboPotential = false;
						}
					}
					else if (rowCount == 8)
					{
						if (Row8ComboPotential.Contains(row | filterComboPotential))
						{
							comboPotential++;
						}
						else
						{
							hasComboPotential = false;
						}
					}
					else if (rowCount == 9)
					{
						hasComboPotential = false;
					}
					else
					{
						filterComboPotential |= row;
					}
				}

				filterTopColomns |= row;

				neighborsV += NeighborsVertical[row];
				neighborsH += Row.Count[row & previous];
				previous = row;
			}

			// loop for blockades too.
			for (var r = field.RowCount - 1; r >= 0; r--)
			{
				var rw = field[r];
				var row = rw.row;
				if (row == Row.Empty) { break; }
				filterBlockades |= Row.Filled ^ row;

				var blockadesMask = filterBlockades & row;
				blockades += Row.Count[blockadesMask];
			}

			score += wallLeft * pars.WallsLeft;
			score += wallRight * pars.WallsRight;
			score += holes * pars.Holes;
			score += blockades * pars.Blockades;
			score += neighborsH * pars.NeighborsHorizontal;
			score += neighborsV * pars.NeighborsVertical;
			score += pars.ComboPotential[comboPotential];
			score += Row.Count[previous] * pars.Floor;

			return score;
		}

		public int WinScore { get { return short.MaxValue; } }
		public int DrawScore { get { return 0; } }
		public int LostScore { get { return short.MinValue; } }
	}
}
