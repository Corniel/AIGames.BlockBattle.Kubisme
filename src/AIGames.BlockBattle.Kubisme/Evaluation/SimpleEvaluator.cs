using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
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
		[ExcludeFromCodeCoverage]
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

		protected SimpleParameters pars { get; set; }

		/// <summary>Gets a score only based on characters of the current state.</summary>
		public int GetScore(Field field)
		{
			var score = 0;
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;

			int filterTopColomns = 0;

			var holes = 0;
			var wallLeft = 0;
			var wallRight = 0;
			var neighborsH = 0;
			var neighborsV = 0;
			ushort previous = 0;
			
			// Variables for unreachable garbage.
			int reachableMask = Row.Filled;

			var lineIsReachable = true;
			var unreachble = field.FirstFilled;

			// Variables for combo potential.
			var comboPotential = 0;
			var hasComboPotential = true;
			var filterComboPotential = 0;

			// Variables for T-Spin potential
			var hasTSpinPotential = false;
			var prevCount = 0;

			for (var r = 0; r < field.FirstFilled; r++)
			{
				score += pars.FreeRowWeights[r];
			}

			// loop through the rows.
			for (var r = field.FirstFilled; r < field.RowCount; r++)
			{
				var row = field[r];
				var rowCount = Row.Count[row];

				var rowMirrored = Row.Filled ^ row;
				var holesMask = filterTopColomns & rowMirrored;
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

				// Check if the line is still reachable.
				if (lineIsReachable)
				{
					reachableMask &= rowMirrored;
					lineIsReachable = reachableMask != 0;
					if (!lineIsReachable)
					{
						unreachble = r;
					}
				}

				// check for rows who can be filled in a combo.
				if (hasComboPotential)
				{
					if (rowCount == 7)
					{
						if (Row.Row7BlockOneHole.Contains(row | filterComboPotential))
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
						if (Row.Row8BlockOneHole.Contains(row | filterComboPotential))
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

				// If no T-Spin potential detected yet, and access to the row.
				// the previous has to be 7, and the current 9.
				if (!hasTSpinPotential && filterTopColomns != Row.Filled &&
					prevCount == 7 && rowCount == 9)
				{
					for (var i = 0; i < 8; i++)
					{
						if (BlockTUturn.TSpinRow2Mask[i] == row && BlockTUturn.TSpinRow1Mask[i] == previous)
						{
							hasTSpinPotential = true;
							break;
						}
					}
				}

				filterTopColomns |= row;

				neighborsV += NeighborsVertical[row];
				neighborsH += Row.Count[row & previous];
				previous = row;
				prevCount = rowCount;
			}

			// loop for blockades too.
			var blockades = 0;
			var lastBlockades = 0;
			var filterBlockades = 0;

			var lastHoles = 0;
			var filterLastBlockades = 0;
			var filterCurrentBlockades = 0;

			for (var r = field.RowCount - 1; r >= field.FirstFilled; r--)
			{
				// blockades
				var row = field[r];
				var mirrored = Row.Filled ^ row;
				filterBlockades |= mirrored;

				var blockadesMask = filterBlockades & row;
				blockades += Row.Count[blockadesMask];

				// last blockades
				filterLastBlockades = lastHoles & row;

				// blockade detection.
				if (filterLastBlockades != 0 && filterLastBlockades != filterCurrentBlockades)
				{
					lastBlockades = 0;
					filterCurrentBlockades = filterLastBlockades;
					lastBlockades = Row.Count[filterLastBlockades];
				}
				else
				{
					lastBlockades += Row.Count[filterCurrentBlockades & row];
				}
				lastHoles = mirrored;
			}

			score += wallLeft * pars.WallsLeft;
			score += wallRight * pars.WallsRight;
			score += holes * pars.Holes;
			score += blockades * pars.Blockades;
			score += lastBlockades * pars.LastBlockades;
			score += neighborsH * pars.NeighborsHorizontal;
			score += neighborsV * pars.NeighborsVertical;

			var iMax = field.RowCount - unreachble;
			for (var i = 0; i < iMax; i++)
			{
				score += pars.UnreachableWeights[i];
			}
			score += pars.ReachableRange[unreachble - field.FirstFilled];

			for (var i = 0; i < comboPotential; i++)
			{
				score += (i + 1 + field.Combo) * pars.ComboPotential[i];
			}
			score += Row.Count[previous] * pars.Floor;

			if (hasTSpinPotential)
			{
				score += pars.TSpinPotential;
			}

			return score;
		}

		public int WinScore { get { return short.MaxValue; } }
		public int DrawScore { get { return 0; } }
		public int LostScore { get { return short.MinValue; } }
	}
}
