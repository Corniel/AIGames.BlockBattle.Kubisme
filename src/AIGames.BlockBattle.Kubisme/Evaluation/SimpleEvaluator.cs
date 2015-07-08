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
				// 184  00:14:04.8166790 76.13, Max: 238
				{
					FreeRows = new int[] { -161, 251, 240, 143, -30, 557, -67, 359, -20, 166, 173, 292, 53, 48, 440, 242, 67, -11, 322, 169, 485 },
					Points = 8325,
					Combo = -115,
					Holes = -583,
					Blockades = -367,
					Walls = 513,
					Floor = 364,
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
