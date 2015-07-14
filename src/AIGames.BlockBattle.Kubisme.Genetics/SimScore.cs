using System;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public struct SimScore : IComparable, IComparable<SimScore>
	{
		public const byte WIN = 2;
		public const byte DRAW = 1;
		public const byte LOST = 0;

		private static readonly char[] res = { '0', '½', '1' };
		
		private SimScore(byte res, int turns, int score)
		{
			Result = res;
			Turns = (byte)turns;
			Points = (byte)score;
		}
		public readonly byte Result;
		public readonly byte Turns;
		public readonly byte Points;

		public bool IsWin { get { return Result == SimScore.WIN; } }
		public bool IsDraw { get { return Result == SimScore.DRAW; } }
		public bool IsLost { get { return Result == SimScore.LOST; } }

		public override string ToString()
		{
			return String.Format("{0}, Turns: {1}, Points: {2}", res[Result], Turns, Points);
		}

		public int CompareTo(object obj)
		{
			return CompareTo((SimScore)obj);
		}
		public int CompareTo(SimScore other)
		{
			var c = other.Result.CompareTo(Result);
			if (c != 0) { return c; }

			if (Result == WIN)
			{
				c = Turns.CompareTo(other.Turns);
			}
			else
			{
				c = other.Turns.CompareTo(Turns);
			}
			if (c != 0) { return c; }
			return other.Points.CompareTo(Points);
		}

		public static SimScore Lost(int turns, int score)
		{
			return new SimScore(SimScore.LOST, turns, score);
		}
		public static SimScore Draw(int turns, int score)
		{
			return new SimScore(SimScore.DRAW, turns, score);
		}
		public static SimScore Win(int turns, int score)
		{
			return new SimScore(SimScore.WIN, turns, score);
		}
	}
}
