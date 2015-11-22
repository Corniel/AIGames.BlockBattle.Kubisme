using System;

namespace AIGames.BlockBattle.Kubisme
{
	public struct OpponentEvaluation
	{
		public OpponentEvaluation(int points0, int points1, int points2, int filled1, int filled2)
		{
			Points0 = points0;
			Points1 = points1;

			FirstFilled1 = filled1;
			FirstFilled2 = filled2;

			var garbage0 = points0 / 3;
			var garbage1 = points1 / 3;
			var garbage2 = points2 / 3;

			Garbage1 = garbage1 - garbage0;
			Garbage2 = garbage2 - garbage1;
		}

		public readonly int Points0;
		public readonly int Points1; 
		
		public readonly int FirstFilled1;
		public readonly int Garbage1;
		
		public readonly int FirstFilled2;
		public readonly int Garbage2;
		
		public override string ToString()
		{
			return String.Format("Points:({0}, {1}), Garbage({2}, {3}), Filled({4}, {5})",
				Points0, Points1,
				Garbage1, Garbage2,
				FirstFilled1, FirstFilled2);
		}
	}
}
