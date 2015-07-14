using System;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public struct SimScore : IComparable, IComparable<SimScore>
	{
		public const int WinningScore = 80;
		public const int MaximumTurns = 200;

		public const byte Win = 2;
		public const byte Draw = 1;
		public const byte Lost = 0;

		private static readonly char[] res = { '0', '½', '1' };
		
		public SimScore(int turns, int score)
		{
			if (turns < MaximumTurns && score >= WinningScore)
			{
				Result = Win;
			}
			else if (turns == MaximumTurns && score == WinningScore)
			{
				Result = Draw;
			}
			else
			{
				Result = Lost;
			}
			Turns = (byte)turns;
			Score = (byte)score;
		}
		public readonly byte Result;
		public readonly byte Turns;
		public readonly byte Score;

		public bool IsWin { get { return Score == Win; } }
		public bool IsDraw{ get { return Score == Draw; } }
		public bool IsLost { get { return Score == Lost; } }

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

			if (Result == Win)
			{
				c = Turns.CompareTo(other.Turns);
			}
			else
			{
				c = other.Turns.CompareTo(Turns);
			}
			if (c != 0) { return c; }
			return other.Score.CompareTo(Score);
		}
	}
}
