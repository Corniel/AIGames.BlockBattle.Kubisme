using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public class SimpleEvaluator : IEvaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		private static readonly byte[] Neighbors = new byte[Row.Locked + 1];
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
				Neighbors[i] = (byte)count;
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
			}

			public int[] RowWeights { get; set; }

			public int Points { get; set; }
			public int Combo { get; set; }
			public int Holes { get; set; }
			public int Blockades { get; set; }
			public int Walls { get; set; }
			public int Floor { get; set; }
			public int Neighbors { get; set; }

			public static Parameters GetDefault()
			{
				return new Parameters()
				// 615.647  524:53 169,72 (45.648), ID:    366, Max: 613
				{
					RowWeights = new int[] { -92, -32, -68, -178, -54, -114, -146, -56, -74, -67, -49, -64, -67, -52, -61, -48, -49, -48, -38, 67, 64 },
					Points = 75,
					Combo = -9,
					Holes = -11,
					Blockades = -13,
					Walls = 59,
					Floor = -55,
					Neighbors = 28,
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
			var neighbors = 0;
			ushort previous = 0;

			for (var r = rMin; r < field.RowCount; r++)
			{
				var row = field[r].row;

				score += RowWeights[Row.Count[row], r];

				if ((row & MaskWallLeft) != 0) { walls++; }
				if ((row & MaskWallRight) != 0) { walls++; }

				var holesMask = filterTopColomns & (1023 ^ row);

				holes += Row.Count[holesMask];
				blokades += Row.Count[filterBlocades & row];

				filterBlocades |= holesMask;
				filterTopColomns |= row;

				neighbors += Neighbors[row];
				neighbors += Row.Count[row & previous];
				previous = row;
			}
			score += Row.Count[field[field.RowCount - 1].row] * pars.Floor;
			score += walls * pars.Walls;
			score += holes * pars.Holes;
			score += blokades * pars.Blockades;
			score += neighbors * pars.Neighbors;

			return score;
		}
	}
}
