namespace AIGames.BlockBattle.Kubisme.Evaluation
{
	public class SimpleParameters : IParameters
	{
		public SimpleParameters()
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

		public static SimpleParameters GetDefault()
		{
			return new SimpleParameters()
			// 7,397  0.00:08:01 Score: 1.67%, Win: 147.0, Lose: 125.1 Runs: 60, ID: 124, Max: 1, Turns: 147, Points: 80
			{
				RowWeights = new int[] { 14, 1, -28, -75, -6, -83, -39, -14, -37, -18, -45, -44, 26, -37, -103, -11, -8, 39, 0, -14, 8 },
				NineRowWeights = new int[] { -102, -57, 32, -39, 35, -10, -105, 50, -118, -43, -23, 13, -90, -96, 21, 23, 6, 3, 84, 34, -36 },
				Points = -89,
				Combo = -62,
				Holes = -52,
				Blockades = -112,
				WallsLeft = 57,
				WallsRight = 13,
				FLoor = -11,
				NeighborsHorizontal = -115,
				NeighborsVertical = 92,
			};
		}
	}
}
