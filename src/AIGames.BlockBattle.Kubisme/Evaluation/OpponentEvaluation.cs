using System;
using System.Collections.Generic;

namespace AIGames.BlockBattle.Kubisme
{
	public class OpponentEvaluation : IEquatable<OpponentEvaluation>
	{
		public OpponentEvaluation(
			int points0, 
			int points1, 
			int points2, 
			int filled1, 
			int filled2, 
			Dictionary<Block, int> filled3, 
			Dictionary<Block, int> points3)
		{
			Points0 = points0;
			Points1 = points1;
			Points2 = points2;

			FirstFilled1 = filled1;
			FirstFilled2 = filled2;

			var garbage0 = points0 / 3;
			var garbage1 = points1 / 3;
			var garbage2 = points2 / 3;

			Garbage1 = garbage1 - garbage0;
			Garbage2 = garbage2 - garbage1;

			if (filled3 != null)
			{
				FirstFilled3 = filled3;
				Garbage3 = new Dictionary<Block, int>();

				foreach (var block in Block.All)
				{
					var fill = FirstFilled3[block];
					var garbage3 = fill / 3;
					Garbage3[block] = garbage3 - garbage2;
				}
			}

		}

		public int Count { get; set; }

		public int Points0 { get; private set; }
		public int Points1 { get; private set; }
		public int Points2 { get; private set; }
			   
		public int FirstFilled1 { get; private set; }
		public int FirstFilled2 { get; private set; }
		public Dictionary<Block, int> FirstFilled3 { get; private set; }

		public int Garbage1 { get; private set; }
		public int Garbage2 { get; private set; }
		public Dictionary<Block, int> Garbage3 { get; private set; }

		public bool Equals(OpponentEvaluation other)
		{
			return other != null &&
				this.Points0 == other.Points0 &&
				this.Points1 == other.Points1 &&
				this.Garbage1 == other.Garbage1 &&
				this.Garbage2 == other.Garbage2 &&
				this.FirstFilled1 == other.FirstFilled1 &&
				this.FirstFilled2 == other.FirstFilled2;
		}

		public override string ToString()
		{
			return String.Format("Points:({0}, {1}), Garbage({2}, {3}), Filled({4}, {5})",
				Points0, Points1,
				Garbage1, Garbage2,
				FirstFilled1, FirstFilled2);
		}

		
	}
}
