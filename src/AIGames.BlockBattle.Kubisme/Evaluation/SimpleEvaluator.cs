using AIGames.BlockBattle.Kubisme.Models;

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

				for(var i = 0; i < 21; i++)
				{
					RowWeights[i] = -30000 + 100 * i;
				}
				Holes = -387002;
			}

			public int[] RowWeights { get; set; }

			public int Points { get; set; }
			public int Combo { get; set; }
			public int Holes { get; set; }
			public int Blockades { get; set; }
			public int Walls { get; set; }
			public int FLoor { get; set; }
			public int NeighborsHorizontal { get; set; }
			public int NeighborsVertical { get; set; }

			public static Parameters GetDefault()
			{
				return new Parameters()
				// 196,015  484:45 204.12 (  113), ID:   1446, Max: 601
				{
					//RowWeights = new int[] { -3001, -2845, -3922, -1558, -1231, -1790, -1839, -1346, -1874, -1810, -1324, -1204, -1423, -30336, 30584, -6707, -8516, -536, -1474, -1326, -1438 },
					//Points = 54501,
					//Combo = -3229,
					//Holes = -38782,
					//Blockades = -23291,
					//Walls = 50496,
					//NeighborsHorizontal = -33413,
					//NeighborsVertical = 33037,
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
			var walls = 0;
			var blokades = 0;
			var neighborsH = 0;
			var neighborsV = 0;
			ushort previous = 0;

			for (var r = rMin; r < field.RowCount; r++)
			{
				var row = field[r].row;

				score += RowWeights[Row.Count[row], r];

				if ((row & MaskWallLeft) != 0) { walls++; }
				if ((row & MaskWallRight) != 0) { walls++; }

				var holesMask = filterTopColomns & (Row.Filled ^ row);

				holes += Row.Count[holesMask];
				blokades += Row.Count[filterBlocades & row];

				filterBlocades |= holesMask;
				filterTopColomns |= row;

				neighborsV += NeighborsVertical[row];
				neighborsH += Row.Count[row & previous];
				previous = row;
			}
			score += walls * pars.Walls;
			score += holes * pars.Holes;
			score += blokades * pars.Blockades;
			score += neighborsH * pars.NeighborsHorizontal;
			score += neighborsV * pars.NeighborsVertical;
			score += Row.Count[previous] * pars.FLoor;

			return score;
		}
	}
}
