using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public class SimpleEvaluator : IEvaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		private static readonly byte[] Neighbors = new byte[1024];
		static SimpleEvaluator()
		{
			for (short i = 1; i < 1024; i++)
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
		}

		public class Parameters
		{
			public Parameters()
			{
				RowWeights = new int[21];

				for (var i = 0; i < 21; i++)
				{
					RowWeights[i] = (20 - i) * -1000;
				}
				Points = 10000;
				Holes = -1000;
				Blockades = -500;
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
				// 161,878  206:33 150.88 (  246), ID:   1491, Max: 538
				{
					RowWeights = new int[] { -2080, -1897, -1665, -1766, -1769, -1372, -1387, -1067, -1273, -1068, -1110, -824, -728, -647, -600, -494, -301, -304, -218, -34, -160 },
					Points = 8446,
					Combo = -196,
					Holes = -59,
					Blockades = -312,
					Walls = 1124,
					Floor = 502,
					Neighbors = 522,
				};
			}
		}

		public Parameters pars { get; set; }

		public double GetScore(Field field)
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

				score += RowCount.Get(row) * pars.RowWeights[r];

				if ((row & MaskWallLeft) != 0) { walls++; }
				if ((row & MaskWallRight) != 0) { walls++; }

				var holesMask = filterTopColomns & (1023 ^ row);

				holes += RowCount.Get(holesMask);
				blokades += RowCount.Get(filterBlocades & row);

				filterBlocades |= holesMask;
				filterTopColomns |= row;

				neighbors += Neighbors[row];
				neighbors += RowCount.Get(row & previous);
				previous = row;
			}
			score += field[field.RowCount - 1].Count * pars.Floor;
			score += walls * pars.Walls;
			score += holes * pars.Holes;
			score += blokades * pars.Blockades;
			score += neighbors * pars.Neighbors;

			return score;
		}
	}
}
