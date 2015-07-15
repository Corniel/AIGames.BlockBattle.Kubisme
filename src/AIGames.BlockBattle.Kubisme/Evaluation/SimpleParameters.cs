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
		public int Floor { get; set; }
		public int NeighborsHorizontal { get; set; }
		public int NeighborsVertical { get; set; }

		public static SimpleParameters GetDefault()
		{
			return new SimpleParameters()
			// 1,720,874  0.11:06:05 Score: 64.16%, Win: 119.1, Lose: 104.5 Runs: 47,270, ID: 67255
			{
				RowWeights = new int[] { -48, 38, -124, -86, -64, -35, -13, 0, 7, 16, 6, 12, 28, 25, 29, 22, 46, -17, 54, 32, 7 },
				NineRowWeights = new int[] { 18, 89, -1, -75, 18, -23, -62, -18, 65, 60, 86, -30, -3, -63, 34, 5, -24, 48, -39, 36, -65 },
				Points = 250,
				Combo = 101,
				Holes = -136,
				Blockades = -121,
				WallsLeft = 98,
				WallsRight = 14,
				Floor = -55,
				NeighborsHorizontal = -80,
				NeighborsVertical = 102,
			};
		}
	}
}
