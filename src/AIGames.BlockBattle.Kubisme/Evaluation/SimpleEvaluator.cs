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
			pars = Parameters.GetDefault();

			for (var count = 1; count <= 10; count++)
			{
				for (var row = 0; row <= 20; row++)
				{
					RowWeights[count, row] = count * pars.RowWeights[row];
				}
			}
		}

		private int[,] RowWeights = new int[11, 21];

		public class Parameters
		{
			public Parameters()
			{
				RowWeights = new int[21];
				NineRowWeights = new int[21];
			}

			public int[] RowWeights { get; set; }
			public int[] NineRowWeights { get; set; }

			public int Points { get; set; }
			public int Combo { get; set; }
			public int Holes { get; set; }
			public int Blockades { get; set; }
			public int WallsLeft { get; set; }
			public int WallsRight { get; set; }
			public int FLoor { get; set; }
			public int NeighborsHorizontal { get; set; }
			public int NeighborsVertical { get; set; }

			public static Parameters GetDefault()
			{
				return new Parameters()
				// 1.102.953  0.06:50:48 Score: 10,32%, Win: 125,2, Lose: 116,2 Runs: 1.550, ID: 286, Max: 89
				{
					RowWeights = new int[] { -632, -490, -291, -86, 126, 221, 94, 242, 34, 2, -20, -76, 47, -88, -218, 155, 89, 14, -9, -66, -96 },
					NineRowWeights = new int[] { 94, 205, -101, -88, -55, -153, 2, -161, 2, 37, 27, -37, 230, -22, -116, -122, 65, 9, 4, 175, -68 },
					Points = 530,
					Combo = 175,
					Holes = -343,
					Blockades = -232,
					WallsLeft = 210,
					WallsRight = 71,
					FLoor = 18,
					NeighborsHorizontal = -207,
					NeighborsVertical = 231,
				};
			}
		}

		public Parameters pars { get; set; }

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

				score += RowWeights[rowCount, r];
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
			score += Row.Count[previous] * pars.FLoor;
			score += pars.NineRowWeights[nineRows];

			return score;
		}
	}
}
