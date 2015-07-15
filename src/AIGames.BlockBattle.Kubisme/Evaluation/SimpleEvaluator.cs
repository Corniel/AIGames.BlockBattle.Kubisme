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

		public SimpleEvaluator()
		{
			pars = SimpleParameters.GetDefault();
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

		protected SimpleParameters pars { get; set; }

		public int GetScore(Field field)
		{
			var rMin = field.FirstNoneEmptyRow;

			var score = 0;
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;
			score += pars.RowWeights[rMin];

			int filterTopColomns = 0;
			int filterBlocades = 0;
			var holes = 0;
			var wallLeft = 0;
			var wallRight = 0;
			var blokades = 0;
			var neighborsH = 0;
			var neighborsV = 0;
			ushort previous = 0;

			var openNineRows = true;
			var nineRows = 0;

			for (var r = rMin; r < field.RowCount; r++)
			{
				var row = field[r].row;
				var rowMirrored = Row.Filled ^ row;
				var rowCount =Row.Count[row];
				var holesMask = filterTopColomns & rowMirrored;

				score += rowCount * pars.RowWeights[r];
				holes += Row.Count[holesMask];
				blokades += Row.Count[filterBlocades & row];

				// Give points for blocks against the wall, that are not under an hole.
				if ((row & MaskWallLeft) != 0 && (holes & MaskWallLeft) == 0) { wallLeft++; }
				if ((row & MaskWallRight) != 0 && (holes & MaskWallRight) == 0) { wallRight++; }

				filterBlocades |= holesMask;
				filterTopColomns |= row;

				// Only if this option is not blocked already.
				if (openNineRows)
				{
					// If facing a nine row.
					if (rowCount == 9)
					{
						// No top column should block the open file.
						if (Row.Count[filterTopColomns | row] == 9)
						{
							nineRows++;
						}
						else
						{
							openNineRows = false;
						}
					}
					else if(nineRows > 0)
					{
						openNineRows = false;
					}
				}

				neighborsV += NeighborsVertical[row];
				neighborsH += Row.Count[row & previous];
				previous = row;
			}
			score += wallLeft * pars.WallsLeft;
			score += wallRight + pars.WallsRight;
			score += holes * pars.Holes;
			score += blokades * pars.Blockades;
			score += neighborsH * pars.NeighborsHorizontal;
			score += neighborsV * pars.NeighborsVertical;
			score += Row.Count[previous] * pars.Floor;
			score += pars.NineRowWeights[nineRows];

			return score;
		}

		
	}
}
