using System;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public struct SimScore : IComparable, IComparable<SimScore>
	{
		private static readonly char[] res = { '0', '½', '1' };
		private SimScore(byte r, int t, int sc)
		{
			Result = r;
			Turns = (byte)t;
			Score = (byte)sc;
		}
		public readonly byte Result;
		public readonly byte Turns;
		public readonly byte Score;

		public override string ToString()
		{
			return String.Format("{0}, Turns: {1}, Score: {2}", res[Result], Turns, Score);
		}


		public int CompareTo(object obj)
		{
			return CompareTo((SimScore)obj);
		}
		public int CompareTo(SimScore other)
		{
			var c = other.Result.CompareTo(Result);
			if (c != 0) { return c; }
			c = Turns.CompareTo(other.Turns);
			if (c != 0) { return c; }
			return other.Score.CompareTo(Score);
		}

		public static SimScore Create(int turns, int score)
		{
			if (turns < 200 && score > 80)
			{
				return new SimScore(2, turns, score);
			}
			if (turns == 200 && score == 80)
			{
				return new SimScore(1, turns, score);
			}
			return new SimScore(0, turns, score);
		}
	}
}
