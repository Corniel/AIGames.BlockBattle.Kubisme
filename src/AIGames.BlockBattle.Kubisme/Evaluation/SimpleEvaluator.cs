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

			int filterFreeCells = Row.Filled;
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

			for (var r = 0; r < field.FirstFilled; r++)
			{
				score += 10 * pars.FreeCellWeights[r];
			}

			// loop through the rows.
			for (var r = field.FirstFilled; r < field.RowCount; r++)
			{
				var row = field[r];

				var rowMirrored = Row.Filled ^ row;
				var holesMask = filterTopColomns & rowMirrored;

				filterFreeCells &= rowMirrored;

				score += Row.Count[filterFreeCells] * pars.FreeCellWeights[r];
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
					var rowCount = Row.Count[row];

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
						//if (pars.NineHasComboPotential)
						//{
						//	if (Row.Count[row | filterComboPotential] == 9)
						//	{
						//		comboPotential++;
						//	}
						//	else
						//	{
						//		hasComboPotential = false;
						//	}
						//}
						//else
						//{
							hasComboPotential = false;
						//}
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
			for (var r = field.RowCount - 1; r >= field.FirstFilled; r--)
			{
				var row = field[r];
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

			for (var i = 0; i < comboPotential; i++)
			{
				score += (i + 1 + field.Combo) * pars.ComboPotential[i];
			}
			score += Row.Count[previous] * pars.Floor;

			return score;
		}

		public int WinScore { get { return short.MaxValue; } }
		public int DrawScore { get { return 0; } }
		public int LostScore { get { return short.MinValue; } }
	}
}
