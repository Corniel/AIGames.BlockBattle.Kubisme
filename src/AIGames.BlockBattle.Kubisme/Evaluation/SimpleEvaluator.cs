using AIGames.BlockBattle.Kubisme.Models;

namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public class SimpleEvaluator : IEvaluator
	{
		/// <summary>Mask pos 0 and 9;</summary>
		public const ushort MaskWallLeft = 0X0001;
		public const ushort MaskWallRight = 0X0200;

		public SimpleEvaluator()
		{
			pars = Parameters.GetDefault();
		}

		public class Parameters
		{
			public Parameters()
			{
				FreeRows = new int[21];
				Points = 1000;
				Combo = 100;
				Holes = -750;
				Blockades = -350;
				Walls = 500;
				Floor = 300;

				for (var i = 0; i < 21; i++)
				{
					FreeRows[i] = i + i * i;
				}
			}

			public int[] FreeRows { get; set; }

			public int Points { get; set; }
			public int Combo { get; set; }
			public int Holes { get; set; }
			public int Blockades { get; set; }
			public int Walls { get; set; }
			public int Floor { get; set; }

			public static Parameters GetDefault()
			{
				return new Parameters()
				// 5,865  07:46:59.4413994 101.96, Max: 269
				{
					FreeRows = new int[] { -64, -523, -180, -135, -137, -504, -283, 543, -80, 369, 141, 341, 2, 198, 575, 497, 147, 94, 143, -141, 447 },
					Points = 8445,
					Combo = -285,
					Holes = -477,
					Blockades = -608,
					Walls = 690,
					Floor = 532,
				};
			}
		}

		public Parameters pars{get;set;}

		public double GetScore(Field field)
		{
			var rMin = field.FirstNoneEmptyRow;

			var score = 0;
			score += field.Points * pars.Points;
			score += field.Combo * pars.Combo;
			score += pars.FreeRows[rMin];

			int filterTopColomns = 0;
			int filterBlocades = 0;
			var holes = 0;
			var walls = 0;
			var blokades = 0;

			for (var r = rMin; r < field.RowCount; r++)
			{
				var row = field[r].row;

				if ((row & MaskWallLeft) != 0) { walls++; }
				if ((row & MaskWallRight) != 0) { walls++; }

				var holesMask = filterTopColomns & (1023 ^ row);

				holes += RowCount.Get(holesMask);
				blokades += RowCount.Get(filterBlocades & row);

				filterBlocades |= holesMask;
				filterTopColomns |= row;
			}
			score += field[field.RowCount - 1].Count * pars.Floor;
			score += walls * pars.Walls;
			score += holes * pars.Holes;
			score += blokades * pars.Blockades;

			return score;
		}
	}
}
