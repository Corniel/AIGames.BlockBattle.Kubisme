using System.Diagnostics.CodeAnalysis;

namespace AIGames.BlockBattle.Kubisme
{
	public class ComplexEvaluator : IEvaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		private static readonly byte[] NeighborsVertical = new byte[Row.Locked + 1];
		static ComplexEvaluator()
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
				pars = value as ComplexParameters;
			}
		}
		protected ComplexParameters pars { get; set; }

		public Field Initial { get; set; }
		
		public int GetScore(Field field, int depth)
		{
			var score = 0;

			// Points for static evaluation.
			score += pars.GarbagePotential[field.Points % 3];
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;
			score += field.Skips * pars.Skips;
			score += field.FirstFilled * pars.FirstFilled;
			score += pars.FreeRows[field.FirstFilled];

			#region Filled row logic

			int filterTopColomns = 0;

			var holes = 0;
			var wallLeft = 0;
			var wallRight = 0;
			var neighborsH = 0;
			var neighborsV = 0;
			ushort previous = 0;

			var unreachableFound = false;

			var oBlockPlacement = false;
			
			// Variables for T-Spin potential
			var hasTSpinPotential = false;
			var prevCount = 0;

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

				neighborsV += NeighborsVertical[row];
				neighborsH += Row.Count[row & previous];
				previous = row;
				prevCount = rowCount;

				if (!unreachableFound && !oBlockPlacement && r > 1)
				{
					var reachableBlock = row & (filterTopColomns ^ Row.Filled);

					// Two blocks concatenated
					if (Row.HasRow2BlocksConnected(reachableBlock, Row.Count[reachableBlock]))
					{
						oBlockPlacement = true;
						score += pars.OBlockPlacement;
					}
				}

				// We found an unreachable spot.
				if (!unreachableFound && filterTopColomns == Row.Filled)
				{
					unreachableFound = true;
					var unreachables = field.RowCount - r + 1;

					score += unreachables * pars.UnreachableFactor;
					score += pars.Unreachables[unreachables];
				}

				// Update the filter top columns.
				filterTopColomns |= row;
			}

			score += holes * pars.Holes;
			score += wallLeft * pars.WallsLeft;
			score += wallRight * pars.WallsRight;
			score += neighborsH * pars.NeighborsHorizontal;
			score += neighborsV * pars.NeighborsVertical;

			score += Row.Count[previous] * pars.Floor;

			if (hasTSpinPotential)
			{
				score += pars.TSpinPotential;
			}

			#endregion

			#region Blockades

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

			score += blockades * pars.Blockades;
			score += lastBlockades * pars.LastBlockades;
			#endregion

			return score;
		}
	}
}
